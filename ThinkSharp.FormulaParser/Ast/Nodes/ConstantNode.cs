﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing.Ast.Nodes
{
    public class ConstantNode : Node
    {
        internal ConstantNode(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        public override TReturn Visit<TReturn>(INodeVisitor<TReturn> visitor) => visitor.Visit(this);
    }
}