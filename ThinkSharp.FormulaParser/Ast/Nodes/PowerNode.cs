﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing.Ast.Nodes
{
    [DebuggerDisplay("{BaseNode}^{ExponentNode}")]
    public class PowerNode : Node
    {
        public PowerNode(Node baseNode, Node exponentNode)
        {
            this.BaseNode = baseNode ?? throw new ArgumentNullException(nameof(baseNode));
            this.ExponentNode = exponentNode ?? throw new ArgumentNullException(nameof(exponentNode));
        }

        public Node BaseNode { get; }
        public Node ExponentNode { get; }

        public override TReturn Visit<TReturn>(INodeVisitor<TReturn> visitor) => visitor.Visit(this);
    }
}
