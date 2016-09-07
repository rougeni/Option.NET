using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary.Computations;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;

namespace ProjetNET.Models
{
    class VanillaCallPricingModel : IPricing
    {

        private Pricer vanillaPricer;

        public VanillaCallPricingModel()
        {
            vanillaPricer = new Pricer();
        }


        public List<PricingResults> pricingUntilMaturity(List<DataFeed> dataFeed, DateTime maturity)
        {
            throw new NotImplementedException();
        }

        public PricingResults getPayOff(List<DataFeed> dataFeed)
        {
            return vanillaPricer.PriceCall(new VanillaCall(oName, oShares, oMaturity, oStrike), oMaturity, 252, oSpot, oVolatility);
        }

        #region Getter & Setter
        public string oName
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

        public Share[] oShares
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

        public DateTime oMaturity
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

        public double oStrike
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
        


        public DateTime currentDate
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

        public double oSpot
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


        public double oVolatility
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
        #endregion Getter & Setter
    }
}
