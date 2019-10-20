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
    public class TermTimesOrDividedByOneIsTermRuleTest
    {
        private static IFormulaParser parser = FormulaParser
            .CreateBuilder()
            .ConfigureValidationBehavior(v => v.DisableVariableNameValidation())
            .Build();
        private static ITermRewritingSystem[] rewritingSystems = new ITermRewritingSystem[]
            {
                TermRewritingSystem.ForTesting(new TermTimesOrDividedByOneIsTerm())
            };

        [TestMethod]
        public void Test_OneTImesOne() => AssertRewritten("1", "1*1");

        [TestMethod]
        public void Test_ATimesOne() => AssertRewritten("a", "a*1");

        [TestMethod]
        public void Test_OneTimesA() => AssertRewritten("a", "1*a");

        [TestMethod]
        public void Test_ADividedByOne() => AssertRewritten("a", "a/1");

        [TestMethod]
        public void Test_OneDividedByA() => AssertRewritten("1/a", "1/a");

        [TestMethod]
        public void Test_AManyTimeOneTimeADividedByOne() => AssertRewritten("a*a", "a*1*1*1*1*1*a/1");

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
