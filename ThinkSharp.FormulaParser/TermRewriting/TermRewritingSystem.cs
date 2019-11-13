using System;
using System.Collections.Generic;
using System.Linq;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.TermRewriting.Rules;

namespace ThinkSharp.FormulaParsing.TermRewriting
{
    public class TermRewritingSystem : ITermRewritingSystem
    {
        public static ITermRewritingSystem Create()
        {
            var system = new TermRewritingSystem();
            system.rules.Add(new EvaluateIntegers());
            system.rules.Add(new RemoveBracketsInTermPlusMinusBrackets());
            system.rules.Add(new RemoveBracketsInTermPlusMinusBracketsPlusMinusTerm());
            system.rules.Add(new RemoveNegativeSignFromNumber());
            system.rules.Add(new RemoveNegativeSignInFrontOfBrackets());
            system.rules.Add(new RemoveNegativeSignInInSubtract());
            system.rules.Add(new RemoveNegativeSignInInSum());
            system.rules.Add(new RemoveNegativeSignTimesNegatieSign());
            system.rules.Add(new RemoveNestedBrackets());
            system.rules.Add(new RemovePositiveSign());
            system.rules.Add(new RemoveRootBrackets());
            system.rules.Add(new TermDividesByTermIsOne());
            system.rules.Add(new TermPlusOrMinusZeroIsTermRule());
            system.rules.Add(new TermTimesOrDividedByMinusOneIsMinusTerm());
            system.rules.Add(new TermTimesOrDividedByOneIsTerm());
            system.rules.Add(new RearrangeIntegersInSum());
            return system;
        }

        public static ITermRewritingSystem ForTesting(params Rule[] rulesToTest)
        {
            var system = new TermRewritingSystem();
            foreach (var rule in rulesToTest)
                system.rules.Add(rule);
            return system;
        }

        private List<Rule> rules = new List<Rule>();
        public Node Simplify(Node node) => SimplyfyInternal(node);

        private Node SimplyfyInternal(Node node)
        {

            var visitors = rules.Select(r => new ReplaceRuleTreeVisitor(r)).ToArray();

            Node simlifiedNode = node;
            Node previousSimlifiedNode = node;
            do 
            {
                previousSimlifiedNode = simlifiedNode;
                foreach (var visitor in visitors)
                    simlifiedNode = simlifiedNode.Visit(visitor);
            } while (simlifiedNode != previousSimlifiedNode);

            return simlifiedNode;
        }

        //private Node CollapseIdenticalNodes(Node node, IDictionary<string, Node> collapsedNodes)
        //{

        //}

        //private Node ExpandCollapsedNodes(Node node, IDictionary<string, Node> collapsedNodes)
        //{

        //}
    }
}
