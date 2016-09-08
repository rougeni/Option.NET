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

        // declaration
        public static extern int WREmodelingCov(
            ref int returnsSize,
            ref int nbSec,
            double[,] secReturns,
            double[,] covMatrix,
            ref int info
        );
        /*public static extern int WREmodelingEM(
            ref int nbValues,
            ref int NbAssets,
            int[,] assetsValues,
            ref double missValue,
            int[,] assetsValuesCorrected,
            ref int info
            );

        public int testWREsimu()
        {
            int nbValues = 2;
            int NbAssets = 2;
            int[,] assetsValues = { { 1, 1 }, { 1, 1 } };
            double missValue = 0;
            int[,] assetsValuesCorrected = new int[2, 2];
            int info = 10;
            return WREmodelingEM(ref nbValues, ref NbAssets, assetsValues, ref missValue, assetsValuesCorrected, ref info);
        }*/

        /*
        public static extern int WREsimulGeometricBrownianX(
            ref int p,
            ref int T,
            ref int N,
            double[] S,
            double[] mu,
            double[,] cov,
            double[,] y,
            int info
            );
        */

        
        public List<DataFeed> getForwardListDataField(String VanillaCallName, Share[] underlyingShares, double[] weight, DateTime startDate,  DateTime endTime, double strike)
        {
            SimulatedDataFeedProvider simulvalues = new SimulatedDataFeedProvider();
            DataGestion dg = new DataGestion();
            int p = dg.numberOfAssets();
            //DateTime lastTime = dg.lastDay();
            /*weight = new double[1];
            weight[0] = 1;*/
            IOption optionData = new BasketOption(VanillaCallName, underlyingShares,weight, endTime, strike);
            List<DataFeed> retMarket = simulvalues.GetDataFeed(optionData,startDate);//TODO : check this line
            return retMarket;
        }
        
        


        public int testCov()
        {
            int nbValues = 2;
            int nbAssets = 2;
            double[,] assetsReturns = { { 1, 3 }, { 2, 5 } };
            double[,] cov = new double[2, 2];
            int info = 10;
            return WREmodelingCov(ref nbValues, ref nbAssets, assetsReturns, cov, ref info);
        }
        
        public double[,] computeCovarianceMatrix(double[,] returns)
        {
            int dataSize = returns.GetLength(0);
            int nbAssets = returns.GetLength(1);
            double[,] covMatrix = new double[nbAssets, nbAssets];
            int info = 0;
            int res;
            res = WREmodelingCov(ref dataSize, ref nbAssets, returns, covMatrix, ref info);
            if (res != 0)
            {
                if (res < 0)
                    throw new Exception("ERROR: WREmodelingCov encountred a problem. See info parameter for more details");
                else
                    throw new Exception("WARNING: WREmodelingCov encountred a problem. See info parameter for more details");
            }
            return covMatrix;
        }

        public void dispMatrix(double[,] myCovMatrix)
        {
            int n = myCovMatrix.GetLength(0);

            Console.WriteLine("Covariance matrix:");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(myCovMatrix[i, j] + "\t");
                }
                Console.Write("\n");
            }
        }

        public void TestWRE()
        {
            // header
            Console.WriteLine("******************************");
            Console.WriteLine("*    WREmodelingCov in C#   *");
            Console.WriteLine("******************************");

            // sample data
            double[,] returns = { {0.05, -0.1, 0.6}, {-0.001, -0.4, 0.56}, {0.7, 0.001, 0.12}, {-0.3, 0.2, -0.1},
                                {0.1, 0.2, 0.3}};

            // call WRE via computeCovarianceMatrix encapsulation
            double[,] myCovMatrix = computeCovarianceMatrix(returns);

            // display result
            dispMatrix(myCovMatrix);

            // ending the program            
            Console.WriteLine("\nType enter to exit");
            Console.ReadKey(true);
        }


    }
}
