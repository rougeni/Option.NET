using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetNET.Data;
using System.Diagnostics;
using System.Collections.Generic;

namespace TestUnitaires
{
    [TestClass]
    public class TestDataBase
    {

        [TestMethod]
        public void testTailleTableauCotation()
        {
            DataConnection dataConn = new DataConnection();
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
            DataConnection dataConn = new DataConnection();
            double[] tabCote = dataConn.getCotation("CA FP", 10);
            if( tabCote[0] != 29.4250 ||
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
            DataConnection dataConn = new DataConnection();
            String id = dataConn.getID("test");
            if (id != null)
            {
                throw new ArgumentOutOfRangeException("id non null dans tastFakeGetId");
            }
        }

        [TestMethod]
        public void testGetID()
        {
            DataConnection dataConn = new DataConnection();
            String id = dataConn.getID("ALSTOM");
            if (!id.Contains("ALO FP"))
            {
                throw new ArgumentOutOfRangeException("l'ID récupéré n'est pas bon :" + id);
            }
        }

        [TestMethod]
        public void listingIDs()
        {
            DataConnection dataConn = new DataConnection();
            List<String> liste = dataConn.getListofID();
            if (liste.Count != 14)
            {
                throw new ArgumentOutOfRangeException("La liste d'ID contiens " + liste.Count + " éléments au lieu de 14");
            }
        }

        [TestMethod]
        public void listingNAMES()
        {
            DataConnection dataConn = new DataConnection();
            List<String> liste = dataConn.getListofNames();
            if (liste.Count != 14)
            {
                throw new ArgumentOutOfRangeException("La liste d'ID contiens " + liste.Count + " éléments au lieu de 14");
            }
        }

        [TestMethod]
        public void pick_a_name()
        {
            DataConnection dataConn = new DataConnection();
            string accor = dataConn.getName("AC FP");
            if (!accor.Contains("ACCOR SA"))
            {
                throw new ArgumentOutOfRangeException("Le nom récupéré ne correspond pas à l'ID saisi: " + accor);
            }
        }

        [TestMethod]
        public void getTable()
        {
            DataConnection dataConn = new DataConnection();
            string[,] tableB = dataConn.getIDNames();
            if (tableB.Length != 28)
            {
                throw new ArgumentOutOfRangeException("Le tableau n'a pas la bonne  dimension : " + tableB.Length);
            }
            if (!tableB[3, 0].Contains("AIR FP") || !tableB[3,1].Contains("AIRBUS GROUP SE"))
            {
                throw new ArgumentOutOfRangeException("les valeurs du tableau ne sont pas correctes");
            }
        }
    }
}
