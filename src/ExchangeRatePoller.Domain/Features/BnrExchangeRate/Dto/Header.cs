namespace ExchangeRatePoller.Domain.Features.BnrExchangeRate.Dto
{
    using System;

    public class Header
    {
        public string Publisher { get; set; }

        public DateTime PublishingDate { get; set; }

        public string MessageType { get; set; }
    }
}
