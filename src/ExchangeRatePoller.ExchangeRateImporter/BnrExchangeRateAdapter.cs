using ExchangeRate.Domain.Dto;
using ExchangeRate.Domain.Services;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ExchangeRatePoller.ExchangeRateImporter
{
    public class BnrExchangeRateAdapter : IExchangeRateAdapter
    {
        public async Task<DataSet> GetExchangeRates(int year = 0)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri($"https://www.bnr.ro/files/xml/years/nbrfxrates{year}.xml");
                var xmlDoc = await httpClient.GetAsync(httpClient.BaseAddress);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataSet));
                try
                {
                    return xmlSerializer.Deserialize(await xmlDoc.Content.ReadAsStreamAsync()) as DataSet;
                    
                }
                catch
                {
                    var xmlDocString = await xmlDoc.Content.ReadAsStringAsync();
                    xmlDocString = xmlDocString.Replace(">-<", ">0<");
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(xmlDocString);
                    var textReader = new StringReader(xmlDocString);
                    return xmlSerializer.Deserialize(textReader) as DataSet;
                }
            }
        }
    }
}
