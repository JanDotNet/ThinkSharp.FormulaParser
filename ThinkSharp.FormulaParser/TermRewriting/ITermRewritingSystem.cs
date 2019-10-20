using System;
using System.Collections.Generic;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.TermRewriting
{
    public interface ITermRewritingSystem
    {
        Node Simplify(Node node);
    }
}
