using System;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.ANTLR
{
    internal class AntlrToAstNodesGrammerTreeVisitor : FormulaGrammerBaseVisitor<Node>
    {
        private readonly IParserConfiguration configuration;
        private readonly string formulaText;

        public AntlrToAstNodesGrammerTreeVisitor(IParserConfiguration configuration, string formulaText)
        {
            this.configuration = configuration;
            this.formulaText = formulaText;
        }

        public override Node VisitFormula([NotNull] FormulaGrammerParser.FormulaContext context)
        {
            var node = base.Visit(context.expression());

            return new FormulaNode(node);
        }

        public override Node VisitExpression([NotNull] FormulaGrammerParser.ExpressionContext context)
        {
            int idxMultiplyExpression = 0;
            int idxOp = 1;

            var multiplyExpression = context.multiplyingExpression(idxMultiplyExpression++);
            var node = this.VisitMultiplyingExpression(multiplyExpression);
            
            while((multiplyExpression = context.multiplyingExpression(idxMultiplyExpression++)) != null)
            {
                var op = BinaryOperator.BySymbol(context.GetChild(idxOp).GetText());
                var right = this.VisitMultiplyingExpression(multiplyExpression);

                node = new BinaryOperatorNode(op, node, right);
                idxOp += 2;
            }

            return node;
        }

        public override Node VisitMultiplyingExpression([NotNull] FormulaGrammerParser.MultiplyingExpressionContext context)
        {
            int idxPowExpession = 0;
            int idxOp = 1;

            var powExpresion = context.powExpression(idxPowExpession++);
            var node = this.VisitPowExpression(powExpresion);

            while ((powExpresion = context.powExpression(idxPowExpession++)) != null)
            {
                var op = BinaryOperator.BySymbol(context.GetChild(idxOp).GetText());
                var right = this.VisitPowExpression(powExpresion);

                node = new BinaryOperatorNode(op, node, right);

                idxOp += 2;
            }

            return node;
        }

        public override Node VisitPowExpression([NotNull] FormulaGrammerParser.PowExpressionContext context)
        {
            int idxExponent = 0;

            if (this.configuration.IsPowSupportDisabled)
            {
                ParsingException.ThrowInvalidTokenException(context.POW(0).Symbol);
            }

            var signedAtom = context.signedAtom(idxExponent++);
            var node = this.Visit(signedAtom);            

            while ((signedAtom = context.signedAtom(idxExponent++)) != null)
            {
                var exponent = this.Visit(signedAtom);
                node = new PowerNode(node, exponent);
            }

            return node;
        }

        public override Node VisitPlusAtom([NotNull] FormulaGrammerParser.PlusAtomContext context)
        {
            return new SignedNode(Sign.Plus, this.Visit(context.atom()));
        }

        public override Node VisitNegativeAtom([NotNull] FormulaGrammerParser.NegativeAtomContext context)
        {
            return new SignedNode(Sign.Minus, this.Visit(context.atom()));
        }

        public override Node VisitAtom([NotNull] FormulaGrammerParser.AtomContext context)
        {
            var expression = context.expression();

            if (expression != null)
            {
                if (this.configuration.IsBracketSupportDisabled)
                {
                    ParsingException.ThrowInvalidTokenException(context.LPAREN().Symbol);
                }

                return new BracketedNode(this.VisitExpression(expression));
            }

            return base.VisitAtom(context);
        }
        
        //public override Node VisitScientificNumber([NotNull] FormulaGrammerParser.ScientificNumberContext context)
        //{
        //    //if (this.configuration.IsScientificNotationSupportDisabled)
        //    //{
        //    //    ParsingException.ThrowInvalidTokenException(context.E().Symbol);
        //    //}

        //    var scientificNumber = context.SCIENTIFIC_NUMBER().Symbol.Text;
        //    //var baseNumber = this.Visit();
        //    //var exponentNumber = this.Visit(context.number(1));
        //    //var isNegativ = context.MINUS() != null;

        //    //if (isNegativ)
        //    //{
        //    //    exponentNumber = new SignedNode(Sign.Minus, exponentNumber);
        //    //}

        //    //var pow = new PowerNode(new IntegerNode("10", 10), exponentNumber);
        //    //return new BinaryOperatorNode(BinaryOperator.BySymbol("*"), baseNumber, exponentNumber);
        //    return new DecimalNode(scientificNumber, double.Parse(scientificNumber));
        //}

        public override Node VisitPrefixedIntNumber([NotNull] FormulaGrammerParser.PrefixedIntNumberContext context)
        {
            var token = context.PREFIX_INT_NUMBER().Symbol.Text;
            return new IntegerNode(NumberFormat.Dec, long.Parse(token.Substring(2)));
        }

        public override Node VisitPrefixedBinNumber([NotNull] FormulaGrammerParser.PrefixedBinNumberContext context)
        {
            if (this.configuration.IsBinaryNumberNotationSupportDisabled)
            {
                ParsingException.ThrowInvalidTokenException(context.PREFIX_BIN_NUMBER().Symbol);
            }

            var token = context.PREFIX_BIN_NUMBER().Symbol.Text;
            return new IntegerNode(NumberFormat.Bin, Convert.ToInt64(token.Substring(2), 2));
        }

        public override Node VisitPrefixedDecNumber([NotNull] FormulaGrammerParser.PrefixedDecNumberContext context)
        {
            var token = context.PREFIX_DEC_NUMBER().Symbol.Text;
            return new DecimalNode(double.Parse(token.Substring(2)));
        }

        public override Node VisitPrefixedHexNumber([NotNull] FormulaGrammerParser.PrefixedHexNumberContext context)
        {
            if (this.configuration.IsHexadecimalNumberNotationSupportDisabled)
            {
                ParsingException.ThrowInvalidTokenException(context.PREFIX_HEX_NUMBER().Symbol);
            }

            var token = context.PREFIX_HEX_NUMBER().Symbol.Text;
            return new IntegerNode(NumberFormat.Hex, Convert.ToInt64(token.Substring(2), 16));
        }

        public override Node VisitPrefixedOctNumber([NotNull] FormulaGrammerParser.PrefixedOctNumberContext context)
        {
            if (this.configuration.IsOctalNumberNotationSupportDisabled)
            {
                ParsingException.ThrowInvalidTokenException(context.PREFIX_OCT_NUMBER().Symbol);
            }

            var token = context.PREFIX_OCT_NUMBER().Symbol.Text;
            return new IntegerNode(NumberFormat.Oct, Convert.ToInt64(token.Substring(2), 8));
        }

        public override Node VisitDecimalNumber([NotNull] FormulaGrammerParser.DecimalNumberContext context)
        {
            var token = context.DECIMAL_NUMBER().Symbol.Text;
            return new DecimalNode(double.Parse(token));
        }

        public override Node VisitIntgerNumber([NotNull] FormulaGrammerParser.IntgerNumberContext context)
        {
            var token = context.INTEGER_NUMBER().Symbol.Text;
            return new IntegerNode(NumberFormat.Dec, long.Parse(token));
        }

        public override Node VisitVariable([NotNull] FormulaGrammerParser.VariableContext context)
        {
            var variableNameToken = context.IDENTIFIER().Symbol;

            if (this.configuration.HasConstant(variableNameToken.Text))
            {
                return new ConstantNode(variableNameToken.Text);
            }

            if (this.configuration.IsVariablesSupportDisabled)
            {
                ParsingException.ThrowInvalidTokenException(variableNameToken);
            }

            var variableName = variableNameToken.Text;

            if (!this.configuration.IsVariableNameValidationDisabled && !this.configuration.HasVariable(variableName))
            {
                ParsingException.ThrowUnknownVariableException(variableNameToken);
            }

            return new VariableNode(variableName);
        }

        public override Node VisitFunc([NotNull] FormulaGrammerParser.FuncContext context)
        {
            var functionNameToken = context.IDENTIFIER().Symbol;
            if (this.configuration.IsFunctionsSupportDisabled)
            {
                ParsingException.ThrowInvalidTokenException(functionNameToken);
            }

            var functionName = functionNameToken.Text;

            var parameters = new List<Node>();
            var idxExp = 0;

            FormulaGrammerParser.ExpressionContext exp;
            while ((exp = context.expression(idxExp++)) != null)
            {
                parameters.Add(this.Visit(exp));
            }

            if (!this.configuration.IsFunctionNameValidationDisabled)
            {
                if (!this.configuration.HasFunction(functionName))
                {
                    ParsingException.ThrowUnknownFunctionException(functionNameToken);
                }

                if (!this.configuration.HasFunction(functionName, parameters.Count))
                {
                    ParsingException.ThrowFunctionArgumentCountDoesNotExistException(functionNameToken, parameters.Count);
                }
            }

            return new FunctionNode(functionName, parameters.ToArray());
        }
    }
}
