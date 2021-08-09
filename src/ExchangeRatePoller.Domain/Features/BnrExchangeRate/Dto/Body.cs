using System.Xml.Serialization;

namespace ExchangeRatePoller.Domain.Features.BnrExchangeRate.Dto
{
    public class Body
    {
        public string Subject { get; set; }

        public string OrigCurrency { get; set; }

        [XmlElement("Cube")]
        public Cube[] Cubes { get; set; }
    }
}
