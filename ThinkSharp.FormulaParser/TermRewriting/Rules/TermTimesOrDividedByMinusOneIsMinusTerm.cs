using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.TermRewriting.Rules
{
    public class TermTimesOrDividedByMinusOneIsMinusTerm : Rule
    {
        public override bool Match(Node node)
        {
            if (node is BinaryOperatorNode opNode)
            {
                var isTimes = opNode.BinaryOperator.Symbol == "*";
                var isDividedBy = opNode.BinaryOperator.Symbol == "/";
                
                if (isTimes && opNode.LeftNode is NumberNode leftNumNode)
                {
                    if (leftNumNode.Value == -1.0)
                    {
                        return true;
                    }
                }

                if ((isTimes || isDividedBy) && opNode.RightNode is NumberNode rightNumNode)
                {
                    if (rightNumNode.Value == -1.0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override Node Rewrite(Node node)
        {
            if (node is BinaryOperatorNode opNode)
            {
                var isTimes = opNode.BinaryOperator.Symbol == "*";
                var isDividedBy = opNode.BinaryOperator.Symbol == "/";

                if (isTimes && opNode.LeftNode is NumberNode leftNumNode)
                {
                    if (leftNumNode.Value == -1.0)
                    {
                        if (opNode.RightNode is SignedNode rightSignedNode)
                        {
                            if (rightSignedNode.Sign == Sign.Minus)
                            {
                                return rightSignedNode.Node;
                            }
                            return new SignedNode(Sign.Minus, rightSignedNode.Node);
                        }

                        return new SignedNode(Sign.Minus, opNode.RightNode);
                    }
                }

                if ((isTimes || isDividedBy) && opNode.RightNode is NumberNode rightNumNode)
                {
                    if (rightNumNode.Value == -1.0)
                    {
                        if (opNode.LeftNode is SignedNode leftSignedNode)
                        {
                            if (leftSignedNode.Sign == Sign.Minus)
                            {
                                return leftSignedNode.Node;
                            }

                            return new SignedNode(Sign.Minus, leftSignedNode.Node);
                        }

                        return new SignedNode(Sign.Minus, opNode.LeftNode);
                    }
                }
            }

            Debug.Fail("Should not happen");
            return node;
        }
    }
}
