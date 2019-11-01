using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.Utils;

namespace ThinkSharp.FormulaParsing.Test
{
    [TestClass]
    public class NodeComparerTest
    {
        private static IEqualityComparer<Node> comparer = new NodeComparer();
        private static IFormulaParser parser = FormulaParser
            .CreateBuilder()
            .ConfigureValidationBehavior(b =>
            {
                b.DisableVariableNameValidation();
                b.DisableFunctionNameValidation();
            })
            .Build();

        [TestMethod]
        public void TestComparePlusEquals()
        {
            AssertAreEqual("1+2", "2+1");
            AssertAreEqual("1*2", "2*1");
            AssertAreEqual("1+a*c+3", "3+c*a+1");
            AssertAreEqual("1+a*c+3", "1+3+c*a");
            AssertAreEqual("a+c*(1+3*3+4)+2+3+4", "2+3+(3*3+4+1)*c+a+4");
        }

        [TestMethod]
        public void TestComparePowEquals() 
            => AssertAreEqual("(a+b)^(1+2*b+3)", "(b+a)^(3+b*2+1)");

        [TestMethod]
        public void TestCompareFunctionEquals()
            => AssertAreEqual("3*test(a+1,1+b,d*c)", "test(1+a,b+1,c*d)*3");

        [TestMethod]
        public void TestCompareDividedByEquals()
            => AssertAreEqual("3*(a+b+c)/2*(f*d*s)", "(c+a+b)*3/2*(d*f*s)");

        //[TestMethod]
        public void TestCompareMinusEquals()
        {
            AssertAreEqual("1-3", "-3+1");
        }

        private void AssertAreEqual(params string[] formulas)
        {
            if (formulas.Length < 2)
                Assert.Fail("Need at least 2 formulas!");

            var nodes = formulas.Select(f => parser.Parse(f).Value).ToArray();
            for (int i = 0; i < nodes.Length; i++)
            { 
                for (int j = i; j < nodes.Length; j++)
                {
                    Assert.IsTrue(comparer.Equals(nodes[i], nodes[j]));
                }
            }
        }
    }
}
