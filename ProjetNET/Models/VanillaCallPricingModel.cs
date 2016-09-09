using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary.Computations;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using ProjetNET.Data;
using PricingLibrary.Utilities;

namespace ProjetNET.Models
{
    internal class VanillaCallPricingModel : IPricing
    {

        private Pricer vanillaPricer;
        private int businessDays = DayCount.CountBusinessDays(new DateTime(2014, 1, 1), new DateTime(2014, 12, 31));
        private double tauxSR;

        public VanillaCallPricingModel()    
        {
            vanillaPricer = new Pricer();
            oName = "Vanilla";
            tauxSR = PricingLibrary.Utilities.MarketDataFeed.RiskFreeRateProvider.GetRiskFreeRate();
            oSpot = new double[1];
        }


        public List<PricingResults> pricingUntilMaturity(List<DataFeed> listDataFeed)
        {
            if (oName.Equals(null) || oShares.Equals(null) || oMaturity == null || oStrike.Equals(null) || listDataFeed.Count == 0)
            {
                throw new NullReferenceException();  // TODO pls check if correct
            }
            List<PricingResults> listPrix = new List<PricingResults>();
            calculVolatility(listDataFeed);

            VanillaCall vanny = new VanillaCall(oName, oShares, oMaturity, oStrike);

            foreach (DataFeed df in listDataFeed)
            {
                double listPrice = (double)df.PriceList[oShares[0].Id];
                oSpot[0] = listPrice;
                listPrix.Add(vanillaPricer.PriceCall(vanny, df.Date, businessDays, oSpot[0], oVolatility[0]));
            }
            
            return listPrix;
        }

        public PricingResults getPayOff(List<DataFeed> listDataFeed)
        {
            if (oName.Equals(null) || oShares.Equals(null) || oMaturity == null || oStrike.Equals(null) || listDataFeed.Count == 0)
            {
                throw new NullReferenceException();  // TODO pls check if correct
            }
            calculVolatility(listDataFeed);

            for (int myShare = 0; myShare < oShares.Length; myShare++)
            {
                oSpot[myShare] = (double)listDataFeed[listDataFeed.Count - 1].PriceList[oShares[myShare].Id];
            }

            return vanillaPricer.PriceCall(new VanillaCall(oName, oShares, oMaturity, oStrike), oMaturity, businessDays, oSpot[0], oVolatility[0]);
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
            oVolatility = new double[oShares.Length];
            oVolatility[0] = Math.Sqrt(variance);
        }

        public List<Portefeuille> getPortefeuillesCouverture(List<DataFeed> listDataFeed, List<PricingResults> ListePricingResult)
        {
            List<Portefeuille> listePortefeuille = new List<Portefeuille>();
            IEnumerator<PricingResults> enumPR = ListePricingResult.GetEnumerator();
            IEnumerator<DataFeed> enumLDF = listDataFeed.GetEnumerator();
            bool estDebut = true;
            double valeur = 0;
            double ancienneValeur = 0;
            PricingResults ancienPR = null;
            DataFeed ancienDF = null;
            string sousJacent = oShares[0].Id;
            while(enumPR.MoveNext() && enumLDF.MoveNext())
            {
                PricingResults pr = (PricingResults)enumPR.Current;
                DataFeed df = (DataFeed)enumLDF.Current;
                if (estDebut)
                {
                    //calcul de PI0
                    valeur = (double)pr.Price;
                    estDebut = false;
                }
                else
                {
                    // calcul de PIn
                    valeur = ancienPR.Deltas[0] * (double)df.PriceList[sousJacent] + (ancienneValeur - ancienPR.Deltas[0] * (double)ancienDF.PriceList[sousJacent]) * Math.Exp(tauxSR/365);
                }
                ancienPR = pr;
                ancienDF = df;
                ancienneValeur = valeur;
                Portefeuille port = new Portefeuille(df.Date, valeur);
                listePortefeuille.Add(port);
            }
            return listePortefeuille;
        }

        #region Getter & Setter
        public string oName { get; set; }

        public Share[] oShares { get; set; }

        public DateTime oMaturity { get; set; }

        public double oStrike { get; set; }

        public DateTime currentDate { get; set; }

        public double[] oSpot { get; set; }

        public double[] oVolatility { get; set; }

        public double[] oWeights { get; set; }
        #endregion Getter & Setter

    }
}
