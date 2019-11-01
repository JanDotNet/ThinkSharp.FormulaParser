using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.TermRewriting.Rules
{
    public class RemoveNegativeSignTimesNegatieSign : Rule
    {
        protected override bool MatchInternal(Node node, Node Parent)
        {
            if (node is SignedNode signedNode
                && signedNode.Sign == Sign.Minus)
            {
                var innerNode = signedNode.Node;

                while (true)
                {
                    if (innerNode is SignedNode innersignedNode 
                        && innersignedNode.Sign == Sign.Minus)
                    {
                        return true;
                    }

                    if (innerNode is BinaryOperatorNode binNode)
                    {
                        innerNode = binNode.LeftNode;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return false;
        }

        protected override Node RewriteInternal(Node node, Node Parent)
        {
            if (node is SignedNode signedNode
                && signedNode.Sign == Sign.Minus)
            {
                var innerNode = signedNode.Node;
                var binNodes = new Stack<BinaryOperatorNode>();
                while (true)
                {
                    if (innerNode is SignedNode innersignedNode
                        && innersignedNode.Sign == Sign.Minus)
                    {
                        var rewrittenNode = innersignedNode.Node;
                        while (binNodes.Count > 0)
                        {
                            var current = binNodes.Pop();
                            rewrittenNode = new BinaryOperatorNode(current.BinaryOperator, rewrittenNode, current.RightNode);
                        }

                        return rewrittenNode;
                    }

                    if (innerNode is BinaryOperatorNode binNode)
                    {
                        binNodes.Push(binNode);
                        innerNode = binNode.LeftNode;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            Debug.Fail("Should not happen");
            return node;
        }
    }
}
