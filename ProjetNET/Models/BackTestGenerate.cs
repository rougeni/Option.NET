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
    class BackTestGenerate : IGenerateHistory
    {

        public String vanillaCallName { get; set; }
        public Share[] underlyingShares { get; set; }
        public double[] weight { get; set; }
        public DateTime endTime { get; set; }
        public double strike { get; set; }
        public DateTime startDate { get; set; }

        /**
         * Transmet la liste des dataFeed venant de la base de données
         * en précisant certains paramètres passés comme attributs de la classe
         * 
         * */
        public List<DataFeed> generateHistory()
        {
            DataGestion dg = new DataGestion();
            return dg.getListDataField(startDate, endTime,underlyingShares);
        }

    }
}
