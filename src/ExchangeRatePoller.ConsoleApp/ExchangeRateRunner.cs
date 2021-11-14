using ExchangeRatePoller.Domain.Features.BnrExchangeRate.Services;
using ExchangeRatePoller.Domain.Features.BnrExchangeRate.Dto;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ExchangeRatePoller.DataAccess;
using ExchangeRatePoller.DataAccess.Features.BnrExchangeRate.Dto;
using System.Collections.Generic;

namespace ExchangeRatePoller.ConsoleApp
{
    public class ExchangeRateRunner : BackgroundService, IExchangeRateRunner
    {
        private readonly IExchangeRateAdapter _exchangeRateService;
        private readonly IMapper _mapper;
        private readonly IExchangeRateRepository _exchangeRateRepository;

        public ExchangeRateRunner(IExchangeRateAdapter exchangeRateService, IMapper mapper, IExchangeRateRepository exchangeRateRepository)
        {
            this._exchangeRateService = exchangeRateService;
            this._mapper = mapper;
            this._exchangeRateRepository = exchangeRateRepository;
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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Starting Bnr Runner");

            var utcNow = DateTime.UtcNow.ToLocalTime();
            var nextRunTime = utcNow;
            bool firstRun = true;

            while (!stoppingToken.IsCancellationRequested)
            {
                while (DateTime.UtcNow.ToLocalTime().Hour < 12)
                {
                    if (DateTime.UtcNow.ToLocalTime().Second % 30 == 0)
                    {
                        Console.WriteLine("Waiting for 12:00");
                        Thread.Sleep(TimeSpan.FromSeconds(30));
                    }
                }

                utcNow = DateTime.UtcNow;

                if (firstRun || utcNow == nextRunTime)
                {
                    firstRun = false;

                    Console.WriteLine($"{DateTime.UtcNow.ToLocalTime()} - Getting BNR Exchange Rate");

                    var dataSet = await _exchangeRateService.GetExchangeRates();

                    foreach (var cube in dataSet.Body.Cubes)
                    {
                        var domainCube = this._mapper.Map<Cube, Domain.Features.BnrExchangeRate.Models.Cube>(cube);
                        var cubeDto = this._mapper.Map<Domain.Features.BnrExchangeRate.Models.Cube, CubeDto>(domainCube);

                        var domainCubeRates = domainCube.Rates;

                        await InsertExchangeRates(cubeDto, domainCubeRates);
                    }

                    Console.WriteLine($"{DateTime.UtcNow.ToLocalTime()} - DONE Getting BNR Exchange Rate");

                    nextRunTime = utcNow.AddDays(1);

                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
        }
    }
}
