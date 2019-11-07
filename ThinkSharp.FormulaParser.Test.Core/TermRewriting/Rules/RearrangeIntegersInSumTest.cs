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
    public class RearrangeIntegersInSumTest : RuleTest
    {
        protected override Rule CreateRule() => new RearrangeIntegersInSum();

        [TestMethod]
        public void Test_A() => this.AssertNotMatching("a");

        [TestMethod]
        public void Test_TenPlusAPlusTen () => AssertRewritten("10+10+a", "10+a+10");

        [TestMethod]
        public void Test_TenPlusAMinusTen() => AssertRewritten("10+-10+a", "10+a-10");

        [TestMethod]
        public void Test_TenMinusAMinusTen() => AssertRewritten("10+-10-a", "10-a-10");

        [TestMethod]
        public void Test_LongChain() => AssertRewritten("a+b+20+10+d+e+f+g+h+j+k+l", "a+b+10+d+e+f+g+h+j+k+l+20");
    }
}
