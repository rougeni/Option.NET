using PricingLibrary.Computations;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary.Utilities;

namespace ProjetNET.Models
{
    internal class BasketPricingModel : IPricing
    {
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
        private double[,] matriceCorr;
        private int businessDays = DayCount.CountBusinessDays(new DateTime(2014, 1, 1), new DateTime(2014, 12, 31));
        private double tauxSR;

        
       

        public BasketPricingModel()
        {
            basketPricer = new Pricer();
            oName = "Basket";
            tauxSR = PricingLibrary.Utilities.MarketDataFeed.RiskFreeRateProvider.GetRiskFreeRate();
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

                listPrix.Add(basketPricer.PriceBasket(new BasketOption(oName, oShares, oWeights, oMaturity, oStrike), df.Date, businessDays, oSpot, oVolatility, matriceCorr));
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
        private double produitScalaire(double[] p,Dictionary<string,decimal> dictionary)
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

            return basketPricer.PriceBasket(new BasketOption(oName, oShares, oWeights, oMaturity, oStrike), oMaturity, businessDays, oSpot, oVolatility, matriceCorr);
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

            for (int line = 0; line < listDataFeed.Count -1; line++)
            {
                for (int col = 0; col < oShares.Length; col++)
                {
                    variance[col] += Math.Pow(Math.Log10(prix[line+1, col] / prix[line, col]), 2);
                    avg[col] += Math.Log10(prix[line+1, col] / prix[line, col]);
                }
            }


            for (int col = 0; col < oShares.Length; col++)
            {
                variance[col] = variance[col] / listDataFeed.Count - Math.Pow(avg[col] / listDataFeed.Count, 2);
                oVolatility[col] = Math.Sqrt(variance[col]);
            }


            // Partie de calcul des rendements pour la matrice de correlation
            double[,] rendements = new double[listDataFeed.Count - 1, oShares.Length];
            for (int date = 0; date < listDataFeed.Count - 1; date++)
            {
                for (int share = 0; share < oShares.Length; share++)
                {
                    rendements[date, share] = Math.Log10(prix[date + 1, share] / prix[date, share]);
                }
            }

            int  nbValues = listDataFeed.Count-1;
            int nbAssets = oShares.Length;
            double[,] corr = new double[nbAssets, nbAssets];
            int info = 0;
            WREmodelingCorr(ref nbValues, ref nbAssets, rendements, corr, ref info);
            matriceCorr = corr;

        }


        public double[] oWeights { get; set; }

        public double[] oVolatility { get; set; }

        public double[] oSpot { get; set; }

        public DateTime currentDate { get; set; }

        public double oStrike { get; set; }

        public DateTime oMaturity { get; set; }

        private Share[] shares; 

        public Share[] oShares {
            get { return shares; }
            set
            {
                shares = value;
                oSpot = new double[shares.Length];
                oVolatility = new double[shares.Length];
                oWeights = new double[shares.Length];
                for (int i = 0; i < shares.Length; i++)
                {
                    oWeights[i] = ((double)1 / (double)shares.Length);
                }
            }
        }

        public string oName { get; set; }
    }
}
