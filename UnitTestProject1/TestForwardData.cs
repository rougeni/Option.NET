using System;
using ProjetNET.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
