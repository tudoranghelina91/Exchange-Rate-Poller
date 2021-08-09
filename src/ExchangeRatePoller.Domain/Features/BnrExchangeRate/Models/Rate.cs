namespace ExchangeRatePoller.Domain.Features.BnrExchangeRate.Models
{
    public class Rate
    {
        public Currency Currency { get; set; }

        public double Value { get; set; }
    }
}
