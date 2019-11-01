using System;
using System.Collections.Generic;
using System.Diagnostics;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.TermRewriting.Rules
{
    public class RemoveNegativeSignInFrontOfBrackets : Rule
    {
        protected override bool MatchInternal(Node node, Node parent)
        {
            if (node is SignedNode signNode 
                && signNode.Sign == Sign.Minus
                && signNode.Node is BracketedNode bracketedNode)
            {
                return true;
            }

            return false;
        }

        protected override Node RewriteInternal(Node node, Node parent)
        {
            if (node is SignedNode signNode && signNode.Sign == Sign.Minus && signNode.Node is BracketedNode bracketedNode)
            {
                var child = this.Process(bracketedNode.ChildNode, true);
                return new BracketedNode(child);
            }

            Debug.Fail("Should not happen");
            return node;
        }

        private Node Process(Node node, bool isLeftBranch)
        {
            if (node is BinaryOperatorNode binOp)
            {
                var left = Process(binOp.LeftNode, isLeftBranch);
                var right = Process(binOp.RightNode, false);

                var symbol = binOp.BinaryOperator.Symbol;
                var op = symbol == "+" ? BinaryOperator.BySymbol("-")
                       : symbol == "-" ? BinaryOperator.BySymbol("+")
                       : binOp.BinaryOperator;

                return new BinaryOperatorNode(op, left, right);
            }

            return isLeftBranch ? new SignedNode(Sign.Minus, node) : node;
        }
    }
}
