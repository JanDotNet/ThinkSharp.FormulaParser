using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.TermRewriting.Rules
{
    public class RemoveBracketsInTermPlusMinusBrackets : Rule
    {
        protected override bool MatchInternal(Node node, Node parent)
        {
            return (parent == null || parent is FormulaNode)
                && node is BinaryOperatorNode binaryOperatorNode
                && binaryOperatorNode.IsPlusOrMinus()
                && (binaryOperatorNode.LeftNode is BracketedNode || binaryOperatorNode.RightNode is BracketedNode);
        }

        protected override Node RewriteInternal(Node node, Node parent)
        {
            if ((parent == null || parent is FormulaNode) 
                && node is BinaryOperatorNode binaryOperatorNode)
            {
                if (binaryOperatorNode.IsPlus())
                {
                    if (TryRemoveBrackets(binaryOperatorNode.LeftNode, out Node left)
                      | TryRemoveBrackets(binaryOperatorNode.RightNode, out Node right))
                    {
                        return new BinaryOperatorNode(binaryOperatorNode.BinaryOperator, left, right);
                    }
                }

                if (binaryOperatorNode.IsMinus())
                {
                    if (TryRemoveBrackets(binaryOperatorNode.LeftNode, out Node left))
                    {
                        return new BinaryOperatorNode(binaryOperatorNode.BinaryOperator, left, binaryOperatorNode.RightNode);
                    }

                    if (TryRemoveBrackets(binaryOperatorNode.RightNode, out Node right))
                    {
                        return new BinaryOperatorNode(binaryOperatorNode.BinaryOperator, binaryOperatorNode.LeftNode, right.SwitchSignForAllSummands());
                    }
                }
            }

            Debug.Fail("Should not happen");
            return node;
        }

        private static bool TryRemoveBrackets(Node node, out Node rewritten)
        {
            if (node is BracketedNode bracketed)
            {
                rewritten = bracketed.ChildNode;
                return true;
            }

            rewritten = node;
            return false;
        }
    }
}
