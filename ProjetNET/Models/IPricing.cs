﻿using PricingLibrary.Computations;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.Models
{
    interface IPricing
    {
        List<PricingResults> pricingUntilMaturity(List<DataFeed> listDataFeed);

        PricingResults getPayOff(List<DataFeed> listDataFeed);

        // nom de l option
        String oName { get; set; }
        // tab des sous-jacent
        Share[] oShares { get; set; }
        DateTime oMaturity { get; set; }
        double oStrike { get; set; }
        // prix a la date de depart
        double[] oSpot { get; }
        double[] oVolatility { get; set; }
        DateTime currentDate { get; set; }
        double[] oWeights { get; set; }
    }
}
