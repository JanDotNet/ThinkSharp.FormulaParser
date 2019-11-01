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
    public class RemoveBracketsInTermPlusMinusBracketsTest : RuleTest
    {
        protected override Rule CreateRule() => new RemoveBracketsInTermPlusMinusBrackets();

        [TestMethod]
        public void Test_APlusAInBracketsPlusA() => AssertRewritten("a+a+a", "(a+a)+a");

        [TestMethod]
        public void Test_APlusAPlusAInBrackets() => AssertRewritten("a+a+a", "a+(a+a)");

        [TestMethod]
        public void Test_APlusAInBracketsMinusA() => AssertRewritten("a+a-a", "(a+a)-a");

        [TestMethod]
        public void Test_AMinusAPlusAInBrackets() => AssertRewritten("a-a-a", "a-(a+a)");
    }
}
