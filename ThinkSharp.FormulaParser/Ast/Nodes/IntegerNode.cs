using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing.Ast.Nodes
{
    [DebuggerDisplay("{Value}")]
    public class IntegerNode : Node
    {
        public IntegerNode(long value) : this(NumberFormat.Dec, value)
        {
        }
        public IntegerNode(NumberFormat format, long value)
        {
            this.Format = format;
            this.Value = value;
        }

        public NumberFormat Format { get; }

        public long Value { get; }

        public override TReturn Visit<TReturn>(INodeVisitor<TReturn> visitor) => visitor.Visit(this);
    }
}
