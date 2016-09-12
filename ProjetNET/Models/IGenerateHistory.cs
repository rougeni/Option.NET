using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary.Utilities.MarketDataFeed;
using PricingLibrary.FinancialProducts;

namespace ProjetNET.Models
{
    /**
     * Interface to provide a structure for the creation of an history - backward or forwad.
     * 
     * */
    public interface IGenerateHistory
    {

        String vanillaCallName { get; set;}
        Share[] underlyingShares { get; set; }
        double[] weight { get; set; }
        DateTime endTime { get; set; }
        double strike { get; set; }
        DateTime startDate { get; set; }

        List<DataFeed> generateHistory();
    }
}
