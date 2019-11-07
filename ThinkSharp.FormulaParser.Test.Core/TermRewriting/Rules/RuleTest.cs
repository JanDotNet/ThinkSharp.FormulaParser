using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ThinkSharp.FormulaParsing;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.TermRewriting;
using ThinkSharp.FormulaParsing.TermRewriting.Rules;
using ThinkSharp.FormulaParsing.Test;
using ThinkSharp.FormulaParsing.Utils;

namespace ThinkSharp.FluentFormulaParser.Test.TermRewriting.Rules
{
    public abstract class RuleTest
    {
        private static IFormulaParser parser = FormulaParser
            .CreateBuilder()
            .ConfigureValidationBehavior(v => v.DisableVariableNameValidation())
            .Build();

        protected abstract Rule CreateRule();

        protected void AssertNotMatching(string formula)
        {
            var node = parser.Parse(formula).Value;
            node = node is FormulaNode formulaNode
                ? formulaNode.ChildNode
                : node;

            AssertNotMatching(node);
        }

        protected void AssertNotMatching(Node node)
        {
            var rule = CreateRule();

            Assert.IsFalse(rule.Match(node));
        }

        protected void AssertRewritten(string expected, string formula)
        {
            var node = parser.Parse(formula).Value;
            var rule = CreateRule();

            var visitor = new ReplaceRuleTreeVisitor(rule);

            var simplifiedNode = node.Visit(visitor);

            Assert.IsTrue(simplifiedNode != node, "Rule matching fails.");
            
            var nodeToTExtVisitor = new NodesToFormulaTextVisitor();

            var actual = simplifiedNode.Visit(nodeToTExtVisitor);

            Assert.AreEqual(expected, actual, "Simplified formula is not equal to actual.");
        }

        protected void AssertRewritten(Node expected, Node input)
        {   
            var rule = CreateRule();

            var visitor = new ReplaceRuleTreeVisitor(rule);

            var simplifiedNode = input.Visit(visitor);

            Assert.IsTrue(simplifiedNode != input, "Rule matching fails.");

            var comparer = new NodeComparer();

            Assert.IsTrue(comparer.Equals(expected, simplifiedNode), "Expected and actual trees are not equal.");
        }
    }
}