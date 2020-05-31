using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Sharpen;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing
{
    [TestClass]
    public class FormularParserCustomVisitorTest
    {
        [TestMethod]
        public void TestRunCostumVisitor()
        {
            var variables = new Dictionary<string, double>
            {
                ["x"] = 2
            };

            var parser = FormulaParser.Create();

            var node = parser.Parse("1 + 2");

            var transformedNode1 = parser.RunVisitor(node, new SwitchPlusAndMinus());

            var transformedNode2 = parser.RunVisitor("1 + 2", new SwitchPlusAndMinus());

            var transformedNode3 = parser.RunVisitor("1 + x", new SwitchPlusAndMinus(), variables);

            foreach (var transformedNode in new[] {transformedNode1, transformedNode2, transformedNode3})
            {
                var result = parser.Evaluate(transformedNode, variables);
                Assert.AreEqual(-1.0, result.Value);
            }
        }

        private class SwitchPlusAndMinus : NodeVisitor<Node>
        {
            public override Node Visit(BinaryOperatorNode node)
            {
                BinaryOperator newBinaryOperator = node.BinaryOperator;
                switch (node.BinaryOperator.Symbol)
                {
                    case "+":
                        newBinaryOperator = BinaryOperator.BySymbol("-");
                        break;
                    case "-":
                        newBinaryOperator = BinaryOperator.BySymbol("+");
                        break;
                }

                return new BinaryOperatorNode(newBinaryOperator, node.LeftNode.Visit(this), node.RightNode.Visit(this));
            }

            public override Node Visit(VariableNode node)
            {
                if (node.Name == "x")
                {
                    return new DecimalNode( 2);
                }

                throw new InvalidOperationException("Unknown Variable.");
            }

            public override Node Visit(FormulaNode node)
            {
                return new FormulaNode(node.ChildNode.Visit(this));
            }

            public override  Node Visit(DecimalNode node) => node;

            public override Node Visit(IntegerNode node) => node;
        }
    }
}
