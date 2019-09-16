using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing
{
    public interface IFormulaParser
    {
        /// <summary>
        /// Evaluates the provided formula to a numeric value.
        /// </summary>
        /// <param name="formula">
        /// The formula to evaluate.
        /// </param>
        /// <returns>
        /// The <see cref="FormulaParserResult{T}"/> object that contains the evaluation result or an error.
        /// </returns>
        FormulaParserResult<double> Evaluate(string formula);

        /// <summary>
        /// Evaluates the provided formula to a numeric value.
        /// </summary>
        /// <param name="formula">
        /// The formula to evaluate.
        /// </param>
        /// <param name="variables">
        /// A dictionary that provides variables to be used for evaluation.
        /// </param>
        /// <returns>
        /// The <see cref="FormulaParserResult{T}"/> object that contains the evaluation result or an error.
        /// </returns>
        FormulaParserResult<double> Evaluate(string formula, IDictionary<string, double> variables);

        /// <summary>
        /// Evaluates the provided <see cref="Node"/>.
        /// </summary>
        /// <param name="formulaNode">
        /// The root node of the parsing tree to evaluate.
        /// </param>
        /// <returns>
        /// The <see cref="FormulaParserResult{T}"/> object that contains the evaluation result or an error.
        /// </returns>
        FormulaParserResult<double> Evaluate(Node formulaNode);

        /// <summary>
        /// Evaluates the provided <see cref="Node"/>.
        /// </summary>
        /// <param name="formulaNode">
        /// The root node of the parsing tree to evaluate.
        /// </param>
        /// <param name="variables">
        /// A dictionary that provides variables to be used for evaluation.
        /// </param>
        /// <returns>
        /// The <see cref="FormulaParserResult{T}"/> object that contains the evaluation result or an error.
        /// </returns>
        FormulaParserResult<double> Evaluate(Node formulaNode, IDictionary<string, double> variables);

        /// <summary>
        /// Parses the provided formula to a parsing tree.
        /// </summary>
        /// <param name="formula">
        /// The formula to parse.
        /// </param>
        /// <returns>
        /// The <see cref="FormulaParserResult{T}"/> object that contains the root node of the parsing tree or an error.
        /// </returns>
        FormulaParserResult<Node> Parse(string formula);

        /// <summary>
        /// Parses the provided formula to a parsing tree.
        /// </summary>
        /// <param name="formula">
        /// The formula to parse.
        /// </param>
        /// <param name="variables">
        /// A dictionary that provides variables to be used for evaluation.
        /// </param>
        /// <returns>
        /// The <see cref="FormulaParserResult{T}"/> object that contains the root node of the parsing tree or an error.
        /// </returns>
        FormulaParserResult<Node> Parse(string formula, IDictionary<string, double> variables);

        /// <summary>
        /// Parses the formula and executes the visitor to the genereted parsing tree.
        /// </summary>
        /// <typeparam name="TResult">
        /// The type of the visitors result.
        /// </typeparam>
        /// <param name="formula">
        /// The formula to parse.
        /// </param>
        /// <param name="visitor">
        /// The visitor to run on the parsing tree.
        /// </param>
        /// <returns>
        /// The <see cref="FormulaParserResult{T}"/> object that contains the result produced by the visitor or an error.
        /// </returns>
        FormulaParserResult<TResult> RunVisitor<TResult>(string formula, INodeVisitor<TResult> visitor);

        /// <summary>
        /// Parses the formula and executes the visitor to the genereted parsing tree.
        /// </summary>
        /// <typeparam name="TResult">
        /// The type of the visitors result.
        /// </typeparam>
        /// <param name="formula">
        /// The formula to parse.
        /// </param>
        /// <param name="visitor">
        /// The visitor to run on the parsing tree.
        /// </param>
        /// <param name="variables">
        /// A dictionary that provides variables to be used for evaluation.
        /// </param>
        /// <returns>
        /// The <see cref="FormulaParserResult{T}"/> object that contains the result produced by the visitor or an error.
        /// </returns>
        FormulaParserResult<TResult> RunVisitor<TResult>(string formula, INodeVisitor<TResult> visitor, IDictionary<string, double> variables);

        /// <summary>
        /// Executes the visitor to the provided parsing tree.
        /// </summary>
        /// <typeparam name="TResult">
        /// The type of the visitors result.
        /// </typeparam>
        /// <param name="node">
        /// The root node of the parsing tree to run the visitor on.
        /// </param>
        /// <param name="visitor">
        /// The visitor to run on the parsing tree.
        /// </param>
        /// <returns>
        /// The <see cref="FormulaParserResult{T}"/> object that contains the result produced by the visitor or an error.
        /// </returns>
        FormulaParserResult<TResult> RunVisitor<TResult>(Node node, INodeVisitor<TResult> visitor);
    }
}
