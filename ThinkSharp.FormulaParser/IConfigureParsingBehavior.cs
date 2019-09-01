using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FormulaParsing
{
    public interface IConfigureParsingBehavior
    {
        void DisableVariableNameValidation();

        void DisableFunctionNameValidation();
    }
}
