using PricingLibrary.Computations;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.Models
{
    public class BasketPricingModel : IPricing
    {

        private Pricer basketPricer;

        public BasketPricingModel()
        {
            basketPricer = new Pricer();
            oName = "Basket";
        }

        public List<PricingResults> pricingUntilMaturity(List<DataFeed> listDataFeed)
        {
            if (oName.Equals(null) || oShares.Equals(null) || oMaturity == null || oStrike.Equals(null) || oSpot.Equals(null) || listDataFeed.Count == 0 || oWeights.Equals(null))
            {
                throw new NullReferenceException();  // TODO pls check if correct
            }
            List<PricingResults> listPrix = new List<PricingResults>();

            foreach (DataFeed df in listDataFeed)
            {
                if (df.Date <= oMaturity)
                {
                    listPrix.Add(basketPricer.PriceBasket(new BasketOption(oName, oShares, oWeights, oMaturity, oStrike), df.Date, 252, oSpot, oVolatility, null));
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
            if (oName.Equals(null) || oShares.Equals(null) || oMaturity == null || oStrike.Equals(null) || oSpot.Equals(null) || listDataFeed.Count == 0 || oWeights.Equals(null))
            {
                throw new NullReferenceException();  // TODO pls check if correct
            }
            calculVolatility(listDataFeed);

            return basketPricer.PriceBasket(new BasketOption(oName, oShares, oWeights, oMaturity, oStrike), oMaturity, 252, oSpot, oVolatility, null); //TODO Cholesky
        }

        private void calculVolatility(List<DataFeed> listDataFeed)
        {
            double[,] prix = new double[listDataFeed.Count, oShares.Length];
            //double[,] rendement = new double[listDataFeed.Count - 1, oShares.Length];
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


            double[] variance = new double[oShares.Length];
            double[] avg = new double[oShares.Length];
            for (int col = 0; col < oShares.Length; col++)
            {
                variance[col] = 0;
                avg[col] = 0;
            }

            for (int line = 0; line < listDataFeed.Count; line++)
            {
                for (int col = 0; col < oShares.Length; col++)
                {
                    variance[col] += Math.Pow(Math.Log10(prix[line, col] / prix[line - 1, col]), 2);
                    avg[col] += Math.Log10(prix[line, col] / prix[line - 1, col]);
                }
            }


            for (int col = 0; col < oShares.Length; col++)
            {
                variance[col] = variance[col] / listDataFeed.Count - Math.Pow(avg[col] / listDataFeed.Count, 2);
                oVolatility[col] = Math.Sqrt(variance[col]);
            }
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

        public double[] oSpot
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


        public double[] oVolatility
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


        public double[] oWeights
        {
            get
            {
                return oWeights;
            }
            set
            {
                oWeights = value;
                    ;
            }
        }
        #endregion Getter & Setter
    }
}
