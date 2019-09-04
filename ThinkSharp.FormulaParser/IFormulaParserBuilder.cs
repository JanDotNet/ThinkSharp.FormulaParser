using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FormulaParsing
{
    public interface IFormulaParserBuilder
    {
        /// <summary>
        /// Allows to configure functions of the <see cref="IFormulaParser"/>.
        /// </summary>
        /// <param name="functions">
        /// The <see cref="IConfigureFunctions"/> object that provides methods for configuring functions of the formula parser.
        /// </param>
        /// <returns>
        /// The <see cref="FormulaParserBuilder"/>.
        /// </returns>
        IFormulaParserBuilder ConfigureFunctions(Action<IConfigureFunctions> functions);

        /// <summary>
        /// Allows to configure constants of the <see cref="IFormulaParser"/>.
        /// </summary>
        /// <param name="constants">
        /// The <see cref="IConfigureConstants"/> object that provides methods for configuring constants of the formula parser.
        /// </param>
        /// <returns>
        /// The <see cref="FormulaParserBuilder"/>.
        /// </returns>
        IFormulaParserBuilder ConfigureConstats(Action<IConfigureConstants> constants);

        /// <summary>
        /// Allows to disable features of the <see cref="IFormulaParser"/>.
        /// </summary>
        /// <param name="constants">
        /// The <see cref="IConfigureSupportedFeatures"/> object that provides methods for disabling features of the formula parser.
        /// </param>
        /// <returns>
        /// The <see cref="IFormulaParserBuilder"/>.
        /// </returns>
        IFormulaParserBuilder ConfigureSupportedFeatures(Action<IConfigureSupportedFeatures> supportedFeatures);

        /// <summary>
        /// Allows to configure the validation behavior of the <see cref="IFormulaParser"/>.
        /// </summary>
        /// <remarks>
        /// The default behavior is, that names of configured functions / provided variables are required. If the formula contains unknown
        /// functions / variables, the parsing process fails with an appropriated error message.
        /// This method allows to disable validation for variables / functions which may be useful for creating a parsing tree. 
        /// </remarks>
        /// <param name="parsingBehavior">
        /// The <see cref="IConfigureValidationBehavior"/> object that provides methods for disabling features of the formula parser.
        /// </param>
        /// <returns>
        /// The <see cref="IFormulaParserBuilder"/>.
        /// </returns>
        IFormulaParserBuilder ConfigureValidationBehavior(Action<IConfigureValidationBehavior> parsingBehavior);

        /// <summary>
        /// Build the configured <see cref="IFormulaParser"/>.
        /// </summary>
        /// <returns>
        /// The configured <see cref="IFormulaParser"/>.
        /// </returns>
        IFormulaParser Build();
    }
}
