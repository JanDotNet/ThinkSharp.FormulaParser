using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FormulaParsing
{
    /// <summary>
    /// Interface encapsulating the API for configuring validation behavior.
    /// </summary>
    public interface IConfigureValidationBehavior
    {
        /// <summary>
        /// Diables the validation of variable names when creating a parsing tree.
        /// </summary>
        void DisableVariableNameValidation();

        /// <summary>
        /// Diables the validation of functions names when creating a parsing tree.
        /// </summary>
        void DisableFunctionNameValidation();
    }
}
