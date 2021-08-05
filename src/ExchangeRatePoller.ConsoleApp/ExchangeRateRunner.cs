using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRatePoller.ConsoleApp
{
    public class ExchangeRateRunner : BackgroundService, IExchangeRateRunner
    {
        private IExchangeRateService _exchangeRateService;

        private int pollingHour = 23;
        private int pollingMinute = 59;
        private int pollingSecond = 59;

        public ExchangeRateRunner(IExchangeRateService exchangeRateService)
        {
            this._exchangeRateService = exchangeRateService;
            exchangeRateService.GetBnrExchangeRates();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Starting Bnr Runner");
            while (!stoppingToken.IsCancellationRequested)
            {
                var utcNow = DateTime.UtcNow;

                if (utcNow.Minute % 2 == 0)
                {
                    utcNow.AddMinutes(-1);
                    Console.WriteLine("Getting BNR Exchange Rate");
                    await _exchangeRateService.GetBnrExchangeRates();
                    Console.WriteLine("DONE Getting BNR Exchange Rate");
                }
            }

        }
    }
}
