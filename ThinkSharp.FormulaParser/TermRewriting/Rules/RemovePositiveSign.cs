using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.TermRewriting.Rules
{
    public class RemovePositiveSign : Rule
    {
        protected override bool MatchInternal(Node node, Node Parent)
        {
            return node is SignedNode signedNode && signedNode.Sign == Sign.Plus;
        }

        protected override Node RewriteInternal(Node node, Node Parent)
        {
            if (node is SignedNode signedNode && signedNode.Sign == Sign.Plus)
            {
                return signedNode.Node;
            }

            Debug.Fail("Should not happen");
            return node;
        }
    }
}
