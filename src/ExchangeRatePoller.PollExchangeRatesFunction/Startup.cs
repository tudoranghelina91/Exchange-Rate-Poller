using ExchangeRatePoller.DataAccess;
using ExchangeRatePoller.Domain.Features.BnrExchangeRate.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRatePoller.PollExchangeRatesFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IExchangeRateRepository, ExchangeRateRepository>()
                        .AddTransient<IExchangeRateAdapter, BnrExchangeRateAdapter>()
                        .AddAutoMapper(typeof(ExchangeRateMappingProfile));

            builder.Services.Configure<DbSettings>(options =>
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile("Config/appsettings.json")
                    .Build();

                options.ConnectionString = config.GetConnectionString(
                    config.GetSection("Databases")["ExchangeRates"]);

                options.DatabaseName = config.GetSection("Databases")["ExchangeRates"];
            });
        }
    }
}
