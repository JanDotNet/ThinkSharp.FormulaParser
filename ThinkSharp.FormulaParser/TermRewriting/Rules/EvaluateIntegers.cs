using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.TermRewriting.Rules
{
    public class EvaluateIntegers : Rule
    {
        protected override bool MatchInternal(Node node, Node parent)
        {
            return node is BinaryOperatorNode binOpNode
                && (binOpNode.IsPlusOrMinus() || binOpNode.IsMultipy())
                && binOpNode.LeftNode is IntegerNode
                && binOpNode.RightNode is IntegerNode;
        }

        protected override Node RewriteInternal(Node node, Node parent)
        {
            if (node is BinaryOperatorNode binOpNode
                && (binOpNode.IsPlusOrMinus() || binOpNode.IsMultipy())
                && binOpNode.LeftNode is IntegerNode leftInt
                && binOpNode.RightNode is IntegerNode rightInt)
            {
                var value = binOpNode.BinaryOperator.Evaluate(leftInt.Value, rightInt.Value);
                return new IntegerNode(NumberFormat.Dec, value);
            }

            Debug.Fail("Should not happen");
            return node;
        }
    }
}
