using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FormulaParsing
{
    internal interface IConfiguration
    {
        bool IsScientificNotationSupportDisabled { get; }

        bool IsBracketSupportDisabled { get; }

        bool IsPowSupportDisabled { get; }

        bool IsVariablesSupportDisabled { get; }

        bool IsFunctionsSupportDisabled { get; }

        bool IsVariableNameValidationDisabled { get; }

        bool IsFunctionNameValidationDisabled { get; }

        bool HasFunction(string name);

        bool HasFunction(string name, int argumentCount);

        bool HasConstant(string name);
    }
}
