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


        public Facade(IGenerateHistory generateHistory, IPricing pricing)
        {
            this.generateHistory = generateHistory;
            this.pricing = pricing;
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
    }
}
