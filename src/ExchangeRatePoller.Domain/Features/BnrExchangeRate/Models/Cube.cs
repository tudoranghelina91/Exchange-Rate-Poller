using System;
using System.Collections.Generic;

namespace ExchangeRatePoller.Domain.Features.BnrExchangeRate.Models
{
    public class Cube
    {
        public DateTime Date { get; set; }

        public IList<Rate> Rates { get; set; }
    }
}
