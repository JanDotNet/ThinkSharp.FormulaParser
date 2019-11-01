using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.TermRewriting.Rules
{
    public class RemoveBracketsInTermPlusMinusBracketsPlusMinusTerm : Rule
    {
        protected override bool MatchInternal(Node node, Node parent)
        {
            return (parent is BinaryOperatorNode parentBinNode
                && parentBinNode.IsPlusOrMinus()
                && node is BinaryOperatorNode binNode
                && binNode.IsPlusOrMinus()
                && ((parentBinNode.LeftNode == node 
                     && binNode.RightNode is BracketedNode) 
                  || (parentBinNode.RightNode == node 
                     && binNode.LeftNode is BracketedNode)));
        }

        protected override Node RewriteInternal(Node node, Node parent)
        {
            if (parent is BinaryOperatorNode parentBinNode 
                && parentBinNode.IsPlusOrMinus()
                && node is BinaryOperatorNode binNode 
                && binNode.IsPlusOrMinus())
            {
                if (parentBinNode.LeftNode == binNode && binNode.RightNode is BracketedNode rightBracketed)
                {
                    var rightNode = rightBracketed.ChildNode;

                    if (binNode.IsMinus())
                    {
                        rightNode = rightNode.SwitchSignForAllSummands();
                    }

                    return new BinaryOperatorNode(
                            binNode.BinaryOperator,
                            binNode.LeftNode,
                            rightNode);
                }

                if (parentBinNode.RightNode == binNode && binNode.LeftNode is BracketedNode leftBracketed)
                {
                    var leftNode = leftBracketed.ChildNode;

                    if (parentBinNode.IsMinus())
                    {
                        leftNode = leftNode.SwitchSignForAllSummands();
                    }

                    return new BinaryOperatorNode(
                            binNode.BinaryOperator,
                            leftNode,
                            binNode.RightNode);
                }
            }

            Debug.Fail("Should not happen");
            return node;
        }
    }
}
