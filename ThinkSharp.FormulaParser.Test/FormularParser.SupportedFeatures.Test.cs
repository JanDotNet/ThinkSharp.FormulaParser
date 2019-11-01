using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FormulaParsing.Test
{
    [TestClass]
    public class ConfigureSupportedFeaturesTest
    {      
        [TestMethod]
        public void TestDisablePow()
        {
            var parser = FormulaParser
                .CreateBuilder()
                .ConfigureSupportedFeatures(supportedFeatures =>
                {
                    supportedFeatures.DisablePow();
                })
                .Build();

            var result = parser.Evaluate("2^3");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("column 1: Invalid token '^'.", (string)result.Error);
        }

        [TestMethod]
        [Ignore]
        public void TestDisableScientificNotation()
        {
            var parser = FormulaParser
                .CreateBuilder()
                .ConfigureSupportedFeatures(supportedFeatures =>
                {
                    supportedFeatures.DisableScientificNotation();
                })
                .Build();

            var result = parser.Evaluate("2e3");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("column 0: Invalid token '2e3'.", (string)result.Error);
        }

        [TestMethod]
        public void TestDisableVariables()
        {
            var parser = FormulaParser
                .CreateBuilder()
                .ConfigureSupportedFeatures(supportedFeatures =>
                {
                    supportedFeatures.DisableVariables();
                })
                .Build();

            var variables = new Dictionary<string, double>
            {
                ["TEST"] = 3.0
            };
            var result = parser.Evaluate("2 * TEST", variables);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("column 4: Invalid token 'TEST'.", (string)result.Error);
        }

        [TestMethod]
        public void TestDisableFunctions()
        {
            var parser = FormulaParser
                .CreateBuilder()
                .ConfigureSupportedFeatures(supportedFeatures =>
                {
                    supportedFeatures.DisableFunctions();
                })
                .Build();
            var result = parser.Evaluate("2 * max(1,2)");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("column 4: Invalid token 'max'.", (string)result.Error);
        }

        [TestMethod]
        public void TestDisableBracket()
        {
            var parser = FormulaParser
                .CreateBuilder()
                .ConfigureSupportedFeatures(supportedFeatures =>
                {
                    supportedFeatures.DisableBracket();
                })
                .Build();
            var result = parser.Evaluate("2 * (1 + 2)");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("column 4: Invalid token '('.", (string)result.Error);
        }

        [TestMethod]
        public void TestDisableBinaryNumberNotation()
        {
            var parser = FormulaParser
                .CreateBuilder()
                .ConfigureSupportedFeatures(supportedFeatures =>
                {
                    supportedFeatures.DisableBinaryNumberNotation();
                })
                .Build();
            var result = parser.Evaluate("0b101");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("column 0: Invalid token '0b101'.", (string)result.Error);
        }

        [TestMethod]
        public void TestDisableHexadecimalNumberNotation()
        {
            var parser = FormulaParser
                .CreateBuilder()
                .ConfigureSupportedFeatures(supportedFeatures =>
                {
                    supportedFeatures.DisableHexadecimalNumberNotation();
                })
                .Build();
            var result = parser.Evaluate("0xABC");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("column 0: Invalid token '0xABC'.", (string)result.Error);
        }
    }
}
