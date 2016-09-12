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
        #region Extern Declaration
        [DllImport("wre-ensimag-c-4.1.dll", EntryPoint = "WREmodelingLogReturns", CallingConvention = CallingConvention.Cdecl)]

        public static extern int WREmodelingLogReturns(
            ref int nbValues,
            ref int nbAssets,
            double[,] assetsValues,
            ref int horizon,
            double[,] assetsReturns,
            ref int info
            );

        [DllImport("wre-ensimag-c-4.1.dll", EntryPoint = "WREanalysisExpostVolatility", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WREanalysisExpostVolatility(
            ref int nbValues,
            double[] portfolioReturns,
            ref double expostVolatility,
            ref int info);


        [DllImport("wre-ensimag-c-4.1.dll", EntryPoint = "WREmodelingCov", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WREmodelingCov(
            ref int nbValues,
            ref int nbAssets,
            double[,] assetsReturns,
            double[,] cov,
            ref int info
            );
        #endregion Extern Declaration


        #region private fields
        private Pricer vanillaPricer;
        private int businessDays = DayCount.CountBusinessDays(new DateTime(2014, 1, 1), new DateTime(2014, 12, 31));  // Convention of businessdays a year
        private double tauxSR;
        #endregion private fields

        #region public methods

        /**
         * Constrctor for a VanillaPricingModel.
         * 
         * */
        public VanillaCallPricingModel()    
        {
            vanillaPricer = new Pricer();
            oName = "Vanilla";
            tauxSR = PricingLibrary.Utilities.MarketDataFeed.RiskFreeRateProvider.GetRiskFreeRate();
            oSpot = new double[1];
            oObservation = 30;
        }

        /*
         * Method to calculate all the pricings in between a certain date and the maturity.
         * 
         * */
        public List<PricingResults> pricingUntilMaturity(List<DataFeed> listDataFeed)
        {
            if (oName.Equals(null) || oShares.Equals(null) || oMaturity == null || oStrike.Equals(null) || listDataFeed.Count == 0)
            {
                throw new NullReferenceException();
            }
            List<PricingResults> listPrix = new List<PricingResults>();
            List<DataFeed> listdf = new List<DataFeed>(listDataFeed);


            VanillaCall vanny = new VanillaCall(oName, oShares, oMaturity, oStrike);

            while (listdf.Count > oObservation)
            {
                calculVolatility(listdf);
                double listPrice = (double)listdf[oObservation].PriceList[oShares[0].Id];
                oSpot[0] = listPrice;
                listPrix.Add(vanillaPricer.PriceCall(vanny, listdf[oObservation].Date, businessDays, oSpot[0], oVolatility[0]));
                vanillaPricer.PriceCall(vanny, listdf[oObservation].Date, businessDays, oSpot[0], oVolatility[0]);
                listdf.RemoveAt(0);

            }
            
            return listPrix;
        }

        /*
         * Method to return the Payoff at maturity.
         * */
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

        /*
         * Calculates the portefeuille de couverture.
         * */
        public List<Portefeuille> getPortefeuillesCouverture(List<DataFeed> listDataFeed, List<PricingResults> ListePricingResult)
        {

            List<Portefeuille> listePortefeuille = new List<Portefeuille>();
            IEnumerator<PricingResults> enumPR = ListePricingResult.GetEnumerator();
            int ind = 0;

            if (listDataFeed.Count < oObservation)
                throw new InvalidOperationException("Il y a moins de données que nécessaire à l'estimation.");

            while (ind < oObservation)
            {
                ind++;
                listDataFeed.RemoveAt(0);
            }
            IEnumerator<DataFeed> enumLDF = listDataFeed.GetEnumerator();
            bool estDebut = true;
            double valeur = 0;
            double ancienneValeur = 0;
            PricingResults ancienPR = null;
            DataFeed ancienDF = null;
            string sousJacent = oShares[0].Id;
            int waitForRebalancing = oRebalancement;

            double currentDelta = 0;
            while(enumPR.MoveNext() && enumLDF.MoveNext())
            {
                PricingResults pr = (PricingResults)enumPR.Current;
                DataFeed df = (DataFeed)enumLDF.Current;
                if (estDebut)
                {
                    valeur = (double)pr.Price;
                    estDebut = false;
                    currentDelta = pr.Deltas[0];
                }
                else
                {
                    if (waitForRebalancing == 0)
                    {
                        valeur = currentDelta * (double)df.PriceList[sousJacent] + (ancienneValeur - currentDelta * (double)ancienDF.PriceList[sousJacent]) * Math.Exp(tauxSR / businessDays);
                        waitForRebalancing = oRebalancement ;
                        currentDelta = pr.Deltas[0];
                    }
                    else
                    {
                        valeur = currentDelta * (double)df.PriceList[sousJacent] + (ancienneValeur - currentDelta * (double)ancienDF.PriceList[sousJacent]) * Math.Exp(tauxSR / businessDays);
                        waitForRebalancing--;
                    }
                }
                ancienPR = pr;
                ancienDF = df;
                ancienneValeur = valeur;
                Portefeuille port = new Portefeuille(df.Date, valeur);
                listePortefeuille.Add(port);
            }
            return listePortefeuille;
        }
        

      
        /*
         * Method to calculate the volatility and ovariance matrix.
         * */
        public void calculVolatility(List<DataFeed> listDataFeed)
        {
            //calcul de la volatilité sur les oObservation (objet de classe) jours qui précèdent le début de la période
            int nbValues = listDataFeed.Count;
            int nbAssets = 1;
            double[,] assetsValues = new double[nbValues, nbAssets];
            int horizon = nbValues - oObservation;
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

            double[,] cov = new double[1, 1];
            int nbValuesCov = oObservation;
            resultat = WREmodelingCov(ref nbValuesCov, ref nbAssets, assetsReturns, cov, ref info);
            if (resultat != 0)
            {
                throw new ApplicationException("Erreur lors du calcul de la volatilité pour VaniliaCall: WREmodelingCov erreur numero : " + resultat);
            }

            double[] weight = new double[1];
            weight[0] = 1;
            double exanteVolatility = 0;
            double[] rendements = new double[nbValues-horizon];
            for (int j = 0; j < nbValues-horizon; j++)
            {
                rendements[j] = assetsReturns[j,0];
            }
            int distTime = (nbValues - horizon);
                resultat = WREanalysisExpostVolatility(ref distTime , rendements, ref exanteVolatility, ref info);
            if (resultat != 0)
            {
                throw new ApplicationException("Erreur lors du calcul de la volatilité pour VaniliaCall: WREanalysisExanteVolatility erreur numero : " + resultat);
            }

            oVolatility = new double[1];
            oVolatility[0] = exanteVolatility;
        }

        #endregion public methods

        #region Getter & Setter
        public string oName { get; set; }

        public Share[] oShares { get; set; }

        public DateTime oMaturity { get; set; }

        public double oStrike { get; set; }

        public DateTime currentDate { get; set; }

        public double[] oSpot { get; set; }

        public double[] oVolatility { get; set; }

        public int oRebalancement { get; set; }

        public int oObservation { get; set; }

        public double[] oWeights { get; set; }
        #endregion Getter & Setter

    }
}
