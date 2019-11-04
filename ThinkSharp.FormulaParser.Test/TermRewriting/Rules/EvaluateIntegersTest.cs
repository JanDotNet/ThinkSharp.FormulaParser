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
    public class EvaluateIntegersTest : RuleTest
    {
        protected override Rule CreateRule() => new EvaluateIntegers();

        [TestMethod]
        public void Test_TenPlusTen () => AssertRewritten(
            new IntegerNode(20), 
            new BinaryOperatorNode(
                BinaryOperator.Plus, 
                  new IntegerNode(10), 
                  new IntegerNode(10)));

        [TestMethod]
        public void Test_TenMinusTen() => AssertRewritten(
            new IntegerNode(0),
            new BinaryOperatorNode(
                BinaryOperator.Minus,
                  new IntegerNode(10),
                  new IntegerNode(10)));

        [TestMethod]
        public void Test_TenTimesTen() => AssertRewritten(
            new IntegerNode(100),
            new BinaryOperatorNode(
                BinaryOperator.Multiply,
                  new IntegerNode(10),
                  new IntegerNode(10)));

        [TestMethod]
        public void Test_TenDividedByTen() => AssertNotMatching("10/10");
    }
}
