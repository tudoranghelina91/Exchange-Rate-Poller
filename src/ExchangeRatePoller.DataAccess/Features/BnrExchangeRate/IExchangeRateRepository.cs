using ExchangeRatePoller.DataAccess.Features.BnrExchangeRate.Dto;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace ExchangeRatePoller.DataAccess
{
    public interface IExchangeRateRepository
    {
        public Task<CubeDto> GetCubeByDate(DateTime date);

        public void InsertCube(CubeDto cubeDto);
        
        public Task InsertCurrency(CurrencyDto currencyDto);
        
        public Task<CurrencyDto> GetCurrencyByCode(string currencyCode);

        public Task<CurrencyDto> GetCurrencyById(ObjectId currencyId);
    }
}