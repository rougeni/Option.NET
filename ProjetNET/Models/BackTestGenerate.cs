using ProjetNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetNET.Data;
using PricingLibrary.FinancialProducts;

namespace ProjetNET.Models
{
    class BackTestGenerate : IGenerateHistory
    {
        public List<PricingLibrary.Utilities.MarketDataFeed.DataFeed> generateHistory()
        {
            DataGestion dg = new DataGestion();
            return dg.getListDataField(endTime);
        }

        public string VanillaCallName
        {
            get
            {
                return VanillaCallName;
            }
            set
            {
                VanillaCallName = value;
            }
        }

        public Share[] underlyingShares
        {
            get
            {
                return underlyingShares;
            }
            set
            {
                underlyingShares = value;
            }
        }

        public double[] weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
            }
        }

        public DateTime endTime
        {
            get
            {
                return endTime;
            }
            set
            {
                endTime = value;
            }
        }

        public double strike
        {
            get
            {
                return strike;
            }
            set
            {
                strike = value;
            }
        }

    }
}
