using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing.Ast.Nodes
{
    [DebuggerDisplay("({ChildNode})")]
    public class BracketedNode : Node
    {
        public BracketedNode(Node childNode)
        {
            this.ChildNode = childNode ?? throw new ArgumentNullException(nameof(childNode));
        }

        public Node ChildNode { get; }

        public override TReturn Visit<TReturn>(INodeVisitor<TReturn> visitor) => visitor.Visit(this);
    }
}
