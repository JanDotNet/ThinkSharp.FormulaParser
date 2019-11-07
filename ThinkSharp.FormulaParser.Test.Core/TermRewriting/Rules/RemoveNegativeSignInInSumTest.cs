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
    public class RemoveNegativeSignInInSumTest : RuleTest
    {
        protected override Rule CreateRule() => new RemoveNegativeSignInInSum();

        [TestMethod]
        public void Test_MinusAPlusB() => AssertRewritten("b-a", "-a+b");

        [TestMethod]
        public void Test_AMinusB() => AssertNotMatching("a-b");

        [TestMethod]
        public void Test_AMinusMinusB() => AssertNotMatching("a--b");
    }
}
