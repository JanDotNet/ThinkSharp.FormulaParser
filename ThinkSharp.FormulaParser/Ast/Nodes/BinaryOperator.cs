using System;
using System.Collections.Generic;
using System.Linq;

namespace ThinkSharp.FormulaParsing.Ast.Nodes
{
    public sealed class BinaryOperator
    {
        readonly Func<double, double, double> evaluation;
        private static readonly IEnumerable<BinaryOperator> allBinaryOperators = new[]
        {
            new BinaryOperator("+", (x, y) => x + y),
            new BinaryOperator( "-", (x, y) => x - y),
            new BinaryOperator( "/", (x, y) => x / y),
            new BinaryOperator("*", (x, y) => x* y),
        };

        private BinaryOperator(string symbol, Func<double, double, double> evaluation)
        {
            this.Symbol = symbol;
            this.evaluation = evaluation;
        }

        public string Symbol { get; }

        public static BinaryOperator BySymbol(string symbol) 
            => allBinaryOperators.FirstOrDefault(o => o.Symbol == symbol) ?? throw new InvalidOperationException($"Unknonw binary operator symbol '{0}'.");

        public double Evaluate(double left, double right) => this.evaluation(left, right);
    }
}
