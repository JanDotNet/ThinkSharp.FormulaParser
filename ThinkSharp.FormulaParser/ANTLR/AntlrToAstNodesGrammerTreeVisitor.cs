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

            return new FormulaNode(node, formulaText);
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
        
        public override Node VisitScientificNumber([NotNull] FormulaGrammerParser.ScientificNumberContext context)
        {
            if (this.configuration.IsScientificNotationSupportDisabled)
            {
                ParsingException.ThrowInvalidTokenException(context.SCIENTIFIC_NUMBER().Symbol);
            }

            var scientificNumber = context.SCIENTIFIC_NUMBER().Symbol.Text;
            return new NumberNode(double.Parse(scientificNumber));
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
