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
            int[] tabCote = dataConn.getCotation("CA FP", 10);
            Debug.Assert(tabCote.Length == 10);
            String id = dataConn.getID("test");
            Debug.Assert(id == null);
        }
    }
}
