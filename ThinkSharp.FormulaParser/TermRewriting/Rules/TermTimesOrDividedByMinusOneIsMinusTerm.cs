using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.TermRewriting.Rules
{
    public class TermTimesOrDividedByMinusOneIsMinusTerm : Rule
    {
        protected override bool MatchInternal(Node node, Node Parent)
        {
            if (node is BinaryOperatorNode opNode)
            {
                var isTimes = opNode.BinaryOperator.Symbol == "*";
                var isDividedBy = opNode.BinaryOperator.Symbol == "/";

                return (isTimes || isDividedBy) &&
                       (opNode.LeftNode.EqualsNumber(-1) || opNode.RightNode.EqualsNumber(-1));
            }

            return false;
        }

        protected override Node RewriteInternal(Node node, Node Parent)
        {
            if (node is BinaryOperatorNode opNode)
            {
                var isTimes = opNode.BinaryOperator.Symbol == "*";
                var isDividedBy = opNode.BinaryOperator.Symbol == "/";

                if (isTimes && opNode.LeftNode.EqualsNumber(-1))
                {
                    return opNode.RightNode.SwitchSign();
                }

                if (isDividedBy && opNode.LeftNode.EqualsNumber(-1))
                {
                    return new BinaryOperatorNode(
                        opNode.BinaryOperator,
                        new IntegerNode(1),
                        opNode.RightNode.SwitchSign());
                }

                if ((isTimes || isDividedBy) && opNode.RightNode.EqualsNumber(-1))
                {
                    return opNode.LeftNode.SwitchSign();
                }
            }

            Debug.Fail("Should not happen");
            return node;
        }
    }
}
