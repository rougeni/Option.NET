﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary.Computations;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using ProjetNET.Data;

namespace ProjetNET.Models
{
    public class VanillaCallPricingModel : IPricing
    {

        private Pricer vanillaPricer;

        public VanillaCallPricingModel()
        {
            vanillaPricer = new Pricer();
        }


        public List<PricingResults> pricingUntilMaturity(List<DataFeed> listDataFeed)
        {
            if (oName.Equals(null) || oShares.Equals(null) || oMaturity == null || oStrike.Equals(null) || oSpot.Equals(null) || listDataFeed.Count == 0)
            {
                throw new NullReferenceException();  // TODO pls check if correct
            }
            List<PricingResults> listPrix = new List<PricingResults>();
            calculVolatility(listDataFeed);

            //DateTime startDate = new DateTime(2015, 8, 1);//currentDate;
            foreach (DataFeed df in listDataFeed)
            {
                if (df.Date <= oMaturity)
                {
                    listPrix.Add(vanillaPricer.PriceCall(new VanillaCall(oName, oShares, oMaturity, oStrike), df.Date, 252, oSpot[0], oVolatility[0]));
                }
                else
                {
                    break;
                }
            }
            listPrix.Add(getPayOff(listDataFeed));
            
            return listPrix;
        }

        public PricingResults getPayOff(List<DataFeed> listDataFeed)
        {
            if (oName.Equals(null) || oShares.Equals(null) || oMaturity == null || oStrike.Equals(null) || oSpot.Equals(null) || listDataFeed.Count == 0)
            {
                throw new NullReferenceException();  // TODO pls check if correct
            }
            calculVolatility(listDataFeed);
            return vanillaPricer.PriceCall(new VanillaCall(oName, oShares, oMaturity, oStrike), oMaturity, 252, oSpot[0], oVolatility[0]);
        }

        private void calculVolatility(List<DataFeed> listDataFeed)
        {
            double[] prix = new double[listDataFeed.Count];
            int i = 0;

            // Calculer les prix de toutes les actions dans l'option (ici en théorie qu'une seule..)
            foreach (DataFeed dataF in listDataFeed)
            {
                Dictionary<string, decimal> dico = dataF.PriceList;
                prix[i] = (double)dico[oShares[0].Id];
                i++;
            }
            
            
            double variance = 0;     
            double avg = 0;
            for (int line = 0; line < listDataFeed.Count-1; line++)
            {
                variance += Math.Pow(Math.Log10(prix[line+1] / prix[line]), 2);
                avg += Math.Log10(prix[line + 1] / prix[line]);
            }

            variance = variance / listDataFeed.Count - Math.Pow(avg / listDataFeed.Count, 2);
            oVolatility = new double[1];
            oVolatility[0] = Math.Sqrt(variance);
        }


        #region Getter & Setter
        public string oName { get; set; }

        public Share[] oShares { get; set; }

        public DateTime oMaturity { get; set; }

        public double oStrike { get; set; }


        public DateTime currentDate { get; set; }

        public double[] oSpot { get; set; }


        public double[] oVolatility { get; set; }

        #endregion Getter & Setter
    }
}
