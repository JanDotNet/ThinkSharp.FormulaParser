using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.Ast.Visitors
{
    public class CloneTreeVisitor : NodeVisitor<Node>
    {
        public override Node Visit(BinaryOperatorNode node)
        {
            return new BinaryOperatorNode(
                node.BinaryOperator,
                node.LeftNode.Visit(this),
                node.RightNode.Visit(this));
        }

        public override Node Visit(BracketedNode node)
        {
            return new BracketedNode(node.ChildNode.Visit(this));
        }

        public override Node Visit(ConstantNode node)
        {
            return new ConstantNode(node.Name);
        }

        public override Node Visit(FormulaNode node)
        {
            return new FormulaNode(node.ChildNode.Visit(this));
        }

        public override Node Visit(FunctionNode node)
        {
            return new FunctionNode(node.FunctionName, node.Parameters.Select(p => p.Visit(this)).ToArray());
        }

        public override Node Visit(NumberNode node)
        {
            return new NumberNode(node.Token, node.Value);
        }

        public override Node Visit(PowerNode node)
        {
            return new PowerNode(node.BaseNode.Visit(this), node.ExponentNode.Visit(this));
        }

        public override Node Visit(SignedNode node)
        {
            return new SignedNode(node.Sign, node.Node.Visit(this));
        }

        public override Node Visit(VariableNode node)
        {
            return new VariableNode(node.Name);
        }
    }
}
