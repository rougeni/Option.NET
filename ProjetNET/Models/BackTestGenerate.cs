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
        public List<PricingLibrary.Utilities.MarketDataFeed.DataFeed> generateHistory(String VanillaCallName, Share[] underlyingShares, DateTime refTime, double strike)
        {
            DataGestion dg = new DataGestion();
            return dg.getListDataField(refTime);
        }
    }
}
