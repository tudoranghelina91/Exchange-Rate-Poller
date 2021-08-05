using System.Threading.Tasks;
using ExchangeRatePoller.ConsoleApp.BnrFxRatesDtos;

namespace ExchangeRatePoller.ConsoleApp
{
    public interface IExchangeRateService
    {
        public Task<DataSet> GetBnrExchangeRates();
    }
}
