using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using ThinkSharp.FormulaParsing.ANTLR;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing
{
    /// <summary>
    /// The <see cref="FormulaParser"/> class provides static methods for creating <see cref="IFormulaParser"/> instances.
    /// Use the method <see cref="Create"/> to create a <see cref="IFormulaParser"/> with default configuration.
    /// Use the method <see cref="CreateBuilder"/> to create a <see cref="IFormulaParserBuilder"/> that allows to configure the <see cref="IFormulaParser"/> instance.
    /// </summary>
    public class FormulaParser : IFormulaParser
    {
        private readonly Configuration configuration;

        private FormulaParser() : this(new Configuration())
        { }

        internal FormulaParser(Configuration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Creates a new <see cref="IFormulaParser"/> with its default configuration.
        /// </summary>
        /// <returns>
        /// A new <see cref="IFormulaParser"/> with its default configuration.
        /// </returns>
        public static IFormulaParser Create() => new FormulaParser();

        /// <summary>
        /// Creates a <see cref="IFormulaParserBuilder"/> that allows to configure the <see cref="IFormulaParser"/>.
        /// </summary>
        /// <returns>
        /// a <see cref="IFormulaParserBuilder"/>.
        /// </returns>
        public static IFormulaParserBuilder CreateBuilder() => new FormulaParserBuilder();

        public FormulaParserResult<double> Evaluate(string formula)
        {
            return this.Evaluate(formula, null);
        }

        public FormulaParserResult<double> Evaluate(string formula, IReadOnlyDictionary<string, double> variables)
        {
            return WrapWithExceptionHandling(() =>
            {
                var formulaNode = this.ParseFormula(formula, variables);
                var evalAstVisitor = new EvaluateAstVisitor(this.configuration, variables);
                var result = formulaNode.Visit(evalAstVisitor);

                return new FormulaParserResult<double>(result);
            });
        }

        public FormulaParserResult<double> Evaluate(Node formulaNode)
        {
            return this.Evaluate(formulaNode, null);
        }

        public FormulaParserResult<double> Evaluate(Node formulaNode, IReadOnlyDictionary<string, double> variables)
        {
            return WrapWithExceptionHandling(() =>
            {
                var evalAstVisitor = new EvaluateAstVisitor(this.configuration, variables);
                var result = formulaNode.Visit(evalAstVisitor);

                return new FormulaParserResult<double>(result);
            });
        }

        public FormulaParserResult<Node> Parse(string formula) => this.Parse(formula, null);

        public FormulaParserResult<Node> Parse(string formula, IReadOnlyDictionary<string, double> variables)
        {
            return WrapWithExceptionHandling(() =>
            {
                var node = this.ParseFormula(formula, variables);
                return new FormulaParserResult<Node>(node);
            });
        }

        public FormulaParserResult<TResult> RunVisitor<TResult>(string formula, INodeVisitor<TResult> visitor) => RunVisitor(formula, visitor, null);

        public FormulaParserResult<TResult> RunVisitor<TResult>(string formula, INodeVisitor<TResult> visitor, IReadOnlyDictionary<string, double> variables)
        {
            return WrapWithExceptionHandling(() =>
            {
                var node = this.ParseFormula(formula, variables);
                var result = node.Visit(visitor);

                return new FormulaParserResult<TResult>(result);
            });
        }

        public FormulaParserResult<TResult> RunVisitor<TResult>(Node node, INodeVisitor<TResult> visitor)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            return WrapWithExceptionHandling(() =>
            {
                var result = node.Visit(visitor);
                return new FormulaParserResult<TResult>(result);
            });
        }

        private static FormulaParserResult<TResult> WrapWithExceptionHandling<TResult>(Func<FormulaParserResult<TResult>> action)
        {
            try
            {
                return action();
            }
            catch (ParsingException pex)
            {
                return new FormulaParserResult<TResult>(new ParsingError(pex.Line, pex.Column, pex.InvalidToken, pex.Message));
            }
            catch (Exception ex)
            {
                return new FormulaParserResult<TResult>(new Error(ex.Message));
            }

        }

        private Node ParseFormula(string formula, IReadOnlyDictionary<string, double> variables)
        {
            var inputStream = new AntlrInputStream(formula);            
            var lexer = new FormulaGrammerLexer(inputStream);
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(ThrowingErrorListener.Instance);
            var tokens = new CommonTokenStream(lexer);
            var parser = new FormulaGrammerParser(tokens);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(ThrowingErrorListener.Instance);

            var antlrTree = parser.formula();

            var config = new ParserConfiguration(this.configuration, variables);

            var antlrToAstVisitor = new AntlrToAstNodesGrammerTreeVisitor(config, formula);

            return antlrToAstVisitor.Visit(antlrTree);
        }

        public class ThrowingErrorListener : IAntlrErrorListener<int>, IAntlrErrorListener<IToken>
        {
            public static ThrowingErrorListener Instance { get; } = new ThrowingErrorListener();

            public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
                => ParsingException.ThrowException(line, charPositionInLine, e.OffendingToken?.Text ?? "<?>", msg);

            public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
                => ParsingException.ThrowException(line, charPositionInLine, offendingSymbol?.Text ?? "<?>", msg);
        }
    }
}
