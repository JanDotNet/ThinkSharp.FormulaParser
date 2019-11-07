using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing.TermRewriting.Rules
{
    public class RearrangeIntegersInSum : Rule
    {
        protected override bool MatchInternal(Node node, Node parent)
            => FindBinaryNodesWithInteger(node).Skip(1).Any();

        private class RearrangeVisitor : TransformNodesVisitor
        {
            private readonly BinaryOperatorNode first;
            private readonly BinaryOperatorNode second;

            private readonly Node firstReplacement;
            private readonly Node firstInteger;

            public RearrangeVisitor(BinaryOperatorNode first, BinaryOperatorNode second)
            {
                this.first = first;
                this.second = second;

                if (first.LeftNode is IntegerNode firstLeftInteger)
                {
                    firstInteger = firstLeftInteger;
                    firstReplacement = first.IsMinus()
                        ? new SignedNode(Sign.Minus, first.RightNode)
                        : first.RightNode;
                }
                else if (first.RightNode is IntegerNode firstRightInteger)
                {
                    firstReplacement = first.LeftNode;
                    firstInteger = first.IsMinus()
                            ? new IntegerNode(firstRightInteger.Format, firstRightInteger.Value * -1)
                            : firstRightInteger;
                }
                else
                {
                    throw new ArgumentException("first: left or right node must be an integer.");
                }
            }

            public override Node Visit(BinaryOperatorNode node)
            {
                if (node == first)
                {
                    return firstReplacement.Visit(this);
                }

                if (node == second)
                {
                    if (node.LeftNode is IntegerNode)
                    {
                        return new BinaryOperatorNode(node.BinaryOperator,
                            new BinaryOperatorNode(BinaryOperator.Plus,
                                node.LeftNode.Visit(this),
                                firstInteger),
                            node.RightNode.Visit(this));
                    }
                    if (node.RightNode is IntegerNode)
                    {
                        return new BinaryOperatorNode(BinaryOperator.Plus,
                            node.LeftNode.Visit(this),
                            new BinaryOperatorNode(node.BinaryOperator,
                                firstInteger,
                                node.RightNode.Visit(this)));
                    }
                    else
                    {
                        throw new ArgumentException("second: left or right node must be an integer.");
                    }
                }

                return base.Visit(node);
            }
        }

        private static IEnumerable<BinaryOperatorNode> FindBinaryNodesWithInteger(Node node)
        {
            if (node is BinaryOperatorNode binOpNode && binOpNode.IsPlusOrMinus())
            {
                var isLeftInteger = binOpNode.LeftNode is IntegerNode;
                var isRightInteger = binOpNode.RightNode is IntegerNode;
                if (isLeftInteger ^ isRightInteger)
                {
                    yield return binOpNode;
                }

                foreach (var children in FindBinaryNodesWithInteger(binOpNode.LeftNode))
                {
                    yield return children;
                }

                foreach (var children in FindBinaryNodesWithInteger(binOpNode.RightNode))
                {
                    yield return children;
                }
            }
        }

        protected override Node RewriteInternal(Node node, Node parent)
        {
            var binOpNodesIntegers = FindBinaryNodesWithInteger(node).Take(2).ToArray();

            if (binOpNodesIntegers.Length == 2)
            {
                var visitor = new RearrangeVisitor(binOpNodesIntegers[0], binOpNodesIntegers[1]);
                return node.Visit(visitor);
            }

            Debug.Fail("Should not happen");
            return node;
        }
    }
}
