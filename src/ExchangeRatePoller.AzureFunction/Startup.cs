using ExchangeRatePoller.AzureFunction;
using ExchangeRate.DataAccess;
using ExchangeRate.Domain.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using ExchangeRate.DataAccess.Config;

[assembly: FunctionsStartup(typeof(Startup))]
namespace ExchangeRatePoller.AzureFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .AddTransient<IExchangeRateRepository, ExchangeRateRepository>()
                .AddTransient<IExchangeRateAdapter, BnrExchangeRateAdapter>()
                .AddAutoMapper(typeof(ExchangeRateMappingProfile))
                .Configure<DbSettings>(options =>
                {
                    var config = new ConfigurationBuilder()
                        .AddJsonFile("local.settings.json", true)
                        .Build();

                    options.ConnectionString = config.GetConnectionString("ExchangeRates");
                    options.DatabaseName = "ExchangeRates";
                    
                    if (string.IsNullOrWhiteSpace(options.ConnectionString))
                    {
                        options.ConnectionString = Environment.GetEnvironmentVariable("ExchangeRates");
                    }
                });
        }
    }
}
