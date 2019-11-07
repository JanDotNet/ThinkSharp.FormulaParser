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
    public class RemoveBracketsInTermPlusMinusBracketsPlusMinusTermTest : RuleTest
    {
        protected override Rule CreateRule() => new RemoveBracketsInTermPlusMinusBracketsPlusMinusTerm();

        [TestMethod]
        public void Test_APlusAPlusAInBracketsPlusC() => AssertRewritten("a+a+a+c", "a+(a+a)+c");

        [TestMethod]
        public void Test_APlusAMinusAInBracketsPlusC() => AssertRewritten("a+a-a+c", "a+(a-a)+c");

        [TestMethod]
        public void Test_AMinusAMinusAInBracketsMinusC() => AssertRewritten("a-a+a-c", "a-(a-a)-c");

        [TestMethod]
        public void Test_AMinusAPlusAInBracketsPlusC() => AssertRewritten("a-a-a+c", "a-(a+a)+c");

        [TestMethod]
        public void Test_APlusAPlusAInBracketsMinusC() => AssertRewritten("a+a+a-c", "a+(a+a)-c");

        [TestMethod]
        public void Test_APlusAMinusAInBracketsMinusC() => AssertRewritten("a+a-a-c", "a+(a-a)-c");
    }
}
