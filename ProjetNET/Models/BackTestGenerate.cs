using ProjetNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetNET.Data;

namespace ProjetNET.ViewModels
{
    class BackTestGenerate : IGenerateHistory
    {
        public List<PricingLibrary.Utilities.MarketDataFeed.DataFeed> generateHistory(DateTime timeStart)
        {
            DataGestion dg = new DataGestion();
            return dg.getListDataField(timeStart);
        }
    }
}
