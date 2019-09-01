using System;
using System.Collections.Generic;
using System.Text;

namespace ThinkSharp.FormulaParsing
{
    public class ParsingError : Error
    {
        internal ParsingError(int line, int column, string invalidToken, string message)
        : base(message)
        {
            this.Line = line;
            this.Column = column;
            this.InvalidToken = invalidToken;
        }

        public int Line { get; }

        public int Column { get; }

        public string InvalidToken { get; }

        public override string ToString() => "column " + this.Column + ": " + this.Message;
    }
}
