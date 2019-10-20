using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.TermRewriting.Rules;

namespace ThinkSharp.FormulaParsing.TermRewriting
{
    public class TermRewritingSystem : ITermRewritingSystem
    {
        public static ITermRewritingSystem Create()
        {
            var system = new TermRewritingSystem();
            system.rules.Add(new TermPlusOrMinusZeroIsTermRule());
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
    }
}
