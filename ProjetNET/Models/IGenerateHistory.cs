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

        String VanillaCallName { get; set;}
        Share[] underlyingShares { get; set; }
        double[] weight { get; set; }
        DateTime endTime { get; set; }
        double strike { get; set; }


        List<DataFeed> generateHistory();
    }
}
