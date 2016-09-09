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
    public interface IPricingViewModel
    {
        #region Public Properties

        IPricing Pricing { get; }
        string Name { get; }

        #endregion Public Properties
    }
}
