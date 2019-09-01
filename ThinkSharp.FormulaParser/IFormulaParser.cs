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
        FormulaParserResult<double> Evaluate(string formula);

        FormulaParserResult<double> Evaluate(string formula, IDictionary<string, double> variables);

        FormulaParserResult<double> Evaluate(Node formulaNode);

        FormulaParserResult<double> Evaluate(Node formulaNode, IDictionary<string, double> variables);

        FormulaParserResult<Node> Parse(string formula);

        FormulaParserResult<Node> Parse(string formula, IDictionary<string, double> variables);

        FormulaParserResult<TResult> RunVisitor<TResult>(string formula, INodeVisitor<TResult> visitor);

        FormulaParserResult<TResult> RunVisitor<TResult>(string formula, INodeVisitor<TResult> visitor, IDictionary<string, double> variables);

        FormulaParserResult<TResult> RunVisitor<TResult>(Node node, INodeVisitor<TResult> visitor);
    }
}
