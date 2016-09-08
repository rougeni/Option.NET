using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetNET.Data;
using System.Diagnostics;
using System.Collections.Generic;
using PricingLibrary;
using PricingLibrary.Utilities.MarketDataFeed;

namespace TestUnitaires
{
    [TestClass]
    public class TestDataBase
    {

        [TestMethod]
        public void testTailleTableauCotation()
        {
            DataGestion dataConn = new DataGestion();
            double[] tabCote = dataConn.getCotation("CA FP", 10);
            if (tabCote.Length != 10)
            {
                throw new ArgumentOutOfRangeException("le tableau de cotation n'a pas la bonne taille");
            }
            Debug.Assert(tabCote.Length == 10);
        }

        [TestMethod]
        public void testValeurTableauCotation()
        {
            DataGestion dataConn = new DataGestion();
            double[] tabCote = dataConn.getCotation("CA FP", 10);
            if (tabCote[0] != 29.4250 ||
                tabCote[1] != 29.5050 ||
                tabCote[2] != 30.2000 ||
                tabCote[3] != 30.2800)
            {
                throw new ArgumentOutOfRangeException("une des valeurs n'est pas bonne dans le tableau de  cotation");
            }
        }

        [TestMethod]
        public void testFakeGetID()
        {
            DataGestion dataConn = new DataGestion();
            String id = dataConn.getID("test");
            if (id != null)
            {
                throw new ArgumentOutOfRangeException("id non null dans tastFakeGetId");
            }
        }

        [TestMethod]
        public void testGetID()
        {
            DataGestion dataConn = new DataGestion();
            String id = dataConn.getID("ALSTOM");
            if (!id.Contains("ALO FP"))
            {
                throw new ArgumentOutOfRangeException("l'ID récupéré n'est pas bon :" + id);
            }
        }

        [TestMethod]
        public void listingIDs()
        {
            DataGestion dataConn = new DataGestion();
            List<String> liste = dataConn.getListofID();
            if (liste.Count != 14)
            {
                /*foreach (var o in liste)
                {
                    Console.WriteLine(o);
                }*/
                throw new ArgumentOutOfRangeException("La liste d'ID contiens " + liste.Count + " éléments au lieu de 14");
            }
        }

        [TestMethod]
        public void listingNAMES()
        {
            DataGestion dataConn = new DataGestion();
            List<String> liste = dataConn.getListofNames();
            if (liste.Count != 14)
            {
                throw new ArgumentOutOfRangeException("La liste d'ID contiens " + liste.Count + " éléments au lieu de 14");
            }
        }

        [TestMethod]
        public void pick_a_name()
        {
            DataGestion dataConn = new DataGestion();
            string accor = dataConn.getName("AC FP");
            if (!accor.Contains("ACCOR SA"))
            {
                throw new ArgumentOutOfRangeException("Le nom récupéré ne correspond pas à l'ID saisi: " + accor);
            }
        }

        [TestMethod]
        public void getTable()
        {
            DataGestion dataConn = new DataGestion();
            string[,] tableB = dataConn.getIDNames();
            if (tableB.Length != 28)
            {
                throw new ArgumentOutOfRangeException("Le tableau n'a pas la bonne  dimension : " + tableB.Length);
            }
            if (!tableB[3, 0].Contains("AIR FP") || !tableB[3, 1].Contains("AIRBUS GROUP SE"))
            {
                throw new ArgumentOutOfRangeException("les valeurs du tableau ne sont pas correctes");
            }
        }

        [TestMethod]
        public void testDataFeed()
        {
            DataGestion dataConn = new DataGestion();
            DateTime dateStart = new DateTime(2015, 08, 17);
            List<DataFeed> liste = dataConn.getListDataField(dateStart);

            if (liste.ToArray().Length != 4)
            {
                throw new ArgumentOutOfRangeException("La liste ne contiens pas 4 éléments : " + liste.Count);
            }
        }

        [TestMethod]
        public void testLastday()
        {
            DataGestion dataConn = new DataGestion();
            DateTime endDate= new DateTime(2015, 08, 20);
            DateTime testDate = dataConn.lastDay();
            Debug.Assert(endDate == testDate);
        }
        

            [TestMethod]
        public void testNumberAssets()
        {
            DataGestion dataConn = new DataGestion();
            Debug.Assert(dataConn.numberOfAssets() == 14);   
        }


            [TestMethod]
            public void testLastValues()
            {
                DataGestion dataConn = new DataGestion();
                double[] values = dataConn.lastValues();
                Debug.Assert(values.Length == 14);
                Debug.Assert(values[0] == 43.405);
                Debug.Assert(values[1] == 12.25);
                Debug.Assert(values[11] == 45.475);

            }
    }
}
