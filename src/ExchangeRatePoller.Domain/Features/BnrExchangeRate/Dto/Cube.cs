using System.Xml.Serialization;

namespace ExchangeRatePoller.Domain.Features.BnrExchangeRate.Dto
{
    [XmlType("Cube")]
    public class Cube
    {
        [XmlAttribute("date")]
        public string Date { get; set; }

        [XmlElement("Rate")]
        public Rate[] Rates { get; set; }
    }
}
