using PricingLibrary.Computations;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.Models
{
    public class BasketPricingModel : IPricing
    {


        [DllImport("wre-ensimag-c-4.1.dll", EntryPoint = "WREanlysisExanteVolatility", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WREanlysisExanteVolatility(
            ref int nbAssets,
            double[,] cav,
            double[] weight,
            double[] exanteVolatility,
            ref int info);


        [DllImport("wre-ensimag-c-4.1.dll", EntryPoint = "WREmodelingCov", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WREmodelingCov(
                    ref int nbValues,
                    ref int nbAssets,
                    double[,] assetsReturns,
                    double[,] cov,
                    ref int info);

        [DllImport("wre-ensimag-c-4.1.dll", EntryPoint = "WREmodelingLogReturns", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WREmodelingLogReturns(
                    ref int nbValues,
                    ref int nbAssets,
                    double[,] assetsValues,
                    ref int horizon,
                    double[,] assetsReturns,
                    ref int info);


        // import WRE dlls
        [DllImport("wre-ensimag-c-4.1.dll", EntryPoint = "WREmodelingCorr", CallingConvention = CallingConvention.Cdecl)]

        // declaration
        public static extern int WREmodelingCorr(
            ref int nbValues,
            ref int nbAssets,
            double[,] assetReturns,
            double[,] corr,
            ref int info
        );

        private Pricer basketPricer;
        private int businessDays = DayCount.CountBusinessDays(new DateTime(2014, 1, 1), new DateTime(2014, 12, 31));
        private double[,] matriceCorr;
        private double tauxSR;

        public BasketPricingModel()
        {
            basketPricer = new Pricer();
            oName = "Basket";
        }

        public List<PricingResults> pricingUntilMaturity(List<DataFeed> listDataFeed)
        {
            if (oName.Equals(null) || oShares.Equals(null) || oMaturity == null || oStrike.Equals(null) || listDataFeed.Count == 0 )
            {
                throw new NullReferenceException();  // TODO pls check if correct
            }
            List<PricingResults> listPrix = new List<PricingResults>();
            calculVolatility(listDataFeed);

            foreach (DataFeed df in listDataFeed)
            {
                for (int myShare = 0; myShare < oShares.Length; myShare++){
                    oSpot[myShare] = (double) df.PriceList[oShares[myShare].Id];
                }

                listPrix.Add(basketPricer.PriceBasket(new BasketOption(oName, oShares, oWeights, oMaturity, oStrike), df.Date, 252, oSpot, oVolatility, matriceCorr));
            }

            return listPrix;
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
            while (enumPR.MoveNext() && enumLDF.MoveNext())
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
                    valeur = produitScalaire(ancienPR.Deltas, df.PriceList) + (ancienneValeur - produitScalaire(ancienPR.Deltas, ancienDF.PriceList)) * Math.Exp(tauxSR / businessDays);
                }
                ancienPR = pr;
                ancienDF = df;
                ancienneValeur = valeur;
                Portefeuille port = new Portefeuille(df.Date, valeur);
                listePortefeuille.Add(port);
            }
            return listePortefeuille;
        }
        private double produitScalaire(double[] p, Dictionary<string, decimal> dictionary)
        {
            double val = 0;
            for (int i = 0; i < p.Length; i++)
            {
                val += p[i] * (double)dictionary[oShares[i].Id];
            }
            return val;
        }

        public PricingResults getPayOff(List<DataFeed> listDataFeed)
        {
            if (oName.Equals(null) || oShares.Equals(null) || oMaturity == null || oStrike.Equals(null) || listDataFeed.Count == 0 )
            {
                throw new NullReferenceException();  // TODO pls check if correct
            }
            calculVolatility(listDataFeed);

            for (int myShare = 0; myShare < oShares.Length; myShare++)
            {
                oSpot[myShare] = (double) listDataFeed[listDataFeed.Count-1].PriceList[oShares[myShare].Id];
            }

            return basketPricer.PriceBasket(new BasketOption(oName, oShares, oWeights, oMaturity, oStrike), oMaturity, 252, oSpot, oVolatility, matriceCorr);
        }

        public void calculVolatility(List<DataFeed> listDataFeed)
        {
        //Calcul des log rendements
            int nbAssets = listDataFeed.ToArray()[0].PriceList.Count;
            int nbValues = 30; // listDataFeed.Count();
            double[,] assetsValues = new double[nbValues,nbAssets];
            int horizon = 0;
            double[,] assetsReturns = new double[nbValues,nbAssets];
            int info = 0;
            int a =0;
            for (int i = 0; i < nbAssets; i++ )
            {
                int b = 0;
                foreach (var iden in oShares)
                {
                    assetsValues[a, b] = (double)listDataFeed.ToArray()[i].PriceList[iden.Id];
                }
                b++;
                a++;
            }
            int resultat = WREmodelingLogReturns(ref nbValues,ref nbAssets, assetsValues, ref horizon, assetsReturns,ref info);
            if (resultat != 0)
            {
                //throw new ApplicationException("Erreur lors du calcul de la volatilité pour Basket: WREmodelingLogReturns, erreur numero : " + resultat);
            }
            //calcul de la covariance
            double[,] cov = new double[nbAssets,nbAssets];
            resultat = WREmodelingCov(ref nbValues,ref nbAssets, assetsReturns, cov, ref info);
            if (resultat != 0)
            {
                throw new ApplicationException("Erreur lors du calcul de la volatilité pour Basket: WREmodelingCov, erreur numero : " + resultat);
            }
            double[] exante = new double[nbAssets];
            //Calcul de la volatilité
            resultat = WREanlysisExanteVolatility(ref nbAssets, cov, oWeights, exante, ref info);
            if (resultat != 0)
            {
                throw new ApplicationException("Erreur lors du calcul de la volatilité pour Basket: WREanlysisExanteVolatility, erreur numero : " + resultat);
            }


        }


/*            double[,] prix = new double[listDataFeed.Count, oShares.Length];
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
                    avg[col] += Math.Log10(prix[line+1, col] / prix[line, col]);
                }
            }


            for (int col = 0; col < oShares.Length; col++)
            {
                variance[col] = variance[col] / listDataFeed.Count - Math.Pow(avg[col] / listDataFeed.Count, 2);
                oVolatility[col] = Math.Sqrt(variance[col]);
            }


            


            // Partie weights
            oWeights = new double[oShares.Length];
            for (int w = 0; w < oShares.Length; w++)
            {
                oWeights[w] = 1 / oShares.Length;
            }
*/

    

        #region Getter & Setter
        public string OName
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

        public Share[] OShares
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

        public DateTime OMaturity
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

        public double OStrike
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



        public DateTime CurrentDate
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

        public double[] OSpot
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


        public double[] OVolatility
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


        public double[] OWeights
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

        public double[] oWeights { get; set; }

        public double[] oVolatility { get; set; }

        public double[] oSpot { get; set; }

        public DateTime currentDate { get; set; }

        public double oStrike { get; set; }

        public DateTime oMaturity { get; set; }

        public Share[] oShares { get; set; }

        public string oName { get; set; }
    }
}
