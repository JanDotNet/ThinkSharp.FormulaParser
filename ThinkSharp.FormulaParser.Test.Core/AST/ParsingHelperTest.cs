using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.Test
{
    [TestClass]
    public class ParsingHelperTest
    {
        [TestMethod]
        public void TestGetBySymbol()
        {
            var o = BinaryOperator.BySymbol("+");
            Assert.AreEqual("+", o.Symbol);
            Assert.AreEqual(2.0, o.Evaluate(1, 1));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestGetBySymbol_NotExisting()
        {
            var o = BinaryOperator.BySymbol("abv");
        }

        private static void Test(string expected, string actual)
        {
            //var node = ParsingHelper.
        }
    }
}
