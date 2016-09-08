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
    class VanillaCallPricingModel : IPricing
    {

        private Pricer vanillaPricer;
        private double[,] vanillaRendement;

        public VanillaCallPricingModel()
        {
            vanillaPricer = new Pricer();
        }


        public List<PricingResults> pricingUntilMaturity(List<DataFeed> dataFeed, DateTime maturity)
        {
            throw new NotImplementedException();
        }

        public PricingResults getPayOff(List<DataFeed> dataFeed)
        {
            calculVolatility(dataFeed);
            return vanillaPricer.PriceCall(new VanillaCall(oName, oShares, oMaturity, oStrike), oMaturity, 252, oSpot, oVolatility);
        }

        private void calculVolatility(List<DataFeed> listDataFeed)
        {
            double[,] prix = new double[listDataFeed.Count, oShares.Length];
            double[,] rendement = new double[listDataFeed.Count-1, oShares.Length];
            int i = 0;
            int j;
            // Calculer les prix de toutes les actions dans l'option (ici en théorie qu'une seule..)
            foreach (DataFeed dataF in listDataFeed)
            {
                j = 0;
                Dictionary<string, decimal> dico = dataF.PriceList;
                foreach (Share s in oShares)
                {
                    prix[i, j] = (double)dico[s.Id];
                    j++;
                }
                i++;
            }
            
            
            double variance = 0;     
            double avg = 0;
            for (int line = 0; line < listDataFeed.Count; line++)
            {
                for (int col = 0; col < oShares.Length; col++)
                {
                    variance += Math.Pow(Math.Log10(prix[line, col] / prix[line - 1, col]), 2);
                    avg += Math.Log10(prix[line, col] / prix[line - 1, col]);
                }
            }

            variance = variance / listDataFeed.Count - Math.Pow(avg / listDataFeed.Count, 2);
            oVolatility = Math.Sqrt(variance);
        }


        #region Getter & Setter
        public string oName
        {
            get
            {
                return oName;
            }
            set
            {
                oName = value;
            }
        }

        public Share[] oShares
        {
            get
            {
                return oShares;
            }
            set
            {
                oShares = value;
            }
        }

        public DateTime oMaturity
        {
            get
            {
                return oMaturity;
            }
            set
            {
                oMaturity = value;
            }
        }

        public double oStrike
        {
            get
            {
                return oStrike;
            }
            set
            {
                oStrike = value;
            }
        }
        


        public DateTime currentDate
        {
            get
            {
                return currentDate;
            }
            set
            {
                currentDate = value;
            }
        }

        public double oSpot
        {
            get
            {
                return oSpot;
            }
            set
            {
                oSpot = value;
            }
        }


        public double oVolatility
        {
            get
            {
                return oVolatility;
            }
            set
            {
                oVolatility = value;
            }
        }
        #endregion Getter & Setter
    }
}
