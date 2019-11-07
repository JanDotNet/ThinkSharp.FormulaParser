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
    public class TermWritingSystemTest
    {
        private static IFormulaParser parser = FormulaParser
            .CreateBuilder()
            .ConfigureValidationBehavior(v => v.DisableVariableNameValidation())
            .Build();

        private static ITermRewritingSystem rewritingSystem = TermRewritingSystem.Create();

        [TestMethod]
        public void Test_APlusMinusManyZerosPlusAIsAPlusA() => AssertRewritten("a+a", "a+0-0+0-0+0-0+0+0+0+a");

        [TestMethod]
        public void Test_MinusOneInBrackets() => AssertRewritten("-1", "-(1)");

        [TestMethod]
        public void Test_APlusBInBrackets() => AssertRewritten("-a-b", "-(a+b)");

        [TestMethod]
        public void Test_MinusAMinusBInBrackets() => AssertRewritten("b+a", "-(-a-b)");

        [TestMethod]
        public void Test_ATimeBTimeCInBrackets() => AssertRewritten("-a*b*c", "-(a*b*c)");

        [TestMethod]
        public void Test_ATimeBMinusCInBracketsPlus3Times4() => AssertRewritten("-a*b+c+12", "-(a*b-c)+3*4");

        [TestMethod]
        public void Test_MaxOneTwoInBackets() => AssertRewritten("-max(1, 2)", "-(max(1, 2))");

        [TestMethod]
        public void Test_Compley1InBackets() => AssertRewritten("-pi-3*abc+max(1, 2)*3*10^4-2*10^5/3", "-(pi + 3 * abc - max(1, 2) * 3*10^4 +2*10^5 / 3)");

        [TestMethod]
        public void Test_OneTimesMinusOne() => AssertRewritten("-1", "1*-1");

        [TestMethod]
        public void Test_ATimesMinusOne() => AssertRewritten("-a", "a*-1");

        [TestMethod]
        public void Test_MinusOneTimesA() => AssertRewritten("-a", "-1*a");

        [TestMethod]
        public void Test_ADividedByMinusOne() => AssertRewritten("-a", "a/-1");

        [TestMethod]
        public void Test_MinusOneDividedByA() => AssertRewritten("1/-a", "-1/a");

        [TestMethod]
        public void Test_MinsADividedByMinusOne() => AssertRewritten("a", "-a/-1");

        [TestMethod]
        public void Test_MinusOneTimesMinsADividedByMinusOne() => AssertRewritten("-a", "-1*-a/-1");

        [TestMethod]
        public void Test_MinusOneTimesMinsADividedByMinusOne2() => AssertRewritten("-a*-a", "a*-a/-1");

        [TestMethod]
        public void Test_MinusOneTimesMinsADividedByMinusOne3() => AssertRewritten("a*-a", "a*-1*-a/-1");

        [TestMethod]
        public void Test_AManyTimeMinusOneTimeADividedByMinusOne() => AssertRewritten("a*a", "a*-1*-1*-1*-1*-1*a/-1");

        [TestMethod]
        public void Test_AManyTimeOneTimeADividedByOne() => AssertRewritten("a*a", "a*1*1*1*1*1*a/1");

        [TestMethod]
        public void Test_TwoPluscPlus2() => AssertRewritten("4+c", "2+c+2");


        private void AssertRewritten(string expected, string forumla)
        {
            var tree = parser.Parse(forumla).Value;

            var simlifiedTree = rewritingSystem.Simplify(tree);

            var nodeToTExtVisitor = new NodesToFormulaTextVisitor();

            Assert.AreEqual(expected, simlifiedTree.Visit(nodeToTExtVisitor));
        }
    }
}
