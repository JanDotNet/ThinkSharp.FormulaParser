using System;
using System.Linq;
using System.Collections.Generic;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.Ast.Visitors
{
    internal class EvaluateAstVisitor : NodeVisitor<double>
    {
        private readonly IDictionary<string, double> variables;
        private readonly IConfigurationEvaluator configuration;

        public EvaluateAstVisitor(IConfigurationEvaluator configurationEvaluation, IDictionary<string, double> variables = null)
        {
            this.variables = new Dictionary<string, double>(variables ?? new Dictionary<string, double>());
            this.configuration = configurationEvaluation ?? throw new ArgumentNullException(nameof(configurationEvaluation));

            foreach (var constant in configurationEvaluation.EnumerateConstantes())
            {
                if (this.variables.ContainsKey(constant.Key))
                {
                    throw EvaluatingException.VariableNameConflictsWithExistingConstantName(constant.Key);
                }

                this.variables.Add(constant);
            }
        }

        public override double Visit(NumberNode node)
        {
            return node.Value;
        }

        public override double Visit(VariableNode node)
        {
            if (!this.variables.TryGetValue(node.Name, out var value))
            {
                throw EvaluatingException.VariableDoesNotExist(node.Name);
            }

            return value;
        }

        public override double Visit(ConstantNode node)
        {
            var constant = this.configuration.EnumerateConstantes().FirstOrDefault(x => x.Key == node.Name);
            if (constant.Key == null)
            {
                throw EvaluatingException.ConstantDoesNotExist(node.Name);
            }

            return constant.Value;
        }

        public override double Visit(BinaryOperatorNode node)
        {
            var leftValue = node.LeftNode.Visit(this);
            var rightValue = node.RightNode.Visit(this);
            return node.BinaryOperator.Evaluate(leftValue, rightValue);
        }

        public override double Visit(PowerNode node)
        {
            var baseNumber = node.BaseNode.Visit(this);
            var exponent = node.ExponentNode.Visit(this);

            return Math.Pow(baseNumber, exponent);
        }

        public override double Visit(SignedNode node)
        {
            var value = node.Node.Visit(this);

            return node.Sign == Sign.Minus ? -value : value;
        }

        public override double Visit(FunctionNode node)
        {
            var parameters = node.Parameters.Select(n => n.Visit(this)).ToArray();

            return this.configuration.EvaluateFunction(node.FunctionName, parameters);
        }
    }
}
