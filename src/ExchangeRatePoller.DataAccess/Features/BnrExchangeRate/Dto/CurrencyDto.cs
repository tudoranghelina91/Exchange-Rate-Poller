using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ExchangeRatePoller.DataAccess.Features.BnrExchangeRate.Dto
{
    public class CurrencyDto
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("code")]
        public string Code { get; set; }

        [BsonElement("multiplier")]
        public double Multiplier { get; set; }
    }
}
