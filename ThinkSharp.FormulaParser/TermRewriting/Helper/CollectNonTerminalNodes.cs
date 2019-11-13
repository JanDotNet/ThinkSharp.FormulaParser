using System;
using System.Collections.Generic;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing.TermRewriting.Helper
{
    public class CollectNonTerminalNodes : INodeVisitor<IEnumerable<Node>>
    {
        public IEnumerable<Node> Visit(FormulaNode node)
        {
            return node.ChildNode.Visit(this);
        }

        public IEnumerable<Node> Visit(BracketedNode node)
        {
            yield return node;
            foreach (var childNode in node.ChildNode.Visit(this))
                yield return node;
        }

        public IEnumerable<Node> Visit(DecimalNode node)
        {
            yield break;
        }

        public IEnumerable<Node> Visit(IntegerNode node)
        {
            yield break;
        }

        public IEnumerable<Node> Visit(VariableNode node)
        {
            yield break;
        }

        public IEnumerable<Node> Visit(ConstantNode node)
        {
            yield break;
        }

        public IEnumerable<Node> Visit(BinaryOperatorNode node)
        {
            yield return node;
            foreach (var childNode in node.LeftNode.Visit(this))
                yield return node;
            foreach (var childNode in node.RightNode.Visit(this))
                yield return node;
        }

        public IEnumerable<Node> Visit(PowerNode node)
        {
            yield return node;
            foreach (var childNode in node.BaseNode.Visit(this))
                yield return node;
            foreach (var childNode in node.ExponentNode.Visit(this))
                yield return node;
        }

        public IEnumerable<Node> Visit(SignedNode node)
        {
            yield return node;
            foreach (var childNode in node.Node.Visit(this))
                yield return node;
        }

        public IEnumerable<Node> Visit(FunctionNode node)
        {
            yield return node;
            foreach (var parameter in node.Parameters)
                foreach (var childNode in parameter.Visit(this))
                    yield return node;
        }
    }
}
