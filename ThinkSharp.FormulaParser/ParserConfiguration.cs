using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FormulaParsing
{
    internal class ParserConfiguration : IParserConfiguration
    {
        private readonly IConfiguration configuration;
        private readonly HashSet<string> varialeNames;

        public ParserConfiguration(IConfiguration configuration, IDictionary<string, double> variables)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.varialeNames = new HashSet<string>(variables?.Keys ?? Enumerable.Empty<string>());
        }

        public bool IsScientificNotationSupportDisabled => this.configuration.IsScientificNotationSupportDisabled;

        public bool IsBracketSupportDisabled => this.configuration.IsBracketSupportDisabled;

        public bool IsPowSupportDisabled => this.configuration.IsPowSupportDisabled;

        public bool IsVariablesSupportDisabled => this.configuration.IsVariablesSupportDisabled;

        public bool IsFunctionsSupportDisabled => this.configuration.IsFunctionsSupportDisabled;

        public bool IsVariableNameValidationDisabled => this.configuration.IsVariableNameValidationDisabled;

        public bool IsFunctionNameValidationDisabled => this.configuration.IsFunctionNameValidationDisabled;

        public bool HasConstant(string name) => this.configuration.HasConstant(name);

        public bool HasFunction(string name, int argumentCount) => this.configuration.HasFunction(name, argumentCount);

        public bool HasFunction(string name) => this.configuration.HasFunction(name);

        public bool HasVariable(string variable) => this.varialeNames.Contains(variable);
    }
}
