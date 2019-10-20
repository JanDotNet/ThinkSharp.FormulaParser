using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing.TermRewriting.Rules
{
    public class MoveOuterSignedToInnerTerm : Rule
    {
        private class SwitchSignWithinBracketsVisitor : INodeVisitor<Node>
        {
            public Node Visit(FormulaNode node) => node.ChildNode.Visit(this);

            public Node Visit(BracketedNode node) => node;

            public Node Visit(NumberNode node) => node;

            public Node Visit(VariableNode node) => node;

            public Node Visit(ConstantNode node) => node;

            public Node Visit(BinaryOperatorNode node)
            {
                var newLeft = node.LeftNode.Visit(this);
                var newRight = node.RightNode.Visit(this);

                var symbol = node.BinaryOperator.Symbol;
                var op = symbol == "+"  ? BinaryOperator.BySymbol("-") 
                    :    symbol == "-"  ? BinaryOperator.BySymbol("+") 
                    : node.BinaryOperator;
    
                return new BinaryOperatorNode(op, newLeft, newRight);
            }

            public Node Visit(PowerNode node) => node;

            public Node Visit(SignedNode node) => node;

            public Node Visit(FunctionNode node) => node;
        }

        public override bool Match(Node node)
        {
            if (node is SignedNode signNode)
            {
                if (signNode.Sign == Sign.Plus)
                {
                    return true;
                }

                var targetNode = signNode.Node;
                while (targetNode is BinaryOperatorNode binaryOperationNode)
                {
                    targetNode = binaryOperationNode.LeftNode;
                }

                if (targetNode == signNode.Node)
                {
                    return targetNode is BracketedNode ||
                           targetNode is NumberNode;
                }

                return targetNode is BracketedNode ||
                       targetNode is SignedNode ||
                       targetNode is ConstantNode ||
                       targetNode is FunctionNode ||
                       targetNode is NumberNode ||
                       targetNode is PowerNode ||
                       targetNode is VariableNode;
            }

            return false;
        }

        public override Node Rewrite(Node node)
        {
            if (node is SignedNode signNode)
            {
                if (signNode.Sign == Sign.Plus)
                {
                    return signNode.Node;
                }

                var targetNode = signNode.Node;
                if (targetNode is BracketedNode targetBracketedNode)
                {
                    var newBracketedContentNode = targetBracketedNode.Visit(new SwitchSignWithinBracketsVisitor());
                    return new SignedNode(Sign.Minus, newBracketedContentNode);
                }

                var binOpQueue = new Queue<BinaryOperatorNode>();
                while (targetNode is BinaryOperatorNode binaryOperationNode)
                {
                    binOpQueue.Enqueue(binaryOperationNode);
                    targetNode = binaryOperationNode.LeftNode;
                }

                var transformedNode = TransformedNode(targetNode);

                if (transformedNode != null)
                {
                    Node leftNode = transformedNode;
                    while (binOpQueue.Count > 0)
                    {
                        var binOpFromQueue = binOpQueue.Dequeue();
                        leftNode = new BinaryOperatorNode(binOpFromQueue.BinaryOperator, leftNode, binOpFromQueue.RightNode);
                    }
                    return leftNode;
                }
            }

            Debug.Fail("Should not happen");
            return node;
        }

        private Node TransformedNode(Node targetNode)
        {
            if (targetNode is BracketedNode targetBracketedNode)
            {
                var newBracketedContentNode = targetBracketedNode.Visit(new SwitchSignWithinBracketsVisitor());
                var newSignedBracketedContentNode = new SignedNode(Sign.Minus, newBracketedContentNode);
                return new BracketedNode(newSignedBracketedContentNode);
            }

            if (targetNode is SignedNode targetSignedNode)
            {
                if (targetSignedNode.Sign == Sign.Minus)
                {
                    return targetSignedNode.Node;
                }
                return new SignedNode(Sign.Minus, targetSignedNode.Node);
            }

            if (targetNode is ConstantNode
                || targetNode is FunctionNode
                || targetNode is PowerNode
                || targetNode is VariableNode)
            {
                return new SignedNode(Sign.Minus, targetNode);
            }

            if (targetNode is NumberNode numberNode)
            {
                var token = numberNode.Token[0] == '-'
                ? numberNode.Token.Substring(1)
                : "-" + numberNode.Token;
                return new NumberNode(token, numberNode.Value * -1);
            }

            return null;
        }
    }
}
