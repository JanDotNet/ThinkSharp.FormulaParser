using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing.Ast.Nodes
{
    public class FunctionNode : Node
    {
        internal FunctionNode(string functionName, params Node[] parameters)
        {
            this.FunctionName = functionName ?? throw new ArgumentNullException(nameof(functionName));
            this.Parameters = parameters ?? new Node[0];
        }

        public string FunctionName { get; }

        public IEnumerable<Node> Parameters { get; }

        public override TReturn Visit<TReturn>(INodeVisitor<TReturn> visitor) => visitor.Visit(this);
    }
}
