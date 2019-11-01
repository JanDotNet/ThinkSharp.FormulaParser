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
    public class RemoveNestedBracketsTest : RuleTest
    {
        protected override Rule CreateRule() => new RemoveNestedBrackets();

        [TestMethod]
        public void Test_APlusAInBrackets() => AssertRewritten("(a+a)", "((a+a))");

        [TestMethod]
        public void Test_APlusAInBracketsPlusA() => AssertNotMatching("((a+a)+a)");

        [TestMethod]
        public void Test_APlusAPlusAInBrackets() => AssertNotMatching("(a+(a+a))");

        [TestMethod]
        public void Test_APlusAPlusAInBracketsPlusC() => AssertNotMatching("(a+(a+a)+c)");

        [TestMethod]
        public void Test_APlusA() => AssertRewritten("(a+a)", "(((a+a)))");
    }
}
