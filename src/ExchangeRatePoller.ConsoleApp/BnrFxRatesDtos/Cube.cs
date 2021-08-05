using System.Xml.Serialization;

namespace ExchangeRatePoller.ConsoleApp.BnrFxRatesDtos
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
