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
    public class TermTimesOrDividedByMinusOneIsMinusTermTest : RuleTest
    {
        protected override Rule CreateRule() => new TermTimesOrDividedByMinusOneIsMinusTerm();

        [TestMethod]
        public void Test_OneTImesOne() => AssertRewritten(
            new IntegerNode(-1),
            new BinaryOperatorNode(
                BinaryOperator.BySymbol("*"),
                new IntegerNode(1),
                new IntegerNode(-1)));

        [TestMethod]
        public void Test_ATimesOne() => AssertRewritten(
            new SignedNode(Sign.Minus, new VariableNode("a")),
            new BinaryOperatorNode(
                BinaryOperator.BySymbol("*"),
                new VariableNode("a"),
                new IntegerNode(-1)));

        [TestMethod]
        public void Test_OneTimesA() => AssertRewritten(
            new SignedNode(Sign.Minus, new VariableNode("a")),
            new BinaryOperatorNode(
                BinaryOperator.BySymbol("*"),
                new IntegerNode(-1),
                new VariableNode("a")));

        [TestMethod]
        public void Test_ADividedByOne() => AssertRewritten(
            new BinaryOperatorNode(
                BinaryOperator.BySymbol("/"),
                new IntegerNode(1),
                new SignedNode(Sign.Minus, new VariableNode("a"))),
            new BinaryOperatorNode(
                BinaryOperator.BySymbol("/"),
                new IntegerNode(-1),
                new VariableNode("a")));

        [TestMethod]
        public void Test_MinsADividedByMinusOne() => AssertRewritten(            
                new VariableNode("a"),
            new BinaryOperatorNode(
                BinaryOperator.BySymbol("/"),
                new SignedNode(Sign.Minus, new VariableNode("a")),
                new IntegerNode(-1)));
    }
}
