using System;
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

            DateTime dateIterator = currentDate;
            while (!dateIterator.Equals(oMaturity))
            {
                listPrix.Add(vanillaPricer.PriceCall(new VanillaCall(oName, oShares, oMaturity, oStrike), dateIterator, 252, oSpot[0], oVolatility[0]));
                dateIterator.AddDays(1);
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
