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
    public class RemovePositiveSignTest : RuleTest
    {
        protected override Rule CreateRule() => new RemovePositiveSign();
        [TestMethod]
        public void Test_PlusPointFourOne() => AssertRewritten("1.40", "+1.4");
        [TestMethod]
        public void Test_PlusOne() => AssertRewritten("1", "+1");
        [TestMethod]
        public void Test_MinusOne() => AssertNotMatching("-1");
        [TestMethod]
        public void Test_PlusOneMinusOneInBrackets() => AssertRewritten("(1-1)", "+(1-1)");
        [TestMethod]
        public void Test_MinusOneMinusOneInBrackets() => AssertNotMatching("-(1-1)");
        [TestMethod]
        public void Test_PlusOnePlusOneInBrackets() => AssertRewritten("(1+1)", "+(1+1)");
        [TestMethod]
        public void Test_MinusOnePlusOneInBrackets() => AssertNotMatching("-(1+1)");
        [TestMethod]
        public void Test_PlusVariable() => AssertRewritten("a", "+a");
        [TestMethod]
        public void Test_MinusVariable() => AssertNotMatching("-a");
        [TestMethod]
        public void Test_PlusFunction() => AssertRewritten("max(3, 2)", "+max(3,2)");
        [TestMethod]
        public void Test_MinusFunction() => AssertNotMatching("-max(3,2)");
    }
}
