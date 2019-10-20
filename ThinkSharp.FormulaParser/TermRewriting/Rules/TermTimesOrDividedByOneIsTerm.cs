using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.TermRewriting.Rules
{
    public class TermTimesOrDividedByOneIsTerm : Rule
    {
        public override bool Match(Node node)
        {
            if (node is BinaryOperatorNode opNode)
            {
                var isTimes = opNode.BinaryOperator.Symbol == "*";
                var isDividedBy = opNode.BinaryOperator.Symbol == "/";
                
                if (isTimes && opNode.LeftNode is NumberNode leftNumNode)
                {
                    if (leftNumNode.Value == 1.0)
                    {
                        return true;
                    }
                }

                if ((isTimes || isDividedBy) && opNode.RightNode is NumberNode rightNumNode)
                {
                    if (rightNumNode.Value == 1.0)
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
                    if (leftNumNode.Value == 1.0)
                    {
                        return opNode.RightNode;
                    }
                }

                if ((isTimes || isDividedBy) && opNode.RightNode is NumberNode rightNumNode)
                {
                    if (rightNumNode.Value == 1.0)
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
