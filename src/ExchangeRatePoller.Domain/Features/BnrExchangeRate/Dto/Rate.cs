namespace ExchangeRatePoller.Domain.Features.BnrExchangeRate.Dto
{
    using System.Xml.Serialization;

    public class Rate
    {
        [XmlAttribute("currency")]
        public string Currency { get; set; }

        [XmlAttribute("multiplier")]
        public decimal Multiplier { get; set; }

        [XmlText]
        public decimal Value { get; set; }
    }
}
