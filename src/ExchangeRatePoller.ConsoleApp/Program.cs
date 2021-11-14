using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ExchangeRatePoller.DataAccess;
using ExchangeRatePoller.Domain.Features.BnrExchangeRate.Services;

namespace ExchangeRatePoller.ConsoleApp
{
    class Program
    {
        static Task Main(string[] args) =>
            CreateHostBuilder(args).Build().RunAsync();

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((services) =>
                {
                    services
                        .AddTransient<IExchangeRateRepository, ExchangeRateRepository>()
                        .AddTransient<IExchangeRateAdapter, BnrExchangeRateAdapter>()
                        .AddAutoMapper(typeof(ExchangeRateMappingProfile))
                        .AddHostedService<ExchangeRateRunner>();

                    services.Configure<DbSettings>(options =>
                    {
                        var config = new ConfigurationBuilder()
                            .AddJsonFile("Config/appsettings.json")
                            .Build();

                        options.ConnectionString = config.GetConnectionString(
                            config.GetSection("Databases")["ExchangeRates"]);

                        options.DatabaseName = config.GetSection("Databases")["ExchangeRates"];
                    });
                });
    }
}
