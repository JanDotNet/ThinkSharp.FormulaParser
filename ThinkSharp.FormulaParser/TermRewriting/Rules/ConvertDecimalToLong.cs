using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing.TermRewriting.Rules
{
    public class ConvertDecimalToLong : Rule
    {
        protected override bool MatchInternal(Node node, Node Parent)
        {
            return node is DecimalNode dec && dec.Value % 1 == 0;
        }

        protected override Node RewriteInternal(Node node, Node Parent)
        {
            if (node is DecimalNode dec && dec.Value % 1 == 0)
            {
                try
                {
                    return new IntegerNode(NumberFormat.Dec, (long)dec.Value);
                }
                catch
                {
                    Debug.Fail("Unable to convert decimal to long.");
                    // ignore
                }
            }

            Debug.Fail("Should not happen");
            return node;
        }
    }
}
