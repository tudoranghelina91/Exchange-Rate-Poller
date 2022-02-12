using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ExchangeRatePoller.DataAccess;
using ExchangeRatePoller.DataAccess.Features.BnrExchangeRate.Dto;
using ExchangeRatePoller.Domain.Features.BnrExchangeRate.Dto;
using ExchangeRatePoller.Domain.Features.BnrExchangeRate.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ExchangeRatePoller.AzureFunction
{
    public class FetchExchangeRate
    {
        private readonly IExchangeRateAdapter _exchangeRateAdapter;
        private readonly IMapper _mapper;
        private readonly IExchangeRateRepository _exchangeRateRepository;

        public FetchExchangeRate(IExchangeRateAdapter exchangeRateAdapter, IMapper mapper, IExchangeRateRepository exchangeRateRepository)
        {
            this._exchangeRateAdapter = exchangeRateAdapter;
            this._mapper = mapper;
            this._exchangeRateRepository = exchangeRateRepository;
        }

        [FunctionName("FetchExchangeRate")]
        public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo timer, ILogger logger)
        {
            if (timer.IsPastDue)
            {
                logger.LogInformation("Timer is past due");
            }

            var dataSet = await _exchangeRateAdapter.GetExchangeRates();

            foreach (var cube in dataSet.Body.Cubes)
            {
                var domainCube = this._mapper.Map<Cube, Domain.Features.BnrExchangeRate.Models.Cube>(cube);
                var cubeDto = this._mapper.Map<Domain.Features.BnrExchangeRate.Models.Cube, CubeDto>(domainCube);

                var domainCubeRates = domainCube.Rates;

                await InsertExchangeRates(cubeDto, domainCubeRates);
            }
        }

        private async Task InsertExchangeRates(CubeDto cubeDto, IList<Domain.Features.BnrExchangeRate.Models.Rate> domainCubeRates)
        {
            for (int i = 0; i < domainCubeRates.Count; i++)
            {
                var currency = await _exchangeRateRepository.GetCurrencyByCode(domainCubeRates[i].Currency.Code);

                if (currency == null)
                {
                    currency = this._mapper.Map<Domain.Features.BnrExchangeRate.Models.Currency, CurrencyDto>(domainCubeRates[i].Currency);
                    await _exchangeRateRepository.InsertCurrency(currency);
                }

                currency = await _exchangeRateRepository.GetCurrencyByCode(domainCubeRates[i].Currency.Code);
                cubeDto.Rates[i].CurrencyId = currency.Id;
            }

            if (await this._exchangeRateRepository.GetCubeByDate(cubeDto.Date) == null)
            {
                this._exchangeRateRepository.InsertCube(cubeDto);
            }
        }
    }
}
