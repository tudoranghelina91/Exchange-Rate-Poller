using AutoMapper;
using ExchangeRate.DataAccess;
using ExchangeRate.Dto;
using ExchangeRate.Domain.Dto;
using ExchangeRate.Domain.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRatePoller.ExchangeRateImporter
{
    class ExchangeRateRunner : BackgroundService, IExchangeRateRunner
    {
        private IExchangeRateAdapter _exchangeRateService;
        private IExchangeRateRepository _exchangeRateRepository;
        private IMapper _mapper;
        
        public ExchangeRateRunner(IExchangeRateAdapter exchangeRateService, IMapper mapper, IExchangeRateRepository exchangeRateRepository)
        {
            this._exchangeRateService = exchangeRateService;
            this._mapper = mapper;
            this._exchangeRateRepository = exchangeRateRepository;
        }

        private async Task InsertExchangeRates(CubeDto cubeDto, IList<ExchangeRate.Domain.Models.Rate> domainCubeRates)
        {
            for (int i = 0; i < domainCubeRates.Count; i++)
            {
                var currency = await _exchangeRateRepository.GetCurrencyByCode(domainCubeRates[i].Currency.Code);

                if (currency == null)
                {
                    currency = this._mapper.Map<ExchangeRate.Domain.Models.Currency, CurrencyDto>(domainCubeRates[i].Currency);
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


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Starting Bnr Importer");
            for (int i = 2005; i <= DateTime.UtcNow.Year; i++)
            {
                Console.WriteLine($"{DateTime.UtcNow.ToLocalTime()} - Getting BNR Exchange Rate for {i}");
                var dataSet = await _exchangeRateService.GetExchangeRates(i);

                foreach (var cube in dataSet.Body.Cubes)
                {
                    var domainCube = this._mapper.Map<Cube, ExchangeRate.Domain.Models.Cube>(cube);
                    var cubeDto = this._mapper.Map<ExchangeRate.Domain.Models.Cube, CubeDto>(domainCube);

                    var domainCubeRates = domainCube.Rates;

                    await InsertExchangeRates(cubeDto, domainCubeRates);
                }

                Console.WriteLine($"{DateTime.UtcNow.ToLocalTime()} - DONE Getting BNR Exchange Rate for {i}");
                Thread.Sleep(TimeSpan.FromMinutes(5));
            }

            Console.WriteLine("Stopping Bnr Importer");
        }
    }
}
