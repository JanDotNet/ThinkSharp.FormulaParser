using System;
using System.Collections.Generic;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing.TermRewriting.Rules
{
    internal static class NodeExtensions
    {
        private class SwitchSignForTermsWithinBracketsVisitor : INodeVisitor<Node>
        {
            public Node Visit(FormulaNode node) => node.ChildNode.Visit(this);
            public Node Visit(BracketedNode node) => node;
            public Node Visit(DecimalNode node) => node;
            public Node Visit(VariableNode node) => node;
            public Node Visit(ConstantNode node) => node;
            public Node Visit(PowerNode node) => node;
            public Node Visit(SignedNode node) => node;
            public Node Visit(FunctionNode node) => node;
            public Node Visit(IntegerNode node) => node;
            public Node Visit(BinaryOperatorNode node)
            {
                var newLeft = node.LeftNode.Visit(this);
                var newRight = node.RightNode.Visit(this);

                var symbol = node.BinaryOperator.Symbol;
                var op = symbol == "+" ? BinaryOperator.BySymbol("-")
                    : symbol == "-" ? BinaryOperator.BySymbol("+")
                    : node.BinaryOperator;

                return new BinaryOperatorNode(op, newLeft, newRight);
            }
        }

        private class SwitchSignConcreteNodeVisitor : INodeVisitor<Node>
        {
            public Node Visit(FormulaNode node) => node.ChildNode.Visit(this);
            public Node Visit(BracketedNode node) 
                => new SignedNode(Sign.Minus, new BracketedNode(node.SwitchSignForAllSummands()));
            public Node Visit(DecimalNode node) => new DecimalNode(node.Value * -1);
            public Node Visit(VariableNode node) => new SignedNode(Sign.Minus, node);
            public Node Visit(ConstantNode node) => new SignedNode(Sign.Minus, node);
            public Node Visit(PowerNode node) => new SignedNode(Sign.Minus, node);
            public Node Visit(SignedNode node) => 
                node.Sign == Sign.Minus 
                ? node.Node
                : new SignedNode(Sign.Minus, node.Node);

            public Node Visit(FunctionNode node) => new SignedNode(Sign.Minus, node);
            public Node Visit(IntegerNode node) => new IntegerNode(node.Format, node.Value * -1);
            public Node Visit(BinaryOperatorNode node) => new SignedNode(Sign.Minus, node);
        }

        private class TransformNodeWithinMinusSignNodeVisitor : INodeVisitor<Node>
        {
            public Node Visit(FormulaNode node) => 
                throw new InvalidOperationException("Formula node is always the root node.");
            public Node Visit(BracketedNode node)
                => new BracketedNode(node.ChildNode.SwitchSignForAllSummands());
            public Node Visit(DecimalNode node) => new DecimalNode(node.Value * -1);
            public Node Visit(VariableNode node) => new SignedNode(Sign.Minus, node);
            public Node Visit(ConstantNode node) => new SignedNode(Sign.Minus, node);
            public Node Visit(PowerNode node) => new SignedNode(Sign.Minus, node);
            public Node Visit(SignedNode node) =>
                node.Sign == Sign.Minus
                ? node.Node
                : new SignedNode(Sign.Minus, node);

            public Node Visit(FunctionNode node) => new SignedNode(Sign.Minus, node);
            public Node Visit(IntegerNode node) => new IntegerNode(node.Format, node.Value * -1);
            public Node Visit(BinaryOperatorNode node) => new SignedNode(Sign.Minus, node);
        }

        public static bool EqualsNumber(this Node node, int number)
            => (node is DecimalNode rightDecNode && rightDecNode.Value == number) ||
               (node is IntegerNode rightIntNode && rightIntNode.Value == number);


        public static bool IsPlus(this BinaryOperatorNode node)
            => node.BinaryOperator.Symbol == "+";

        public static bool IsMinus(this BinaryOperatorNode node)
            => node.BinaryOperator.Symbol == "-";

        public static bool IsPlusOrMinus(this BinaryOperatorNode node)
            => node.IsPlus() || node.IsMinus();

        public static Node SwitchSign(this Node node)
            => node.Visit(new SwitchSignConcreteNodeVisitor());

        public static Node SwitchSignForAllSummands(this Node node)
            => node.Visit(new SwitchSignForTermsWithinBracketsVisitor());

        public static Node ResolveMinusSignedNodeContent(this Node node)
            => node.Visit(new TransformNodeWithinMinusSignNodeVisitor());
    }
}
