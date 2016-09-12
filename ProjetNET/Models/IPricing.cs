using PricingLibrary.Computations;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.Models
{
    /* 
     * Interface to provide a structure for the different pricing models - Vanilla and Basket options.
     * */
    public interface IPricing
    {
        /**
         * Method to get the Payoff at maturity of an option.
         * 
         * */
        PricingResults getPayOff(List<DataFeed> listDataFeed);

        /**
         * Method to calculate all cotation in between a start date and maturity. Returns a list of PricingResults.
         * 
         * */
        List<PricingResults> pricingUntilMaturity(List<DataFeed> listDataFeed);

        /**
         * Method to calculate the portfolio value for each date and takes in count the balancing period.
         *  
         * */
        List<Portefeuille> getPortefeuillesCouverture(List<DataFeed> listDataFeed, List<PricingResults> ListePricingResult);

        // nom de l option
        String oName { get; set; }
        // tab des sous-jacent
        Share[] oShares { get; set; }
        DateTime oMaturity { get; set; }
        double oStrike { get; set; }
        // prix a la date de depart
        DateTime currentDate { get; set; }
        double[] oWeights { get; set; }

        int oRebalancement { get; set; }
    }
}
