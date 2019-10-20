using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing.Test.TermRewriting
{
    internal class NodesToFormulaTextVisitor : INodeVisitor<string>
    {
        public string Visit(FormulaNode node)
        {
            return node.ChildNode.Visit(this);
        }

        public string Visit(BracketedNode node) => $"({node.ChildNode.Visit(this)})";

        public string Visit(NumberNode node) => node.Token;

        public string Visit(VariableNode node) => node.Name;

        public string Visit(ConstantNode node) => node.Name;

        public string Visit(BinaryOperatorNode node) => $"{node.LeftNode.Visit(this)}{node.BinaryOperator.Symbol}{node.RightNode.Visit(this)}";

        public string Visit(PowerNode node) => $"{node.BaseNode.Visit(this)}^{node.ExponentNode.Visit(this)}";

        public string Visit(SignedNode node) => (node.Sign == Sign.Minus ? "-" : "") + node.Node.Visit(this);

        public string Visit(FunctionNode node) => $"{node.FunctionName}({string.Join(", ", node.Parameters.Select(p => p.Visit(this)))})";
    }
}
