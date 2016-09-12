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

        /**
         * Fonction qui transmet les données simulées à partir des paramèttres internes de la classe
         * */
        public List<DataFeed> generateHistory()
        {
            ForwardData dg = new ForwardData();
            return dg.getForwardListDataField(vanillaCallName, underlyingShares, weight, startDate, endTime, strike);
        }

        #region Getteur Setteur
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
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value;
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
        #endregion Getteur Setteur

        #region Public Properties
        public string vanillaCallName { get; set; }

        public Share[] underlyingShares { get; set; }

        public double[] weight { get; set; }

        public DateTime endTime { get; set; }

        public DateTime startDate { get; set; }

        public double strike { get; set; }
        #endregion Public Properties
    }
}
