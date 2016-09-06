using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetNET.Data;
using System.Diagnostics;

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
            if( tabCote[0] != 29.3173 ||
                tabCote[1] != 29.7017 ||
                tabCote[2] != 30.4049 ||
                tabCote[3] != 30.1821)
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
            dataConn.getListofID();
        }
        [TestMethod]
        public void listingNAMES()
        {
            DataConnection dataConn = new DataConnection();
            dataConn.getListofNames();
        }
        [TestMethod]
        public void pick_a_name()
        {
            DataConnection dataConn = new DataConnection();
            string accor = dataConn.getName("AC FP");
            Debug.Assert(accor == "ACCOR SA");
        }
        [TestMethod]
        public void getTable()
        {
            DataConnection dataConn = new DataConnection();
            var tableB = dataConn.getIDNames();
            //TODO: vérifier le contenu de la table
        }
    }
}
