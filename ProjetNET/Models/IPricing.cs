using PricingLibrary.Computations;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.Models
{
    class IPricing
    {
        public List<PricingResults> pricingUntilMaturity(List<DataFeed> dataFeed, DateTime maturity);

        public PricingResults getPayOff(List<DataFeed> dataFeed);

    }
}
