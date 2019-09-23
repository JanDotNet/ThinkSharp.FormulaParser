using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ThinkSharp.FormulaParsing.Validation
{
    public static class ValidationHelper
    {
        private static Regex variableRegex = new Regex("[a-zA-Z$_][a-zA-Z0-9$_]*", RegexOptions.Compiled);

        public static bool IsValidIdentifier(string name) => variableRegex.IsMatch(name);
    }
}
