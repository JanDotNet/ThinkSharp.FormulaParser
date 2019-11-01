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
    public class RemoveNegativeSignTimesNegatieSignTest : RuleTest
    {
        protected override Rule CreateRule() => new RemoveNegativeSignTimesNegatieSign();

        [TestMethod]
        public void Test_MinusMinusA () => AssertRewritten(
            new VariableNode("a"), 
            new SignedNode(Sign.Minus, 
                  new SignedNode(Sign.Minus, 
                      new VariableNode("a"))));

        [TestMethod]
        public void Test_MinusMinusAPlusB() => AssertRewritten(
            new BinaryOperatorNode(
                    BinaryOperator.Plus,
                    new VariableNode("a"),
                    new VariableNode("b")),
            new SignedNode(Sign.Minus,
                new BinaryOperatorNode(
                    BinaryOperator.Plus,
                    new SignedNode(Sign.Minus, new VariableNode("a")),
                    new VariableNode("b"))));

        [TestMethod]
        public void Test_MinusMinusATimesB() => AssertRewritten(
            new BinaryOperatorNode(
                    BinaryOperator.Times,
                    new VariableNode("a"),
                    new VariableNode("b")),
            new SignedNode(Sign.Minus,
                new BinaryOperatorNode(
                    BinaryOperator.Times,
                    new SignedNode(Sign.Minus, new VariableNode("a")),
                    new VariableNode("b"))));
    }
}
