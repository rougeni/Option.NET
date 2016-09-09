using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetNET.Models;
using ProjetNET.Data;
using PricingLibrary.Computations;
using PricingLibrary.Utilities.MarketDataFeed;
using PricingLibrary.FinancialProducts;
using System.Collections.Generic;
using System.Diagnostics;

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
            List<DataFeed> ldf = dg.getListDataField(date, vcpm.oMaturity);
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
            List<DataFeed> ldf = dg.getListDataField(date, vcpm.oMaturity);
            List<PricingResults> LpR = vcpm.pricingUntilMaturity(ldf);
            foreach (PricingResults pr in LpR)
            {
                Console.WriteLine(pr.Price);
            }
        }

        [TestMethod]
        public void testVolatility()
        {
            DataGestion dg = new DataGestion();
            List<DataFeed> ldf = dg.getListDataField(new DateTime(2013, 10, 10), new DateTime(2014, 10, 10));
            VanillaCallPricingModel vcpm = new VanillaCallPricingModel();
            Share share = new Share("ACCOR SA", "AC FP     ");
            vcpm.oShares = new Share[1];
            vcpm.oShares[0] = share;
            vcpm.calculVolatility(ldf);
        }

        [TestMethod]
        public void testVolatilityBasket()
        {
            ForwardTestGenerate f = new ForwardTestGenerate();
            f.vanillaCallName = "Basket NAME";
            Share[] under = new Share[4];

            under[0] = new Share("1A", "ID1");
            under[1] = new Share("2A", "ID2");
            under[2] = new Share("3A", "ID3");
            under[3] = new Share("4A", "ID4");
            f.underlyingShares = under;
            f.weight = new double[4] { 0.25, 0.25, 0.25, 0.25 };
            f.endTime = new DateTime(2014, 09, 09);
            f.startDate = new DateTime(2013, 12, 12);
            f.strike = 20.0;
            List<DataFeed> datum = f.generateHistory();

            BasketPricingModel vcpm = new BasketPricingModel();
            vcpm.oWeights = f.weight;
            vcpm.oSpot = new double[4]{10,10,10,10};
            vcpm.currentDate = f.startDate;
            vcpm.oStrike = 20.0;
            vcpm.oMaturity = f.endTime;
            vcpm.oShares = f.underlyingShares;
            vcpm.oName = f.vanillaCallName;

            vcpm.calculVolatility(datum);
            //Console.WriteLine(vcpm.oVolatility);
            /*for (int i = 0; i < 4; i++)
            {*/
                //System.Diagnostics.Debug.Assert(vcpm.oVolatility[i] > 0.3 && vcpm.oVolatility[i] < 0.5);
                Console.WriteLine(vcpm.oVolatility[0]);
            //}

        }
    }
}
