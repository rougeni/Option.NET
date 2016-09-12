using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using PricingLibrary.Utilities.MarketDataFeed;
using PricingLibrary.FinancialProducts;

namespace ProjetNET.Data
{
    public class ForwardData
    {
        // import WRE dlls
        [DllImport("wre-ensimag-c-4.1.dll", EntryPoint = "WREmodelingCov", CallingConvention = CallingConvention.Cdecl)]

        // declaration de WREmodelingCov
        public static extern int WREmodelingCov(
            ref int returnsSize,
            ref int nbSec,
            double[,] secReturns,
            double[,] covMatrix,
            ref int info
        );
        
        /*
         * Renvoie la liste des prix des données simulées
         * @VanilaCallName: nom de l'option
         * @underlyingShares: tableau des sous-jacents
         * @weight: poids des sous-jacents dans le portefeuille
         * @startDate: date de début de la simulation
         * @endTime: date de fin de la simulation
         * @strike: valeur du strike de l'option
         * */
        public List<DataFeed> getForwardListDataField(String VanillaCallName, Share[] underlyingShares, double[] weight, DateTime startDate,  DateTime endTime, double strike)
        {
            SimulatedDataFeedProvider simulvalues = new SimulatedDataFeedProvider();
            DataGestion dg = new DataGestion();
            int p = dg.numberOfAssets();
            IOption optionData = new BasketOption(VanillaCallName, underlyingShares,weight, endTime, strike);
            List<DataFeed> retMarket = simulvalues.GetDataFeed(optionData,startDate);//TODO : check this line
            return retMarket;
        }
    }
}
