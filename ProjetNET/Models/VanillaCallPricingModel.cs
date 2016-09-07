using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary.Computations;

namespace ProjetNET.Models
{
    class VanillaCallPricingModel : IPricingViewModel
    {

        private Pricer vanillaPricer;

        public VanillaCallPricingModel();


        public List<PricingLibrary.Computations.PricingResults> pricingUntilMaturity(List<PricingLibrary.Utilities.MarketDataFeed.DataFeed> dataFeed, DateTime maturity)
        {
            throw new NotImplementedException();
        }

        public PricingLibrary.Computations.PricingResults getPayOff(List<PricingLibrary.Utilities.MarketDataFeed.DataFeed> dataFeed)
        {
            throw new NotImplementedException();
        }
    }
}
