namespace ExchangeRatePoller.ConsoleApp.BnrFxRatesDtos
{
    using System;

    public class Header
    {
        public string Publisher { get; set; }

        public DateTime PublishingDate { get; set; }

        public string MessageType { get; set; }
    }
}
