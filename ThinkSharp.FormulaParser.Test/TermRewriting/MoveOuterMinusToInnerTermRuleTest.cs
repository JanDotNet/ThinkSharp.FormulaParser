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
    public class MoveOuterMinusToInnerTermRuleTest
    {
        private static IFormulaParser parser = FormulaParser
            .CreateBuilder()
            .ConfigureValidationBehavior(v => v.DisableVariableNameValidation())
            .Build();
        private static ITermRewritingSystem[] rewritingSystems = new ITermRewritingSystem[]
            {
                TermRewritingSystem.ForTesting(new MoveOuterMinusToInnerTerm())
            };

        [TestMethod]
        public void Test_MinusOneInBrackets() => AssertRewritten("-1", "-(1)");

        [TestMethod]
        public void Test_APlusBInBrackets() => AssertRewritten("-a-b", "-(a+b)");

        [TestMethod]
        public void Test_MinusAMinusBInBrackets() => AssertRewritten("a+b", "-(-a-b)");

        [TestMethod]
        public void Test_ATimeBTimeCInBrackets() => AssertRewritten("-a*b*c", "-(a*b*c)");

        [TestMethod]
        public void Test_ATimeBMinusCInBracketsPlus3Times4() => AssertRewritten("-a*b+c+3*4", "-(a*b-c)+3*4");

        [TestMethod]
        public void Test_MaxOneTwoInBackets() => AssertRewritten("-max(1, 2)", "-(max(1, 2))");

        [TestMethod]
        public void Test_Compley1InBackets() => AssertRewritten("-pi-3*abc+max(1, 2)*3e4-2e5/3", "-(pi + 3 * abc - max(1, 2) * 3e4 +2e5 / 3)");
        

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
