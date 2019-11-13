using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.TermRewriting;
using ThinkSharp.FormulaParsing.TermRewriting.Rules;

namespace ThinkSharp.FormulaParsing.Utils
{
    public class NodeComparer : IEqualityComparer<Node>
    {
        private static NodeHashCodeVisitor hashCodeVisitor = new NodeHashCodeVisitor();
        private static ITermRewritingSystem termRewriteSystem = TermRewritingSystem.Create();

        public bool Equals(Node x, Node y)
        {
            if (x.GetType() != y.GetType())
            {
                return false;
            }            
            else if (x is DecimalNode d1 && y is DecimalNode d2)
            {
                return d1.Value == d2.Value;
            }
            else if (x is IntegerNode i1 && y is IntegerNode i2)
            {
                return i1.Value == i2.Value;
            }
            else if (x is VariableNode v1 && y is VariableNode v2)
            {
                return v1.Name == v2.Name;
            }
            else if (x is ConstantNode c1 && y is ConstantNode c2)
            {
                return c1.Name == c2.Name;
            }
            else if (x is PowerNode p1 && y is PowerNode p2)
            {
                return Equals(p1.BaseNode, p2.BaseNode)
                    && Equals(p1.ExponentNode, p2.ExponentNode);
            }
            else if (x is SignedNode s1 && y is SignedNode s2)
            {
                return s1.Sign == s2.Sign
                    && Equals(s1.Node, s2.Node);
            }
            else if (x is FormulaNode f1 && y is FormulaNode f2)
            {
                return Equals(f1.ChildNode, f2.ChildNode);
            }
            else if (x is FunctionNode fun1 && y is FunctionNode fun2)
            {
                if (fun1.FunctionName != fun2.FunctionName)
                {
                    return false;
                }

                var fun1Args = fun1.Parameters.ToArray();
                var fun2Args = fun2.Parameters.ToArray();

                if (fun1Args.Length != fun2Args.Length)
                {
                    return false;
                }

                for (int i = 0; i < fun1Args.Length; i++)
                {
                    if (!Equals(fun1Args[i], fun2Args[i]))
                    {
                        return false;
                    }

                    return true;
                }
            }
            else if (x is BracketedNode b1 && y is BracketedNode b2)
            {
                return Equals(b1.ChildNode, b2.ChildNode);
            }
            else if (x is BinaryOperatorNode bo1 && y is BinaryOperatorNode bo2)
            {
                switch (bo1.BinaryOperator.Symbol)
                {
                    case "/":
                        if (bo1.BinaryOperator != bo2.BinaryOperator)
                            return false;
                        return Equals(bo1.LeftNode, bo2.LeftNode) && Equals(bo1.RightNode, bo2.RightNode);
                    case "*":
                        if (bo1.BinaryOperator != bo2.BinaryOperator)
                            return false;
                        var checkLLRR = Equals(bo1.LeftNode, bo2.LeftNode) && Equals(bo1.RightNode, bo2.RightNode);
                        var checkLRRL = Equals(bo1.LeftNode, bo2.RightNode) && Equals(bo1.RightNode, bo2.LeftNode);
                        return checkLLRR || checkLRRL;
                    case "-":
                    case "+":
                        var isMinus = bo1.BinaryOperator.Symbol == "-";

                        var summands1Left = GetSummands(bo1.LeftNode);
                        var summands1Right = GetSummands(bo1.RightNode);
                        if (isMinus) summands1Right = summands1Right.WrapFirst(n => n.SwitchSign());
                        var summands1 = summands1Left.Concat(summands1Right).Select(termRewriteSystem.Simplify).ToList();

                        isMinus = bo2.BinaryOperator.Symbol == "-";
                        var summands2Left = GetSummands(bo2.LeftNode);
                        var summands2Right = GetSummands(bo2.RightNode);
                        if (isMinus) summands2Right = summands2Right.WrapFirst(n => n.SwitchSign());
                        var summands2 = summands2Left.Concat(summands2Right).Select(termRewriteSystem.Simplify).ToList();

                        if (summands1.Count != summands2.Count)
                        {
                            return false;
                        }

                        for (int i = 0; i < summands1.Count; i++)
                        {
                            var summand1 = summands1[i];
                            var found = false;
                            for (int j = 0; j < summands2.Count; j++)
                            {
                                var summand2 = summands2[j];
                                if (Equals(summand1, summand2))
                                {
                                    found = true;
                                    summands2.Remove(summand2);
                                    break;
                                }
                            }
                            if (!found)
                            {
                                return false;
                            }
                        }
                        return true;

                    default:
                        throw new InvalidOperationException($"Unknown binary operator: {bo1.BinaryOperator.Symbol}.");
                }
            }

            Debug.Fail($"Node types not handled for comparison: {x.GetType()}");
            return false;
        }

        private IEnumerable<Node> GetSummands(Node node)
        {
            if (node is BinaryOperatorNode bonPlus && bonPlus.BinaryOperator.Symbol == "+")
            {
                foreach (var subSummand in GetSummands(bonPlus.LeftNode))
                    yield return subSummand;
                foreach (var subSummand in GetSummands(bonPlus.RightNode))
                    yield return subSummand;
            }
            else if (node is BinaryOperatorNode bonMinus && bonMinus.BinaryOperator.Symbol == "-")
            {
                foreach (var subSummand in GetSummands(bonMinus.LeftNode))
                    yield return subSummand;                
                foreach (var subSummand in GetSummands(bonMinus.RightNode).WrapFirst(n => new SignedNode(Sign.Minus, n)))
                    yield return subSummand;
            }
            else
            {
                yield return node;
            }
        }

        public int GetHashCode(Node node) => node.Visit(hashCodeVisitor);
    }
}
