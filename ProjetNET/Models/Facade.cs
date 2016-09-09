using PricingLibrary.Computations;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.Models
{
    internal class Facade
    {
        private IPricing pricing;

        private IGenerateHistory generateHistory;

        private List<PricingResults> listePricingResult;

        private List<Portefeuille> listePortefeuille;


        public Facade(IGenerateHistory generateHistory, IPricing pricing)
        {
            this.generateHistory = generateHistory;
            this.pricing = pricing;
        }

        ///  3 éléments
        ///     - liste datafield generateHitory
        ///     - result pricing par pricing(datafield)
        ///     - prix du portefeuille de couverture à toutes les dates (Pi(i))
        public void update()
        {
            List<DataFeed> ldf = generateHistory.generateHistory();
            listePricingResult = pricing.pricingUntilMaturity(ldf);
            listePortefeuille = new List<Portefeuille>();
            double tauxSR = PricingLibrary.Utilities.MarketDataFeed.RiskFreeRateProvider.GetRiskFreeRate();
            
            /*foreach (PricingResults pr in listePricingResult)
            {
                double valeur;
                if (debut)
                {
                    valeur = pricing.oStrike;
                    debut = false;
                }
                else
                {
                    //valeur = ancienPR.Deltas[0] * pr.Price + (pricing.oStrike - ancienPR.Deltas[0] * ancienPR.Price) * Math.Exp(tauxSR);
                }
                Portefeuille port = new Portefeuille(pricing.currentDate, valeur);
                listePortefeuille.Add(port);
                ancienPR = pr;
            }*/

            IEnumerator enumPR = ListePricingResult.GetEnumerator();
            IEnumerator enumLDF = ldf.GetEnumerator();
            bool estDebut = true;
            double valeur = 0;
            double ancienneValeur = 0;
            PricingResults ancienPR = null;
            DataFeed ancienDF = null;
            string sousJacent = Pricing.oShares[0].Id;
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
                Portefeuille port = new Portefeuille(pricing.currentDate, valeur);
                listePortefeuille.Add(port);
            }



            
        }

        public IPricing Pricing
        {
            get { return pricing; }
            set { pricing = value; }
        }

        public IGenerateHistory GenrateHistory
        {
            get { return generateHistory; }
            set { generateHistory = value; }
        }

        public void CalculValuePortfolio()
        {
            
        }

        public List<PricingResults> ListePricingResult
        {
            get { return listePricingResult; }
        }

        public List<Portefeuille> ListePortefeuille
        {
            get { return listePortefeuille; }
        }
    }
}
