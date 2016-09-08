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

        public List<DataFeed> generateHistory(String VanillaCallName,Share[] underlyingShares, DateTime endTime, double strike)
        {
            ForwardData dg = new ForwardData();
            return dg.getForwardListDataField(VanillaCallName, underlyingShares, endTime, strike);
        }

    }
}
