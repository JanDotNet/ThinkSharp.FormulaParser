using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.TermRewriting.Rules
{
    public class RemoveRootBrackets : Rule
    {
        protected override bool MatchInternal(Node node, Node parent)
        {
            return (parent == null || parent is FormulaNode) && node is BracketedNode;
        }

        protected override Node RewriteInternal(Node node, Node parent)
        {
            if ((parent == null || parent is FormulaNode) && node is BracketedNode bracketNode)
            {
                return bracketNode.ChildNode;
            }

            Debug.Fail("Should not happen");
            return node;
        }
    }
}
