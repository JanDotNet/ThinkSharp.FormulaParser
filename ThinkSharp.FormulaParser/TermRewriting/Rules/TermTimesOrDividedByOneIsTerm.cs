using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.TermRewriting.Rules
{
    public class TermTimesOrDividedByOneIsTerm : Rule
    {
        protected override bool MatchInternal(Node node, Node Parent)
        {
            if (node is BinaryOperatorNode opNode)
            {
                var isTimes = opNode.BinaryOperator.Symbol == "*";
                var isDividedBy = opNode.BinaryOperator.Symbol == "/";
                
                if (isTimes && opNode.LeftNode.EqualsNumber(1))
                {
                    return true;
                }

                if ((isTimes || isDividedBy) && opNode.RightNode.EqualsNumber(1))
                {
                    return true;
                }
            }

            return false;
        }

        protected override Node RewriteInternal(Node node, Node Parent)
        {
            if (node is BinaryOperatorNode opNode)
            {
                var isTimes = opNode.BinaryOperator.Symbol == "*";
                var isDividedBy = opNode.BinaryOperator.Symbol == "/";

                if (isTimes && opNode.LeftNode.EqualsNumber(1))
                {
                    return opNode.RightNode;
                }

                if ((isTimes || isDividedBy) && opNode.RightNode.EqualsNumber(1))
                {
                    return opNode.LeftNode;
                }
            }

            Debug.Fail("Should not happen");
            return node;
        }
    }
}
