using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing.Ast.Nodes
{
    public abstract class Node
    {
        public abstract TReturn Visit<TReturn>(INodeVisitor<TReturn> visitor);
    }
}
