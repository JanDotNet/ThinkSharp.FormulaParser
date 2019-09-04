using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FormulaParsing
{
    internal class FormulaParserBuilder : IFormulaParserBuilder
    {
        private readonly Configuration configuration = new Configuration();

        public IFormulaParser Build()
        {
            return new FormulaParser(configuration);
        }

        public IFormulaParserBuilder ConfigureConstats(Action<IConfigureConstants> constants) => Configure(constants);

        public IFormulaParserBuilder ConfigureFunctions(Action<IConfigureFunctions> functions) => Configure(functions);

        public IFormulaParserBuilder ConfigureSupportedFeatures(Action<IConfigureSupportedFeatures> supportedFeatures) => Configure(supportedFeatures);

        public IFormulaParserBuilder ConfigureValidationBehavior(Action<IConfigureValidationBehavior> parsingBehavior) => Configure(parsingBehavior);

        private IFormulaParserBuilder Configure<TConfigure>(Action<TConfigure> action) where TConfigure : class
        {
            var config = this.configuration as TConfigure ?? throw new InvalidOperationException($"Type '{typeof(TConfigure).Name}' must be implemented by Configuration.");
            action(config);
            return this;
        }
    }
}
