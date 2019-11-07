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
    public class RemoveRootBracketsTest : RuleTest
    {
        protected override Rule CreateRule() => new RemoveRootBrackets();

        [TestMethod]
        public void Test_APlusAInBrackets() => AssertRewritten("a+a", "(a+a)");

        [TestMethod]
        public void Test_APlusAInBracketsPlusAInBrackets() => AssertRewritten("(a+a)+a", "((a+a)+a)");

        [TestMethod]
        public void Test_APlusAPlusAInBracketsInBrackets() => AssertRewritten("a+(a+a)", "(a+(a+a))");

        [TestMethod]
        public void Test_APlusAPlusAInBracketsPlusCInBrackets() => AssertRewritten("a+(a+a)+c", "(a+(a+a)+c)");
    }
}
