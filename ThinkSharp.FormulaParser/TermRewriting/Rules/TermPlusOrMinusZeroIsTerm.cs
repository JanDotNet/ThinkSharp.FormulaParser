using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.TermRewriting.Rules
{
    public class TermPlusOrMinusZeroIsTermRule : Rule
    {
        protected override bool MatchInternal(Node node, Node Parent)
        {
            if (node is BinaryOperatorNode opNode)
            {
                if (opNode.BinaryOperator.Symbol == "+" ||
                    opNode.BinaryOperator.Symbol == "-")
                {
                    return opNode.LeftNode.EqualsNumber(0) || opNode.RightNode.EqualsNumber(0);
                }
            }

            return false;
        }

        protected override Node RewriteInternal(Node node, Node Parent)
        {
            if (node is BinaryOperatorNode opNode)
            {
                var isAddOperation = opNode.BinaryOperator.Symbol == "+";
                var isMinusOperation = opNode.BinaryOperator.Symbol == "-";

                if (isAddOperation || isMinusOperation)
                {
                    if (opNode.LeftNode.EqualsNumber(0))
                    {
                        return isMinusOperation
                            ? new SignedNode(Sign.Minus, opNode.RightNode)
                            : opNode.RightNode;
                    }

                    if (opNode.RightNode.EqualsNumber(0))
                    {
                        return opNode.LeftNode;
                    }
                }
            }

            Debug.Fail("Should not happen");
            return node;
        }
    }
}
