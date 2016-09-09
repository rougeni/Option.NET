using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary.Computations;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using ProjetNET.Data;
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

        public VanillaCallPricingModel()    
        {
            vanillaPricer = new Pricer();
            oName = "Vanilla";
        }


        public List<PricingResults> pricingUntilMaturity(List<DataFeed> listDataFeed)
        {
            if (oName.Equals(null) || oShares.Equals(null) || oMaturity == null || oStrike.Equals(null) || listDataFeed.Count == 0)
            {
                throw new NullReferenceException();  // TODO pls check if correct
            }
            List<PricingResults> listPrix = new List<PricingResults>();
            calculVolatility(listDataFeed);
            //DateTime startDate = new DateTime(2015, 8, 1);//currentDate;
            foreach (DataFeed df in listDataFeed)
            {
                double listPrice = (double)df.PriceList[oShares[0].Id];
                oSpot[0] = listPrice;
                listPrix.Add(vanillaPricer.PriceCall(new VanillaCall(oName, oShares, oMaturity, oStrike), df.Date, 252, oSpot[0], oVolatility[0]));
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

            return vanillaPricer.PriceCall(new VanillaCall(oName, oShares, oMaturity, oStrike), oMaturity, 252, oSpot[0], oVolatility[0]);
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
            double[,] assetsReturns = new double[nbValues-horizon,nbAssets];
            int info = 10;

            int i = 0;
            foreach (DataFeed df in listDataFeed)
            {
                assetsValues[i, 0] = (double)df.PriceList[oShares[0].Id];
                i++;
            }
            int resultat = WREmodelingLogReturns(ref nbValues, ref nbAssets, assetsValues, ref horizon, assetsReturns, ref info);
            if (resultat != 0){
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
            resultat = WREmodelingCov(ref nbValues, ref nbAssets, assetsReturns, cov, ref info);
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
