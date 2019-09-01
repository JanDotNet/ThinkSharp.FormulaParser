using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FormulaParsing
{
    public class EvaluatingException : Exception
    {
        private EvaluatingException(string message) : base(message)
        { }

        internal static Exception VariableDoesNotExist(string name)
            => new EvaluatingException($"Variable '{name}' does not exist.");

        internal static Exception ConstantDoesNotExist(string name)
            => new EvaluatingException($"Constant '{name}' does not exist.");

        internal static Exception VariableNameConflictsWithExistingConstantName(string name)
            => new EvaluatingException($"Variable name '{name}' conflicts with the name of an existing constant.");

        internal static Exception UnableToEvaluateNotExistingFunction(string name)
            => new EvaluatingException($"Unable to evaluate function '{name}' because it does not exist.");
    }
}
