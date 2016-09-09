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
    public interface IPricing
    {

        PricingResults getPayOff(List<DataFeed> listDataFeed);

        List<PricingResults> pricingUntilMaturity(List<DataFeed> listDataFeed);

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
    }
}
