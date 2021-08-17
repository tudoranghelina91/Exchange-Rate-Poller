using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ExchangeRatePoller.DataAccess;
using ExchangeRatePoller.DataAccess.Features.BnrExchangeRate.Dto;
using ExchangeRatePoller.Domain.Features.BnrExchangeRate.Dto;
using ExchangeRatePoller.Domain.Features.BnrExchangeRate.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ExchangeRatePoller.PollExchangeRatesFunction
{
    public class PollExchangeRates
    {
        private readonly IExchangeRateAdapter _exchangeRateService;
        private readonly IMapper _mapper;
        private readonly IExchangeRateRepository _exchangeRateRepository;

        [FunctionName("PollExchangeRates")]
        public async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"{DateTime.Now} - Getting BNR Exchange Rate");

            var dataSet = await _exchangeRateService.GetExchangeRates();

            foreach (var cube in dataSet.Body.Cubes)
            {
                var domainCube = this._mapper.Map<Cube, Domain.Features.BnrExchangeRate.Models.Cube>(cube);
                var cubeDto = this._mapper.Map<Domain.Features.BnrExchangeRate.Models.Cube, CubeDto>(domainCube);

                var domainCubeRates = domainCube.Rates;

                await InsertExchangeRates(cubeDto, domainCubeRates);

                log.LogInformation($"{DateTime.Now} - Done getting BNR Exchange Rate");
            }
        }

        public PollExchangeRates(IExchangeRateAdapter exchangeRateService, IMapper mapper, IExchangeRateRepository exchangeRateRepository)
        {
            _exchangeRateService = exchangeRateService;
            _mapper = mapper;
            _exchangeRateRepository = exchangeRateRepository;
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
