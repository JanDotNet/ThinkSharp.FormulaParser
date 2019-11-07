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
    public class RemoveNegativSignFromNumberTest : RuleTest
    {
        protected override Rule CreateRule() => new RemoveNegativeSignFromNumber();

        [TestMethod]
        public void Test_MinusOne() => AssertRewritten(new IntegerNode(-1), new SignedNode(Sign.Minus, new IntegerNode(1)));
        [TestMethod]
        public void Test_MinusDecimal() => AssertRewritten(new DecimalNode(-1), new SignedNode(Sign.Minus, new DecimalNode(1)));
        [TestMethod]
        public void Test_MinusVariable() => AssertNotMatching(new SignedNode(Sign.Minus, new VariableNode("TEST")));
    }
}
