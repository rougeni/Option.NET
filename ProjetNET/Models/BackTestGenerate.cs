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

        public double[] weight { get; set; }

        public DateTime endTime { get; set; }

        public double strike { get; set; }

        public string vanillaCallName { get; set; }

        public Share[] underlyingShares { get; set; }

        public List<PricingLibrary.Utilities.MarketDataFeed.DataFeed> generateHistory()
        {
            DataGestion dg = new DataGestion();
            return dg.getListDataField(endTime);
        }

        public string VanillaCallName
        {
            get
            {
                return vanillaCallName;
            }
            set
            {
                vanillaCallName = value;
            }
        }


        public Share[] UnderlyingShares
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

        public double[] Weight
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

        public DateTime EndTime
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

        public double Strike
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
