using ExchangeRatePoller.AzureFunction;
using ExchangeRatePoller.DataAccess;
using ExchangeRatePoller.Domain.Features.BnrExchangeRate.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                .Configure<DbSettings>(options =>
                {
                    var config = new ConfigurationBuilder()
                        .AddJsonFile("local.settings.json")
                        .Build();

                    options.ConnectionString = config.GetConnectionString("ExchangeRates");
                    options.DatabaseName = "ExchangeRates";
                })
                .AddAutoMapper(typeof(ExchangeRateMappingProfile));
        }
    }
}
