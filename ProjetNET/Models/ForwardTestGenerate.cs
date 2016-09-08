using ProjetNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetNET.Data;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;

namespace ProjetNET.Models
{
    public class ForwardTestGenerate : IGenerateHistory
    {


        public List<DataFeed> generateHistory()
        {
            ForwardData dg = new ForwardData();
            return dg.getForwardListDataField(VanillaCallName, underlyingShares, weight, endTime, strike);
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
