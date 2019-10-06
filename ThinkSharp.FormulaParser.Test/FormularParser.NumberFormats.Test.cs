using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing.Test
{
    [TestClass]
    public class FormularParserNumberFormatsTest
    {
        [TestMethod]
        public void TestBinayNotation()
        {
            var parser = FormulaParser.Create();

            Assert.AreEqual(1.0, parser.Evaluate("0b1").Value);
            Assert.AreEqual(5.0, parser.Evaluate("0b101").Value);

            Assert.AreEqual(10.0, parser.Evaluate("0b101 + 0b101").Value);
        }

        [TestMethod]
        public void TestHexadecimalNotation()
        {
            var parser = FormulaParser.Create();

            Assert.AreEqual(1.0, parser.Evaluate("0x1").Value);
            Assert.AreEqual(10.0, parser.Evaluate("0xa").Value);
            Assert.AreEqual(10.0, parser.Evaluate("0xA").Value);

            Assert.AreEqual(477580.0, parser.Evaluate("0x7498C").Value);
            Assert.AreEqual(477580.0, parser.Evaluate("0X7498C").Value);

            Assert.AreEqual(100.0, parser.Evaluate("0xA * 0Xa").Value);
        }

        [TestMethod]
        public void TestDecimalNotation()
        {
            var parser = FormulaParser.Create();

            Assert.AreEqual(1.0, parser.Evaluate("0d1").Value);
            Assert.AreEqual(101.0, parser.Evaluate("0D101").Value);
            Assert.AreEqual(101.0, parser.Evaluate("101").Value);

            Assert.AreEqual(202.0, parser.Evaluate("0d101 + 101").Value);
        }

        [TestMethod]
        public void TestScientificNotation()
        {
            var parser = FormulaParser.Create();

            Assert.AreEqual(1000.0, parser.Evaluate("1e3").Value);
            Assert.AreEqual(1110.0, parser.Evaluate("1.11e3").Value);
        }

        [TestMethod]
        public void TestMixedNotation()
        {
            var parser = FormulaParser.Create();

            Assert.AreEqual(0.04, parser.Evaluate("(0b11 + 0x20 + 5) * 1e-3").Value);
        }
    }
}
