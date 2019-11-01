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
    public class TermPlusOrMinusZeroIsTermTest : RuleTest
    {
        protected override Rule CreateRule() => new TermPlusOrMinusZeroIsTermRule();

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
    }
}
