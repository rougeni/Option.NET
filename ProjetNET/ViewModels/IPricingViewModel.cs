﻿using PricingLibrary.Computations;
using PricingLibrary.Utilities.MarketDataFeed;
using ProjetNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.ViewModels
{
    internal interface IPricingViewModel
    {
        #region Public Properties

        IPricing Initializer { get; }
        string Name { get; }

        #endregion Public Properties
    }
}