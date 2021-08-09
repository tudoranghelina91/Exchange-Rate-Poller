using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ExchangeRatePoller.DataAccess.Features.BnrExchangeRate.Dto
{
    [BsonNoId]
    public class RateDto
    {
        public double Value { get; set; }

        public ObjectId CurrencyId { get; set; }
    }
}
