using ExchangeRatePoller.DataAccess;
using ExchangeRatePoller.Domain.Features.BnrExchangeRate.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRatePoller.AzureFunction
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices((services) =>
                {
                    services
                        .AddTransient<IExchangeRateRepository, ExchangeRateRepository>()
                        .AddTransient<IExchangeRateAdapter, BnrExchangeRateAdapter>()
                        .AddAutoMapper(typeof(ExchangeRateMappingProfile));

                    services.Configure<DbSettings>(options =>
                    {
                        var config = new ConfigurationBuilder()
                            .AddJsonFile("local.settings.json")
                            .Build();

                        options.ConnectionString = config.GetConnectionString(
                            config.GetSection("DatabaseName").Value);

                        options.DatabaseName = config.GetSection("DatabaseName").Value;
                    });
                })
                .Build();

            host.Run();
        }
    }
}