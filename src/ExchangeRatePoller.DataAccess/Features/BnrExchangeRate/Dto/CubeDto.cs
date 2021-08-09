using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ExchangeRatePoller.DataAccess.Features.BnrExchangeRate.Dto
{
    public class CubeDto
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public DateTime Date { get; set; }

        public RateDto[] Rates { get; set; }
    }
}
