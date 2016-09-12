using System;
using ProjetNET.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PricingLibrary.FinancialProducts;
using ProjetNET.Models;
using System.Collections.Generic;
using PricingLibrary.Utilities.MarketDataFeed;
namespace UnitTestProject1
{
    [TestClass]
    public class TestForwardData
    {
        [TestMethod]
        public void TestWRE()
        {
            ForwardData fd = new ForwardData();
            int res = fd.testCov();
            if (res != 0)
            {
                throw new ArgumentOutOfRangeException("le module WRE ne s'est pas exécuté correctement : " + res);
            }
        }


                  [TestMethod]
        public void TestGenHistOptionForward()
        {
            String VanillaCallName = "FistOptionName";
            DataGestion dataConn = new DataGestion();
            Share[] underlyingShares = new Share[dataConn.numberOfAssets()];

            var listeID = dataConn.getListofID();
            for (int i = 0; i < dataConn.numberOfAssets(); i++)
            {
                underlyingShares[i] = new Share( dataConn.getName(listeID.ToArray()[i]),listeID.ToArray()[i]);
            }
            DateTime endTime = new DateTime(2016,1,1);
            DateTime startDate = new DateTime(2015, 10, 10);
            double strike = 100.0;
            double[] weight = new double[dataConn.numberOfAssets()];
            for (int i = 0; i < dataConn.numberOfAssets(); i++)
            {
                weight[i] = 1 / dataConn.numberOfAssets();
            }


            IGenerateHistory test = new ForwardTestGenerate();
            test.endTime = endTime; //
            test.strike = strike; //
            test.underlyingShares = underlyingShares;//
            test.vanillaCallName = VanillaCallName;//
            test.weight = weight;//
            test.startDate = startDate; //
            List<DataFeed> ret = test.generateHistory();
            Console.WriteLine("----");
            foreach (DataFeed dataf in ret)
            {
                List<string> keyList = new List<string> (ret.ToArray()[0].PriceList.Keys);
                
                foreach (var ele in keyList)
                {
                    Console.WriteLine(dataf.Date);
                    Console.WriteLine(dataf.PriceList[ele]);                
                }
            }
        }
    }
}
