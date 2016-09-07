using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary.Utilities.MarketDataFeed;

namespace ProjetNET.Models
{
    internal interface IGenerateHistory
    {
        List<DataFeed> generateHistory(DateTime time);
    }
}
