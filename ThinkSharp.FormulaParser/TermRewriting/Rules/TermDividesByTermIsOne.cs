using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.TermRewriting.Rules
{
    public class TermDividesByTermIsOne : Rule
    {
        protected override bool MatchInternal(Node node, Node Parent)
        {
            if (node is BinaryOperatorNode opNode)
            {
                if (opNode.BinaryOperator.Symbol == "+" ||
                    opNode.BinaryOperator.Symbol == "-")
                {
                    if (opNode.LeftNode is DecimalNode leftNumNode)
                    {
                        if (leftNumNode.Value == 0.0)
                        {
                            return true;
                        }
                    }

                    if (opNode.RightNode is DecimalNode rightNumNode)
                    {
                        if (rightNumNode.Value == 0.0)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        protected override Node RewriteInternal(Node node, Node Parent)
        {
            if (node is BinaryOperatorNode opNode)
            {
                var isAddOperation = opNode.BinaryOperator.Symbol == "+";
                var isSubOperation = opNode.BinaryOperator.Symbol == "-";

                if (isAddOperation || isSubOperation)
                {
                    if (opNode.LeftNode is DecimalNode leftNumNode)
                    {
                        if (leftNumNode.Value == 0.0)
                        {
                            return isSubOperation
                                ? new SignedNode(Sign.Minus, opNode.RightNode)
                                : opNode.RightNode;
                        }
                    }

                    if (opNode.RightNode is DecimalNode rightNumNode)
                    {
                        if (rightNumNode.Value == 0.0)
                        {
                            return opNode.LeftNode;
                        }
                    }
                }
            }

            Debug.Fail("Should not happen");
            return node;
        }
    }
}
