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
                //underlyingShares[0] = new Share(liste.ToArray()[0], dataConn.getName(liste.ToArray()[0]));
    /*        Console.WriteLine(liste.ToArray()[0]);
            Console.WriteLine(liste.ToArray()[1]);
            Console.WriteLine(liste.ToArray()[2]);
            underlyingShares[1] = new Share(liste.ToArray()[1], dataConn.getName(liste.ToArray()[1]));
            underlyingShares[2] = new Share(liste.ToArray()[2], dataConn.getName(liste.ToArray()[2]));
            Console.WriteLine("----");
            Console.WriteLine(underlyingShares[0].Id);
            Console.WriteLine(underlyingShares[0].Name);
            Console.WriteLine(underlyingShares[1].Id);
            Console.WriteLine(underlyingShares[1].Name);
                          Console.WriteLine(underlyingShares[2].Id);
                          Console.WriteLine(underlyingShares[2].Name);
                          */

            DateTime endTime = new DateTime(2016,1,1);
            double strike = 100.0;

            IGenerateHistory test = new ForwardTestGenerate();
            List<DataFeed> ret = test.generateHistory(VanillaCallName, underlyingShares, endTime, strike);
            Console.WriteLine("----");
            //foreach (DataFeed dataf in ret)
            //{
                List<string> keyList = new List<string> (ret.ToArray()[0].PriceList.Keys);
                //decimal decim = dataf.PriceList[liste.ToArray()[0]];
                Console.WriteLine(ret.ToArray()[0].PriceList.Count);
                foreach (var ele in keyList)
                {
                    Console.WriteLine(ele);
                }
            //}
        }
    }
}
