using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FormulaParsing
{
    public interface IFormulaParserBuilder
    {
        IFormulaParserBuilder ConfigureFunctions(Action<IConfigureFunctions> functions);

        IFormulaParserBuilder ConfigureConstats(Action<IConfigureConstants> constants);

        IFormulaParserBuilder ConfigureSupportedFeatures(Action<IConfigureSupportedFeatures> supportedFeatures);

        IFormulaParserBuilder ConfigureParsingBehavior(Action<IConfigureParsingBehavior> parsingBehavior);

        IFormulaParser Build();
    }
}
