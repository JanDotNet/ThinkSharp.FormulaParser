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
    public class RemoveNegativeSignInInSubtractTest : RuleTest
    {
        protected override Rule CreateRule() => new RemoveNegativeSignInInSubtract();

        [TestMethod]
        public void Test_AMinusMinusB() => AssertRewritten("a+b", "a--b");
    }
}
