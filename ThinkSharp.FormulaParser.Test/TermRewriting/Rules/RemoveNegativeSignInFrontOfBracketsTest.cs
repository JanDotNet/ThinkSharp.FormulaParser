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
    public class RemoveNegativeSignInFrontOfBracketsTest : RuleTest
    {
        protected override Rule CreateRule() => new RemoveNegativeSignInFrontOfBrackets();

        [TestMethod]
        public void Test_MinusA() => AssertRewritten("(-a)", "-(a)");

        [TestMethod]
        public void Test_MinusAPlusB() => AssertRewritten("(-a-b)", "-(a+b)");

        [TestMethod]
        public void Test_MinusMinusB() => AssertRewritten("(--a+b)", "-(-a-b)");

        [TestMethod]
        public void Test_MinusAPlusBTimesBracket() => AssertRewritten("(-a-b*(a-b))", "-(a+b*(a-b))");

        [TestMethod]
        public void Test_MinusMinusA() => AssertRewritten("(--a)", "-(-a)");

        [TestMethod]
        public void Test_MinusProduct() => AssertRewritten("(--a*b*c*d)", "-(-a*b*c*d)");
    }
}
