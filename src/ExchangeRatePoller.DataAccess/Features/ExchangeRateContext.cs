using ExchangeRatePoller.DataAccess.Features.BnrExchangeRate.Dto;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ExchangeRatePoller.DataAccess.Features
{
    public class ExchangeRateContext
    {
        private readonly IMongoDatabase _database;

        public ExchangeRateContext(IOptions<DbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            this._database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<CubeDto> Cubes
        {
            get => _database.GetCollection<CubeDto>("Cubes");
        }

        public IMongoCollection<CurrencyDto> Currencies
        {
            get => _database.GetCollection<CurrencyDto>("Currencies");
        }
    }
}
