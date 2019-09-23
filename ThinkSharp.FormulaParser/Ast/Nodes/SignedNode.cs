using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing.Ast.Nodes
{
    public enum Sign { Plus, Minus }

    public class SignedNode : Node
    {
        public SignedNode(Sign sign, Node node)
        {
            this.Sign = sign;
            this.Node = node ?? throw new ArgumentNullException(nameof(node));
        }

        public Sign Sign { get; }

        public Node Node { get; }

        public override TReturn Visit<TReturn>(INodeVisitor<TReturn> visitor) => visitor.Visit(this);
    }
}
