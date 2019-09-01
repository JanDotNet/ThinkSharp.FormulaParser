using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing.Ast.Nodes
{
    public class FormulaNode : Node
    {
        public FormulaNode(Node childNode, string formularText)
        {
            this.ChildNode = childNode ?? throw new ArgumentNullException(nameof(childNode));
            this.FormulaText = formularText;
        }

        public string FormulaText { get; }

        public Node ChildNode { get; }

        public override TReturn Visit<TReturn>(INodeVisitor<TReturn> visitor) => visitor.Visit(this);
    }
}
