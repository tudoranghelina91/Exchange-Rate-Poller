using System.Xml.Serialization;

namespace ExchangeRatePoller.ConsoleApp.BnrFxRatesDtos
{
    public class Body
    {
        public string Subject { get; set; }
        
        public string OrigCurrency { get; set; }

        [XmlElement("Cube")]
        public Cube Cube { get; set; }
    }
}
