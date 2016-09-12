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
    public class TestBasketPricing
    {
        [TestMethod]
        public void TestMethod1()
        {
            BasketPricingModel bpm = new BasketPricingModel();
            bpm.oMaturity = new DateTime(2015, 8, 20);
            bpm.oName = "test";
            Share share = new Share("ACCOR SA", "AC FP     ");
            bpm.oShares = new Share[1];
            bpm.oShares[0] = share;
            bpm.oStrike = 45.5;
            DataGestion dg = new DataGestion();
            DateTime date = new DateTime(2013, 8, 21);
            List<DataFeed> ldf = dg.getListDataField(date, bpm.oMaturity,bpm.oShares); 
            PricingResults pR = bpm.getPayOff(ldf);
            Console.WriteLine(pR.Price);
        }
    }
}
