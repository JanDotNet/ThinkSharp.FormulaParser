using System;
using System.Collections.Generic;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.Ast.Visitors;
using ThinkSharp.FormulaParsing.TermRewriting.Rules;

namespace ThinkSharp.FormulaParsing.TermRewriting
{
    internal class ReplaceRuleTreeVisitor : INodeVisitor<Node>
    {
        private readonly Rule rule;

        public ReplaceRuleTreeVisitor(Rule rule)
        {
            this.rule = rule;
        }

        public Node Visit(FormulaNode node)
            => TryRewrite(node.ChildNode, out Node rewrittenNode)
                ? new FormulaNode(rewrittenNode)
                : node;

        public Node Visit(BracketedNode node)
            => TryRewrite(node.ChildNode, out Node rewrittenNode)
                ? new BracketedNode(rewrittenNode)
                : node;

        public Node Visit(NumberNode node)
            => node;

        public Node Visit(VariableNode node)
            => node;

        public Node Visit(ConstantNode node)
            => node;

        public Node Visit(BinaryOperatorNode node)
        {
            if (TryRewrite(node.LeftNode, out Node leftRewritten))
            {
                return new BinaryOperatorNode(node.BinaryOperator, leftRewritten, node.RightNode);
            }
            if (TryRewrite(node.RightNode, out Node rightRewritten))
            {
                return new BinaryOperatorNode(node.BinaryOperator, node.LeftNode, rightRewritten);
            }
            return node;
        }

        public Node Visit(PowerNode node)
        {
            if (TryRewrite(node.BaseNode, out Node rewrittenBaseNode))
            {
                return new PowerNode(rewrittenBaseNode, node.ExponentNode);
            }
            if (TryRewrite(node.BaseNode, out Node rewrittenExponentNode))
            {
                return new PowerNode(node.BaseNode, rewrittenExponentNode);
            }
            return node;
        }

        public Node Visit(SignedNode node)
            => TryRewrite(node.Node, out Node rewrittenNode)
                ? new SignedNode(node.Sign, rewrittenNode)
                : node;

        public Node Visit(FunctionNode node)
        {
            var rewrittenParameters = new List<Node>();
            var hasRewrittenParameters = false;
            foreach (var argumentNode in node.Parameters)
            {
                if (TryRewrite(argumentNode, out Node rewrittenNode))
                {
                    rewrittenParameters.Add(rewrittenNode);
                    hasRewrittenParameters = true;
                }
                else
                {
                    rewrittenParameters.Add(argumentNode);
                }
            }

            return hasRewrittenParameters
                ? new FunctionNode(node.FunctionName, rewrittenParameters.ToArray())
                : node;
        }

        private bool TryRewrite(Node node, out Node rewrittenNode)
        {
            if (this.rule.Match(node))
            {
                rewrittenNode = rule.Rewrite(node);
                return true;
            }

            rewrittenNode = node.Visit(this);
            return rewrittenNode != node;
        }
    }
}
