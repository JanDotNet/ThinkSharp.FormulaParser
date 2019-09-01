using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing.Ast.Nodes
{
    public class BinaryOperatorNode : Node
    {
        public BinaryOperatorNode(BinaryOperator binaryOperator, Node leftNode, Node rightNode)
        {
            this.BinaryOperator = binaryOperator ?? throw new ArgumentNullException(nameof(binaryOperator));
            this.LeftNode = leftNode ?? throw new ArgumentNullException(nameof(leftNode));
            this.RightNode = rightNode ?? throw new ArgumentNullException(nameof(rightNode));
        }

        public BinaryOperator BinaryOperator { get; }

        public Node LeftNode { get; }

        public Node RightNode { get; }

        public override TReturn Visit<TReturn>(INodeVisitor<TReturn> visitor) => visitor.Visit(this);
    }
}
