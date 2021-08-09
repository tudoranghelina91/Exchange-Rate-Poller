using ExchangeRatePoller.Domain.Features.BnrExchangeRate.Dto;
using System.Threading.Tasks;

namespace ExchangeRatePoller.Domain.Features.BnrExchangeRate.Services
{
    public interface IExchangeRateAdapter
    {
        public Task<DataSet> GetExchangeRates(int year = 0);
    }
}
