using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.TermRewriting.Rules
{
    public class RemoveNegativeSignFromNumber : Rule
    {
        protected override bool MatchInternal(Node node, Node Parent)
        {
            return node is SignedNode signedNode 
                && signedNode.Sign == Sign.Minus
                && (signedNode.Node is DecimalNode || signedNode.Node is IntegerNode);
        }

        protected override Node RewriteInternal(Node node, Node Parent)
        {
            if (node is SignedNode signedNode && signedNode.Sign == Sign.Minus)
            {
                if (signedNode.Node is DecimalNode dnode)
                {
                    return new DecimalNode(dnode.Value * -1);
                }
                if (signedNode.Node is IntegerNode inode)
                {
                    return new IntegerNode(inode.Format, inode.Value * -1);
                }
            }

            Debug.Fail("Should not happen");
            return node;
        }
    }
}
