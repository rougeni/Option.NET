using PricingLibrary.Computations;
using PricingLibrary.Utilities.MarketDataFeed;
using ProjetNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.ViewModels
{
    /**
    * 
     * Interface permettant de lie la vue et 
     * les differents model de pricing (Vanilla Call et Basket)
     * 
     * */
    public interface IPricingViewModel
    {
        #region Public Properties

        IPricing Pricing { get; }
        string Name { get; }

        #endregion Public Properties
    }
}
