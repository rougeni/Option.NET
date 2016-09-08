using PricingLibrary.Computations;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
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
            bool debut = true;
            PricingResults ancienPR = null ;
            double tauxSR = PricingLibrary.Utilities.MarketDataFeed.RiskFreeRateProvider.GetRiskFreeRate();
            foreach (PricingResults pr in listePricingResult)
            {
                double valeur;
                if (debut)
                {
                    valeur = pricing.oStrike;
                    debut = false;
                }
                else
                {
                    valeur = ancienPR.Deltas[0] * pr.Price + (pricing.oStrike - ancienPR.Deltas[0] * ancienPR.Price) * Math.Exp(tauxSR);
                }
                Portefeuille port = new Portefeuille(pricing.currentDate, valeur);
                listePortefeuille.Add(port);
                ancienPR = pr;
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
