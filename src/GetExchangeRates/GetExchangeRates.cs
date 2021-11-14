using System;
using System.Threading.Tasks;
using AutoMapper;
using ExchangeRatePoller.DataAccess;
using ExchangeRatePoller.DataAccess.Features.BnrExchangeRate.Dto;
using ExchangeRatePoller.Domain.Features.BnrExchangeRate.Dto;
using ExchangeRatePoller.Domain.Features.BnrExchangeRate.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace GetExchangeRates
{
    public class GetExchangeRates
    {
        private readonly IExchangeRateAdapter _exchangeRateService;
        private readonly IMapper _mapper;
        private readonly IExchangeRateRepository _exchangeRateRepository;

        [FunctionName("GetExchangeRates")]
        public async Task Run([TimerTrigger("* 30 12 * * * *")]TimerInfo myTimer, ILogger log)
        {
            var dataSet = await _exchangeRateService.GetExchangeRates();

            foreach (var cube in dataSet.Body.Cubes)
            {
                var domainCube = this._mapper.Map<Cube, ExchangeRatePoller.Domain.Features.BnrExchangeRate.Models.Cube>(cube);
                var cubeDto = this._mapper.Map<ExchangeRatePoller.Domain.Features.BnrExchangeRate.Models.Cube, CubeDto>(domainCube);

                var domainCubeRates = domainCube.Rates;
                
                for (int i = 0; i < domainCubeRates.Count; i++)
                {
                    var currency = await _exchangeRateRepository.GetCurrencyByCode(domainCubeRates[i].Currency.Code);

                    if (currency == null)
                    {
                        currency = this._mapper.Map<ExchangeRatePoller.Domain.Features.BnrExchangeRate.Models.Currency, CurrencyDto>(domainCubeRates[i].Currency);
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
}
