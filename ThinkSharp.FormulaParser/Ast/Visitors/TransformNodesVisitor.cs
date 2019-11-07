using System;
using System.Collections.Generic;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.Ast.Visitors
{
    public abstract class TransformNodesVisitor : INodeVisitor<Node>
    {
        public virtual Node Visit(FormulaNode node)
        {
            var child = node.ChildNode.Visit(this);

            if (child == node.ChildNode)
            {
                return node;
            }

            return new FormulaNode(child);
        }

        public virtual Node Visit(BracketedNode node)
        {
            var child = node.ChildNode.Visit(this);

            if (child == node.ChildNode)
            {
                return node;
            }

            return new BracketedNode(child);
        }

        public virtual Node Visit(DecimalNode node) => node;

        public virtual Node Visit(IntegerNode node) => node;

        public virtual Node Visit(VariableNode node) => node;

        public virtual Node Visit(ConstantNode node) => node;

        public virtual Node Visit(BinaryOperatorNode node)
        {
            var left = node.LeftNode.Visit(this);
            var right = node.RightNode.Visit(this);

            if (left == node.LeftNode && right == node.RightNode)
            {
                return node;
            }

            return new BinaryOperatorNode(node.BinaryOperator, left, right);
        }

        public virtual Node Visit(PowerNode node)
        {
            var baseNode = node.BaseNode.Visit(this);
            var exponentNode = node.ExponentNode.Visit(this);

            if (baseNode == node.BaseNode && exponentNode == node.ExponentNode)
            {
                return node;
            }

            return new PowerNode(baseNode, exponentNode);
        }

        public virtual Node Visit(SignedNode node)
        {
            var child = node.Node.Visit(this);

            if (child == node.Node)
            {
                return node;
            }

            return new SignedNode(node.Sign, child);
        }

        public virtual Node Visit(FunctionNode node)
        {
            var transformedParameters = new List<Node>();
            var hasChanged = false;

            foreach (var parameter in node.Parameters)
            {
                var transformedParameter = parameter.Visit(this);
                if (transformedParameter != parameter) hasChanged = true;
                transformedParameters.Add(transformedParameter);
            }

            if (!hasChanged)
            {
                return node;
            }

            return new FunctionNode(node.FunctionName, transformedParameters.ToArray());
        }
    }
}
