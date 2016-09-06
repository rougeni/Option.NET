using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetNET.Data;
using System.Diagnostics;

namespace TestUnitaires
{
    [TestClass]
    public class TestConnection
    {
        [TestMethod]
        public void TestEtablissementConnection()
        {
            DataConnection dataConn = new DataConnection() ;
            dataConn.ConnectToSql();
        }
    }
}
