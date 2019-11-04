using System;
using System.Collections.Generic;
using System.Linq;

namespace ThinkSharp.FormulaParsing.Ast.Nodes
{
    public sealed class BinaryOperator
    {
        private readonly Func<double, double, double> evaluationDouble;
        private readonly Func<long, long, long> evaluationInt;
        private static readonly BinaryOperator[] allBinaryOperators;
        
        public static BinaryOperator Plus { get; }
        public static BinaryOperator Minus { get; }
        public static BinaryOperator Multiply { get; }
        public static BinaryOperator DividedBy { get; }

        static BinaryOperator()
        {
            Plus = new BinaryOperator("+", (x, y) => x + y, (x, y) => x + y);
            Minus = new BinaryOperator("-", (x, y) => x - y, (x, y) => x - y);
            Multiply = new BinaryOperator("*", (x, y) => x * y, (x, y) => x * y);
            DividedBy = new BinaryOperator("/", (x, y) => x / y, (x, y) => x / y);

            allBinaryOperators = new[]
            {
                Plus,
                Minus,
                Multiply,
                DividedBy
            };
        }

        private BinaryOperator(string symbol, 
            Func<double, double, double> evaluateDouble,
            Func<long, long, long> evaluateInt)
        {
            this.Symbol = symbol;
            this.evaluationDouble = evaluateDouble;
            this.evaluationInt = evaluateInt;
;
        }

        public string Symbol { get; }

        public static BinaryOperator BySymbol(string symbol) 
            => allBinaryOperators.FirstOrDefault(o => o.Symbol == symbol) ?? throw new InvalidOperationException($"Unknonw binary operator symbol '{0}'.");

        public double Evaluate(double left, double right) => this.evaluationDouble(left, right);

        public long Evaluate(long left, long right) => this.evaluationInt(left, right);
    }
}
