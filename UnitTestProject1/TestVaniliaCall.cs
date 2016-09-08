using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetNET.Models;
using ProjetNET.Data;
using PricingLibrary.Computations;
using PricingLibrary.Utilities.MarketDataFeed;
using PricingLibrary.FinancialProducts;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class TestVaniliaCall
    {
        [TestMethod]
        public void TestMethod1()
        {
            VanillaCallPricingModel vcpm = new VanillaCallPricingModel();
            vcpm.oMaturity = new DateTime(2015, 8, 20);
            vcpm.oName = "test";
            Share share = new Share("ACCOR SA", "AC FP     ");
            vcpm.oShares = new Share[1];
            vcpm.oShares[0] = share;
            vcpm.oStrike = 45.5;
            vcpm.oSpot = new double[1];
            vcpm.oSpot[0] = 48;
            DataGestion dg =new DataGestion();
            DateTime date = new DateTime(2013,8,20);
            List<DataFeed> ldf = dg.getListDataField(date);
            PricingResults pR = vcpm.getPayOff(ldf);
            Console.WriteLine(pR.Price);
        }

        [TestMethod]
        public void TestMethod2()
        {
            DataGestion dg = new DataGestion();
            VanillaCallPricingModel vcpm = new VanillaCallPricingModel();
            vcpm.oMaturity = new DateTime(2015, 8, 20);
            vcpm.oName = "test";
            Share share = new Share("ACCOR SA", "AC FP     ");
            vcpm.oShares = new Share[1];
            vcpm.oShares[0] = share;
            vcpm.oStrike = 10;
            vcpm.oSpot = new double[1];
            DateTime date = new DateTime(2015, 1, 12);
            vcpm.oSpot[0] = dg.getCotation("AC FP", date);
            List<DataFeed> ldf = dg.getListDataField(date);
            List<PricingResults> LpR = vcpm.pricingUntilMaturity(ldf);
            foreach (PricingResults pr in LpR)
            {
                Console.WriteLine(pr.Price);
            }
        }
    }
}
