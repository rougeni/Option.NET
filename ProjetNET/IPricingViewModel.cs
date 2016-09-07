using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary.Computations;
using PricingLibrary.Utilities.MarketDataFeed;

namespace ProjetNET.Models
{
    internal interface IPricingViewModel
    {

        #region Public Properties

        List<PricingResults> pricingUntilMaturity(List<DataFeed> dataFeed, DateTime maturity);

        PricingResults getPayOff(List<DataFeed> dataFeed);

        #endregion Public Properties

    }
}
