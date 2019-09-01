using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.Ast.Visitors
{
    public abstract class NodeVisitor<TResult> : INodeVisitor<TResult>
    {
        public virtual TResult Visit(FormulaNode node) => node.ChildNode.Visit(this);

        public virtual TResult Visit(BracketedNode node) => node.ChildNode.Visit(this);

        public virtual TResult Visit(NumberNode node) => DefaultResult();

        public virtual TResult Visit(VariableNode node) => DefaultResult();

        public virtual TResult Visit(ConstantNode node) => DefaultResult();

        public virtual TResult Visit(BinaryOperatorNode node)
        {
            node.LeftNode.Visit(this);
            node.RightNode.Visit(this);
            return DefaultResult();
        }

        public virtual TResult Visit(PowerNode node)
        {
            node.BaseNode.Visit(this);
            node.ExponentNode.Visit(this);
            return DefaultResult();
        }

        public virtual TResult Visit(SignedNode node)
        {
            node.Node.Visit(this);
            return DefaultResult();
        }
        
        public virtual TResult Visit(FunctionNode node)
        {
            node.Parameters.ToList().ForEach(p => p.Visit(this));
            return DefaultResult();
        }

        protected virtual TResult DefaultResult() => default(TResult);
    }
}
