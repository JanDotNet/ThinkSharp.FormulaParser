﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing.Ast.Nodes
{
    public class NumberNode : Node
    {
        public NumberNode(string token, double value)
        {
            this.Token = token;
            this.Value = value;
        }

        public string Token { get; }

        public double Value { get; }

        public override TReturn Visit<TReturn>(INodeVisitor<TReturn> visitor) => visitor.Visit(this);
    }
}