using System.Xml.Serialization;

namespace ExchangeRatePoller.Domain.Features.BnrExchangeRate.Dto
{
    [XmlRootAttribute(Namespace = "http://www.bnr.ro/xsd", IsNullable = true)]
    public class DataSet
    {
        public Header Header { get; set; }

        public Body Body { get; set; }
    }
}
