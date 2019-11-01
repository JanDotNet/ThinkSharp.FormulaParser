using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.Ast.Visitors;
using ThinkSharp.FormulaParsing.TermRewriting.Rules;

namespace ThinkSharp.FormulaParsing.TermRewriting
{
    public class ReplaceRuleTreeVisitor : INodeVisitor<Node>
    {
        private readonly Rule rule;
        private readonly ParentNodeContex parentContext = new ParentNodeContex();

        private class ParentNodeContex : IDisposable
        {
            private Stack<Node> stackedNodes = new Stack<Node>();

            public Node Parent => stackedNodes.Count == 0 ? null : stackedNodes.Peek();

            public IDisposable Push(Node parent)
            {
                stackedNodes.Push(parent);
                return this;
            }

            void IDisposable.Dispose() => stackedNodes.Pop();
        }

        public ReplaceRuleTreeVisitor(Rule rule)
        {
            this.rule = rule;
        }

        public Node Visit(FormulaNode node)
        {
            using (parentContext.Push(node))
            {
                var child = node.ChildNode.Visit(this);
                return child == node.ChildNode ? node : new FormulaNode(child);
            }
        }   

        public Node Visit(BracketedNode node)
        {
            using (parentContext.Push(node))
            {
                var child = node.ChildNode.Visit(this);
                node = node.ChildNode == child ? node : new BracketedNode(child);
            }

            return TryRewrite(node);
        }

        public Node Visit(DecimalNode node) => TryRewrite(node);

        public Node Visit(IntegerNode node) => TryRewrite(node);

        public Node Visit(VariableNode node) => TryRewrite(node);

        public Node Visit(ConstantNode node) => TryRewrite(node);

        public Node Visit(BinaryOperatorNode node)
        {
            using (parentContext.Push(node))
            {
                var left = node.LeftNode.Visit(this);
                var right = node.RightNode.Visit(this);
                node = left == node.LeftNode && right == node.RightNode 
                    ? node 
                    : new BinaryOperatorNode(node.BinaryOperator, left, right);
            }
            return TryRewrite(node);
        }

        public Node Visit(PowerNode node)
        {
            using (parentContext.Push(node))
            {
                var baseNode = node.BaseNode.Visit(this);
                var exponentNode = node.ExponentNode.Visit(this);
                node = baseNode == node.BaseNode && exponentNode == node.ExponentNode 
                    ? node
                    : new PowerNode(baseNode, exponentNode);
            }
            return TryRewrite(node);            
        }

        public Node Visit(SignedNode node)
        {
            
            using (parentContext.Push(node))
            {
                var child = node.Node.Visit(this);
                node = child == node.Node ? node : new SignedNode(node.Sign, child);
            }
            return TryRewrite(node);
        }

        public Node Visit(FunctionNode node)
        {
            using (parentContext.Push(node))
            {
                var rewrittenParameters = node.Parameters.Select(p => p.Visit(this)).ToArray();
                node = rewrittenParameters.SequenceEqual(node.Parameters)
                    ? node
                    : new FunctionNode(node.FunctionName, rewrittenParameters);
            }
            
            return TryRewrite(node);
        }

        private Node TryRewrite(Node node)
        {
            if (this.rule.Match(node, this.parentContext.Parent))
            {
                return rule.Rewrite(node, this.parentContext.Parent);
            }

            return node;
        }
    }
}
