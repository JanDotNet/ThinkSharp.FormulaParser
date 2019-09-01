using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.Ast.Visitors
{
    public interface INodeVisitor<TReturn>
    {
        TReturn Visit(FormulaNode node);

        TReturn Visit(BracketedNode node);

        TReturn Visit(NumberNode node);

        TReturn Visit(VariableNode node);

        TReturn Visit(ConstantNode node);

        TReturn Visit(BinaryOperatorNode node);

        TReturn Visit(PowerNode node);

        TReturn Visit(SignedNode node);

        TReturn Visit(FunctionNode node);
    }
}
