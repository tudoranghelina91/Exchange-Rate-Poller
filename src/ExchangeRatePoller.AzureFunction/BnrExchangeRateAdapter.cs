using ExchangeRate.Domain.Services;
using ExchangeRate.Domain.Dto;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExchangeRatePoller.AzureFunction
{
    public class BnrExchangeRateAdapter : IExchangeRateAdapter
    {
        public async Task<DataSet> GetExchangeRates(int year = 0)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://www.bnr.ro/nbrfxrates.xml");
                var xmlDoc = await httpClient.GetAsync(httpClient.BaseAddress);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataSet));
                return xmlSerializer.Deserialize(await xmlDoc.Content.ReadAsStreamAsync()) as DataSet;
            }
        }
    }
}
