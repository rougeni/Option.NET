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
using System.Runtime.InteropServices;

namespace ProjetNET.Models
{
    public class VanillaCallPricingModel : IPricing
    {
        [DllImport("wre-ensimag-c-4.1.dll", EntryPoint = "WREmodelingLogReturns", CallingConvention = CallingConvention.Cdecl)]

        public static extern int WREmodelingLogReturns(
            ref int nbValues,
            ref int nbAssets,
            double[,] assetsValues,
            ref int horizon,
            double[,] assetsReturns,
            ref int info
            );

        [DllImport("wre-ensimag-c-4.1.dll", EntryPoint = "WREanalysisExanteVolatility", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WREanalysisExanteVolatility(
            ref int nbAssets,
            double[,] cov,
            double[] weight,
            double[] exanteVolatility,
            ref int info);

        [DllImport("wre-ensimag-c-4.1.dll", EntryPoint = "WREmodelingCov", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WREmodelingCov(
            ref int nbValues,
            ref int nbAssets,
            double[,] assetsReturns,
            double[,] cov,
            ref int info
            );

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

        public void calculVolatility(List<DataFeed> listDataFeed)
        {
            /*public static extern int WREmodelingLogReturns(
                ref int nbValues,
                ref int nbAssets,
                double[,] assetsValues,
                ref int horizon,
                double[,] assetsReturns,
                int[] info
            );*/

            //calcul de la volatilité sur les 30 jours qui précèdent le début de la période
            int nbValues = listDataFeed.Count;
            int nbAssets = 1;
            double[,] assetsValues = new double[nbValues, nbAssets];
            int horizon = nbValues - 30;
            double[,] assetsReturns = new double[nbValues - horizon, nbAssets];
            int info = 10;

            int i = 0;
            foreach (DataFeed df in listDataFeed)
            {
                assetsValues[i, 0] = (double)df.PriceList[oShares[0].Id];
                i++;
            }
            int resultat = WREmodelingLogReturns(ref nbValues, ref nbAssets, assetsValues, ref horizon, assetsReturns, ref info);
            if (resultat != 0)
            {
                throw new ApplicationException("Erreur lors du calcul de la volatilité pour VaniliaCall: WREmodelingLogReturns, erreur numero : " + resultat);
            }

            /* 
            public static extern int WREmodelingCov(
                ref int nbValues,
                ref int nbAssets,
                double[,] assetsReturns,
                double[,] cov,
                ref int info
            );
             */
            double[,] cov = new double[1, 1];
            int nbValuesCov = 30;
            resultat = WREmodelingCov(ref nbValuesCov, ref nbAssets, assetsReturns, cov, ref info);
            if (resultat != 0)
            {
                throw new ApplicationException("Erreur lors du calcul de la volatilité pour VaniliaCall: WREmodelingCov erreur numero : " + resultat);
            }


            /*public static extern int WREanlysisExanteVolatility(
                ref int nbAssets,
                double[,] cov,
                double[] weight,
                double[] exanteVolatility,
                int info
             );*/

            double[] weight = new double[1];
            weight[0] = 1;
            double[] exanteVolatility = new double[1];

            resultat = WREanalysisExanteVolatility(ref nbAssets, cov, weight, exanteVolatility, ref info);
            if (resultat != 0)
            {
                throw new ApplicationException("Erreur lors du calcul de la volatilité pour VaniliaCall: WREanalysisExanteVolatility erreur numero : " + resultat);
            }

            oVolatility = new double[1];
            oVolatility[0] = exanteVolatility[0];
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
                    valeur = ancienPR.Deltas[0] * (double)df.PriceList[sousJacent] + (ancienneValeur - ancienPR.Deltas[0] * (double)ancienDF.PriceList[sousJacent]) * Math.Exp(tauxSR/businessDays);
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
