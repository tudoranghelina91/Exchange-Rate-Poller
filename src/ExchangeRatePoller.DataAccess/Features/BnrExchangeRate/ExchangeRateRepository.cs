using ExchangeRatePoller.DataAccess.Features;
using ExchangeRatePoller.DataAccess.Features.BnrExchangeRate.Dto;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace ExchangeRatePoller.DataAccess
{
    public class ExchangeRateRepository : IExchangeRateRepository
    {
        private readonly ExchangeRateContext _context;

        public ExchangeRateRepository(IOptions<DbSettings> settings)
        {
            _context = new ExchangeRateContext(settings);
        }

        public void InsertCube(CubeDto cubeDto)
        {
            _context.Cubes.InsertOne(cubeDto);
        }

        public async Task InsertCurrency(CurrencyDto currencyDto)
        {
            await _context.Currencies.InsertOneAsync(currencyDto);
        }

        public async Task<CurrencyDto> GetCurrencyByCode(string currencyCode)
        {
            var filter = Builders<CurrencyDto>.Filter.Eq("code", currencyCode);
            return (await _context.Currencies.FindAsync(filter)).FirstOrDefault();
        }

        public async Task<CurrencyDto> GetCurrencyById(ObjectId currencyId)
        {
            var filter = Builders<CurrencyDto>.Filter.Eq("_id", currencyId);
            return (await _context.Currencies.FindAsync(filter)).FirstOrDefault();
        }

        public async Task<CubeDto> GetCubeByDate(DateTime date)
        {
            var filter = Builders<CubeDto>.Filter.Eq("Date", date);
            return (await _context.Cubes.FindAsync(filter)).FirstOrDefault();
        }
    }
}
