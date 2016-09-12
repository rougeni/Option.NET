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

        #region Extern Declaration
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

        #endregion Extern Declaration

        #region Private Properties

        private Pricer basketPricer;
        private int businessDays = DayCount.CountBusinessDays(new DateTime(2014, 1, 1), new DateTime(2014, 12, 31));
        private double[,] matriceCorr;
        private double tauxSR;

        #endregion Private Properties

        #region Constructor
        public BasketPricingModel()
        {
            basketPricer = new Pricer();
            oName = "Basket";
            tauxSR = PricingLibrary.Utilities.MarketDataFeed.RiskFreeRateProvider.GetRiskFreeRate();
        }
        #endregion Constructor

        #region Interface Implementation

        public List<PricingResults> pricingUntilMaturity(List<DataFeed> listDataFeed)
        {
            if (oName.Equals(null) || oShares.Equals(null) || oMaturity == null || oStrike.Equals(null) || listDataFeed.Count == 0 )
            {
                throw new NullReferenceException();  // TODO pls check if correct
            }
            List<PricingResults> listPrix = new List<PricingResults>();
            List<DataFeed> listdf = new List<DataFeed>(listDataFeed);

            //This line wad added.
            oSpot = new double[oShares.Length];
            BasketOption bask_o = new BasketOption(oName, oShares, oWeights, oMaturity, oStrike);
            while (listdf.Count > 30)
            {
                calculVolatility(listdf);
                for (int myShare = 0; myShare < oShares.Length; myShare++){
                    oSpot[myShare] = (double)listdf[30].PriceList[oShares[myShare].Id];
                }
                PricingResults pr = basketPricer.PriceBasket(bask_o, listdf[30].Date, businessDays, oSpot, oVolatility, matriceCorr);
                if (listPrix.Count > 0)
                {
                    var prev = listPrix.Last();
                    if (Math.Abs(prev.Price - pr.Price) > 3.5)
                    {
                        var i = 0;
                    }
                }
                listPrix.Add(pr);
                listdf.RemoveAt(0);
            }

            return listPrix;
        }

        public List<Portefeuille> getPortefeuillesCouverture(List<DataFeed> listDataFeed, List<PricingResults> ListePricingResult)
        {
            List<Portefeuille> listePortefeuille = new List<Portefeuille>();
            IEnumerator<PricingResults> enumPR = ListePricingResult.GetEnumerator();
            int ind = 0;
            while (ind < 30)
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
            double[] currentDelta = new double[oShares.Length];
            while (enumPR.MoveNext() && enumLDF.MoveNext())
            {
                PricingResults pr = (PricingResults)enumPR.Current;
                DataFeed df = (DataFeed)enumLDF.Current;
                if (estDebut)
                {
                    //calcul de PI0
                    valeur = (double)pr.Price;
                    estDebut = false;
                    currentDelta = pr.Deltas;

                }
                else
                {
                    if (waitForRebalancing == 0)
                    {
                        valeur = produitScalaire(currentDelta, df.PriceList) + (ancienneValeur - produitScalaire(currentDelta, ancienDF.PriceList)) * Math.Exp(tauxSR / businessDays);
                        waitForRebalancing = oRebalancement ;
                        currentDelta = ancienPR.Deltas;
                    }
                    else
                    {
                        valeur = produitScalaire(currentDelta, df.PriceList) + (ancienneValeur - produitScalaire(currentDelta, ancienDF.PriceList)) * Math.Exp(tauxSR / businessDays);
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

            return basketPricer.PriceBasket(new BasketOption(oName, oShares, oWeights, oMaturity, oStrike), oMaturity, 252, oSpot, oVolatility, matriceCorr);
        }

        #endregion Interface Implementation

        #region Internal Method

        /**
         * fonction qui réalise le produit scalaire entre 
         *      - les valeurs contenues dans p
         *      - les prix contenus dans le dictionnaire
         * */
        private double produitScalaire(double[] p, Dictionary<string, decimal> dictionary)
        {
            double val = 0;
            for (int i = 0; i < p.Length; i++)
            {
                val += p[i] * (double)dictionary[oShares[i].Id];
            }
            return val;
        }

        /**
         * calcul du vecteur volatilité des 30 premières valeurs de la liste passée en paramètres
         * */
        public void calculVolatility(List<DataFeed> listDataFeed)
        {
            //Calcul des log rendements
            int nbAssets = listDataFeed.ToArray()[0].PriceList.Count;
            int nbValues = listDataFeed.Count();
            double[,] assetsValues = new double[nbValues,nbAssets];
            int horizon = listDataFeed.Count - 30 ;
            double[,] assetsReturns = new double[(nbValues - horizon), nbAssets];
            int info = 10;
            for (int i = 0; i < nbValues; i++ )
            {
                int b = 0;
                foreach (var iden in oShares)
                {
                    assetsValues[i,b] = (double)listDataFeed.ToArray()[i].PriceList[iden.Id];
                    b++;
                }
            }
            int resultat = WREmodelingLogReturns(ref nbValues,ref nbAssets, assetsValues, ref horizon, assetsReturns,ref info);
            if (resultat != 0)
            {
                throw new ApplicationException("Erreur lors du calcul de la volatilité pour Basket: WREmodelingLogReturns, erreur numero : " + resultat);
            }

            //calcul de la covariance
            double[,] cov = new double[nbAssets,nbAssets];
            int nbValuesCov = 30;
            resultat = WREmodelingCov(ref nbValuesCov,ref nbAssets, assetsReturns, cov, ref info);
            if (resultat != 0)
            {
                throw new ApplicationException("Erreur lors du calcul de la volatilité pour Basket: WREmodelingCov, erreur numero : " + resultat);
            }
            matriceCorr = new double[nbAssets, nbAssets];

            double[,] corr = new double[nbAssets, nbAssets];
            int nbValuesCorr = 30;
            resultat = WREmodelingCorr(ref nbValuesCorr, ref nbAssets, assetsReturns, corr, ref info);

            if (resultat != 0)
            {
                throw new ApplicationException("Erreur lors du calcul de la volatilité pour Basket: WREmodelingCov, erreur numero : " + resultat);
            }
            this.matriceCorr = corr;

            //Calcul de la volatilité
            int distTime = (nbValues - horizon);

            double[] rend = new double[distTime];

            oVolatility = new double[nbAssets];

            for (int j = 0; j < nbAssets; j++)
            {
                for (int k = 0; k < distTime; k++)
                {
                    rend[k] = assetsReturns[k,j];
                }
                    resultat = WREanalysisExpostVolatility(ref distTime, rend, ref oVolatility[j], ref info);
                if (resultat != 0)
                {
                    throw new ApplicationException("Erreur lors du calcul de la volatilité pour Basket: WREanalysisExanteVolatility, erreur numero : " + resultat);
                }
            }
        }

        #endregion Internal Method

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

        #region Public Properties

        public double[] oWeights { get; set; }

        public double[] oVolatility { get; set; }

        public double[] oSpot { get; set; }

        public DateTime currentDate { get; set; }

        public double oStrike { get; set; }

        public DateTime oMaturity { get; set; }

        public Share[] oShares { get; set; }

        public string oName { get; set; }


        public int oRebalancement { get; set; }

        #endregion Public Properties

    }
}
