using System;
using System.Collections.Generic;
using System.Diagnostics;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing.Ast.Nodes
{
    [DebuggerDisplay("{Name}()")]
    public class FunctionNode : Node
    {
        public FunctionNode(string functionName, params Node[] parameters)
        {
            this.FunctionName = functionName ?? throw new ArgumentNullException(nameof(functionName));
            this.Parameters = parameters ?? new Node[0];
        }

        public string FunctionName { get; }

        public IEnumerable<Node> Parameters { get; }

        public override TReturn Visit<TReturn>(INodeVisitor<TReturn> visitor) => visitor.Visit(this);
    }
}
