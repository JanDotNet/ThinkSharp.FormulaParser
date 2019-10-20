using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.TermRewriting;
using ThinkSharp.FormulaParsing.TermRewriting.Rules;

namespace ThinkSharp.FormulaParsing.Test.TermRewriting
{
    [TestClass]
    public class TermPlusOrMinusZeroIsTermRuleTest
    {
        private static IFormulaParser parser = FormulaParser
            .CreateBuilder()
            .ConfigureValidationBehavior(v => v.DisableVariableNameValidation())
            .Build();
        private static ITermRewritingSystem[] rewritingSystems = new ITermRewritingSystem[]
            {
                TermRewritingSystem.ForTesting(new TermPlusOrMinusZeroIsTermRule())
            };

        [TestMethod]
        public void Test_OnePlusZeroIsOne() => AssertRewritten("1", "1+0");

        [TestMethod]
        public void Test_ZeroPlusOneIsOne() => AssertRewritten("1", "1+0");

        [TestMethod]
        public void Test_OneMinusZeroIsOne() => AssertRewritten("1", "1-0");

        [TestMethod]
        public void Test_ZeroMinusOneIsMinusOne() => AssertRewritten("-1", "0-1");

        [TestMethod]
        public void Test_OneTimesOnePlusZeroIsOneTimesOne() => AssertRewritten("1*1", "1*1+0");

        [TestMethod]
        public void Test_APlusMinusManyZerusPlusAIsAPlusA() => AssertRewritten("a+a", "a+0-0+0-0+0-0+0+0+0+a");

        private void AssertRewritten(string expected, string forumla)
        {
            foreach (var system in rewritingSystems)
            {
                var tree = parser.Parse(forumla).Value;

                var simlifiedTree = system.Simplify(tree);

                var nodeToTExtVisitor = new NodesToFormulaTextVisitor();

                Assert.AreEqual(expected, simlifiedTree.Visit(nodeToTExtVisitor));
            }
        }
    }
}
