using System;
using System.Collections.Generic;
using System.Linq;

namespace ThinkSharp.FormulaParsing.Ast.Nodes
{
    public sealed class BinaryOperator
    {
        readonly Func<double, double, double> evaluation;
        private static readonly BinaryOperator[] allBinaryOperators;
        
        public static BinaryOperator Plus { get; }
        public static BinaryOperator Minus { get; }
        public static BinaryOperator Times { get; }
        public static BinaryOperator DividedBy { get; }

        static BinaryOperator()
        {
            Plus = new BinaryOperator("+", (x, y) => x + y);
            Minus = new BinaryOperator("-", (x, y) => x - y);
            Times = new BinaryOperator("*", (x, y) => x * y);
            DividedBy = new BinaryOperator("/", (x, y) => x / y);

            allBinaryOperators = new[]
            {
                Plus,
                Minus,
                Times,
                DividedBy
            };
        }

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
