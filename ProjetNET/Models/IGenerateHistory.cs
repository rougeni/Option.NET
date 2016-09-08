using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary.Utilities.MarketDataFeed;
using PricingLibrary.FinancialProducts;

namespace ProjetNET.Models
{
    public interface IGenerateHistory
    {
        List<DataFeed> generateHistory(String VanillaCallName, Share[] underlyingShares, DateTime endTime, double strike);
    }
}
