using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing.Utils
{
    public class NodeHashCodeVisitor : INodeVisitor<int>
    {
        private static int formulaHash = "formula".GetHashCode();
        private static int bracketsHash = "()".GetHashCode();
        private static int powHash = "pow".GetHashCode();
        public int Visit(FormulaNode node) => formulaHash ^ node.ChildNode.Visit(this);

        public int Visit(BracketedNode node) => bracketsHash.GetHashCode() ^ node.ChildNode.Visit(this);

        public int Visit(VariableNode node) => node.Name.GetHashCode();

        public int Visit(ConstantNode node) => node.Name.GetHashCode();

        public int Visit(BinaryOperatorNode node) 
            => node.BinaryOperator.Symbol.GetHashCode() ^ node.LeftNode.Visit(this) ^ node.RightNode.Visit(this);
        public int Visit(PowerNode node) 
            => powHash.GetHashCode() ^ node.BaseNode.Visit(this) ^ node.ExponentNode.Visit(this);

        public int Visit(SignedNode node) 
            => node.Sign.GetHashCode() ^ node.Node.Visit(this);

        public int Visit(FunctionNode node) 
            => node.Parameters.Select(n => n.Visit(this)).Aggregate(node.FunctionName.GetHashCode(), (x, y) => x ^ y);

        public int Visit(DecimalNode node) 
            => node.Value.GetHashCode();

        public int Visit(IntegerNode node) 
            => node.Value.GetHashCode();
    }
}
