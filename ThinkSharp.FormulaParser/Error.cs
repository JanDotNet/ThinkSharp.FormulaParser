using System;
using System.Collections.Generic;
using System.Text;

namespace ThinkSharp.FormulaParsing
{
    public class Error
    {
        internal Error(string message)
        {
            this.Message = message;
        }

        public static implicit operator string(Error error) => error?.ToString();

        public string Message { get; }

        public override string ToString() => this.Message;
    }
}
