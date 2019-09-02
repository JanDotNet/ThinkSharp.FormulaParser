using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FormulaParsing
{
    public class ParsingException : Exception
    {
        private ParsingException(int line, int column, string invalidToken, string message) : base(message)
        {
            this.Line = line;
            this.Column = column;
            this.InvalidToken = invalidToken;
        }

        public int Line { get; }

        public int Column { get; }

        public string InvalidToken { get; }

        internal static void ThrowException(int line, int column, string invalidToken, string message) => throw new ParsingException(line, column, invalidToken, message);

        internal static void ThrowInvalidTokenException(IToken token) => ThrowException(token.Line, token.Column, token.Text, $"Invalid token '{token.Text}'.");

        internal static void ThrowUnknownFunctionException(IToken token) => ThrowException(token.Line, token.Column, token.Text, $"Unknown function '{token.Text}'.");

        internal static void ThrowFunctionArgumentCountDoesNotExistException(IToken token, int argumentCount) => ThrowException(token.Line, token.Column, token.Text, $"There is no function '{token.Text}' that takes {argumentCount} argument(s).");

        internal static void ThrowUnknownVariableException(IToken token) => ThrowException(token.Line, token.Column, token.Text, $"Unknown variable '{token.Text}'.");
    }
}
