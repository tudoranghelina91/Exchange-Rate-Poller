using ExchangeRatePoller.ConsoleApp.BnrFxRatesDtos;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExchangeRatePoller.ConsoleApp
{
    public class BnrExchangeRateService : IExchangeRateService
    {
        public async Task<DataSet> GetBnrExchangeRates()
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
