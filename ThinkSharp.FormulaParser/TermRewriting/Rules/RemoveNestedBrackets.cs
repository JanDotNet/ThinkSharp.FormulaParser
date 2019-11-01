using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.TermRewriting.Rules
{
    public class RemoveNestedBrackets : Rule
    {
        protected override bool MatchInternal(Node node, Node Parent)
        {
            return node is BracketedNode outerBrackets
                && outerBrackets.ChildNode is BracketedNode;
        }

        protected override Node RewriteInternal(Node node, Node Parent)
        {
            if (node is BracketedNode outerNode && 
                outerNode.ChildNode is BracketedNode innerNode)
            {
                return innerNode;
            }

            Debug.Fail("Should not happen");
            return node;
        }
    }
}
