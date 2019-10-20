using System;
using System.Collections.Generic;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing.TermRewriting.Rules
{
    public abstract class Rule
    {
        public abstract bool Match(Node node);

        public abstract Node Rewrite(Node node);
    }
}
