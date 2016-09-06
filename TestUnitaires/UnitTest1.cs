using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetNET.Data;
using System.Diagnostics;

namespace TestUnitaires
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ProjetNET.Data.Action action = new ProjetNET.Data.Action("bonjour",10,10);
            String testAfficher = action.afficher();
            Debug.Assert(testAfficher.Equals("bonjour"));
        }
    }
}
