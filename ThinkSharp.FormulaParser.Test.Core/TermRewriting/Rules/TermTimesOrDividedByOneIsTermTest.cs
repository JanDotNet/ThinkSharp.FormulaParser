using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ThinkSharp.FormulaParsing;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.TermRewriting.Rules;
using ThinkSharp.FormulaParsing.Test;

namespace ThinkSharp.FluentFormulaParser.Test.TermRewriting.Rules
{
    [TestClass]
    public class TermTimesOrDividedByOneIsTermTest : RuleTest
    {
        protected override Rule CreateRule() => new TermTimesOrDividedByOneIsTerm();

        [TestMethod]
        public void Test_OneTImesOne() => AssertRewritten("1", "1*1");

        [TestMethod]
        public void Test_ATimesOne() => AssertRewritten("a", "a*1");

        [TestMethod]
        public void Test_OneTimesA() => AssertRewritten("a", "1*a");

        [TestMethod]
        public void Test_ADividedByOne() => AssertRewritten("a", "a/1");

        [TestMethod]
        public void Test_TermInBracketsDividedByOne() => AssertRewritten("(1+2+4)", "(1+2+4)/1");

        [TestMethod]
        public void Test_OneDividedByA() => AssertNotMatching("1/a");
    }
}
