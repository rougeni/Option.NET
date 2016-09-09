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
    public class Facade
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
            listePortefeuille = pricing.getPortefeuillesCouverture(ldf, listePricingResult);
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
