using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing.TermRewriting.Rules
{
    public abstract class Rule
    {
        public bool Match(Node node, Node parent = null) => this.MatchInternal(node, parent);

        public Node Rewrite(Node node, Node parent = null)
        {
            var rewrittenNode = this.RewriteInternal(node, parent);

            var nodeToText = new NodesToTextDebugVisitor();

            var from = node.Visit(nodeToText);
            var to = rewrittenNode.Visit(nodeToText);

            Debug.WriteLine($"Rewrite: '{from}' to '{to}' Type: (${this.GetType()})");

            return rewrittenNode;
        }

        protected abstract bool MatchInternal(Node node, Node parent);

        protected abstract Node RewriteInternal(Node node, Node parent);
    }
}
