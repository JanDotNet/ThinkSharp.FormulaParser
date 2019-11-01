using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FormulaParsing
{
    internal class Configuration : IConfigureFunctions, IConfigureConstants, IConfigureSupportedFeatures, IConfigureValidationBehavior, IConfiguration, IConfigurationEvaluator
    {
        private static readonly Random random = new Random();

        private readonly Dictionary<string, Func<double>> functionsArgs0 = new Dictionary<string, Func<double>>();
        private readonly Dictionary<string, Func<double, double>> functionsArgs1 = new Dictionary<string, Func<double, double>>();
        private readonly Dictionary<string, Func<double, double, double>> functionsArgs2 = new Dictionary<string, Func<double, double, double>>();
        private readonly Dictionary<string, Func<double, double, double, double>> functionsArgs3 = new Dictionary<string, Func<double, double, double, double>>();
        private readonly Dictionary<string, Func<double, double, double, double, double>> functionsArgs4 = new Dictionary<string, Func<double, double, double, double, double>>();
        private readonly Dictionary<string, Func<double, double, double, double, double, double>> functionsArgs5 = new Dictionary<string, Func<double, double, double, double, double, double>>();

        private readonly Dictionary<string, Func<double[], double>> functionsArgsN = new Dictionary<string, Func<double[], double>>();

        private readonly Dictionary<string, double> constants = new Dictionary<string, double>();

        // .ctor
        // ////////////////////////////////////////////////////////////////////

        public Configuration()
        {   
            (this as IConfigureFunctions).Add("max", args => args.Aggregate(Math.Max));
            (this as IConfigureFunctions).Add("min", args => args.Aggregate(Math.Min));
            (this as IConfigureFunctions).Add("sum", args => args.Aggregate((x, y) => x + y));

            (this as IConfigureFunctions).Add("rnd", () => random.NextDouble());
            (this as IConfigureFunctions).Add("abs", u => Math.Abs(u));
            (this as IConfigureFunctions).Add("ln", u => Math.Log(u));
            (this as IConfigureFunctions).Add("log", (u, v) => Math.Log(u, v));
            (this as IConfigureFunctions).Add("log", u => Math.Log10(u));
            (this as IConfigureFunctions).Add("sin", u => Math.Sin(u));
            (this as IConfigureFunctions).Add("cos", u => Math.Cos(u));
            (this as IConfigureFunctions).Add("tan", u => Math.Tan(u));
            (this as IConfigureFunctions).Add("sqrt", u => Math.Sqrt(u));

            (this as IConfigureConstants).Add("pi", Math.PI);
            (this as IConfigureConstants).Add("e", Math.E);
        }

        // IConfigureFunctions
        // ////////////////////////////////////////////////////////////////////

        public bool IsScientificNotationSupportDisabled { get; private set; } = false;

        public bool IsBracketSupportDisabled { get; private set; } = false;

        public bool IsPowSupportDisabled { get; private set; } = false;

        public bool IsVariablesSupportDisabled { get; private set; } = false;

        public bool IsFunctionsSupportDisabled { get; private set; } = false;

        public bool IsVariableNameValidationDisabled { get; private set; } = false;

        public bool IsFunctionNameValidationDisabled { get; private set; } = false;

        public bool IsBinaryNumberNotationSupportDisabled { get; private set; } = false;

        public bool IsHexadecimalNumberNotationSupportDisabled { get; private set; } = false;

        public bool IsOctalNumberNotationSupportDisabled { get; private set; } = false;

        bool IConfiguration.HasFunction(string name, int argumentCount)
        {
            if (this.MatchFunction0To5(name, argumentCount))
            {
                return true;
            }

            if (argumentCount >= 1)
            {
                return this.functionsArgsN.ContainsKey(name);
            }

            return false;
        }

        bool IConfiguration.HasFunction(string name)
        {
            return this.functionsArgs0.ContainsKey(name)
                   || this.functionsArgs1.ContainsKey(name)
                   || this.functionsArgs2.ContainsKey(name)
                   || this.functionsArgs3.ContainsKey(name)
                   || this.functionsArgs4.ContainsKey(name)
                   || this.functionsArgs5.ContainsKey(name)
                   || this.functionsArgsN.ContainsKey(name);                    
        }

        private bool MatchFunction0To5(string name, int argumentCount)
        {
            switch (argumentCount)
            {
                case 0: return this.functionsArgs0.ContainsKey(name);
                case 1: return this.functionsArgs1.ContainsKey(name);
                case 2: return this.functionsArgs2.ContainsKey(name);
                case 3: return this.functionsArgs3.ContainsKey(name);
                case 4: return this.functionsArgs4.ContainsKey(name);
                case 5: return this.functionsArgs5.ContainsKey(name);
                default: return false;
            };
        }

        bool IConfiguration.HasConstant(string name) => this.constants.ContainsKey(name);

        // IFunctionRegistry implementation
        // ////////////////////////////////////////////////////////////////////

        void IConfigureFunctions.Add(string name, Func<double> function)
        {
            EnsureFunctionNotExist(name, this.functionsArgs0);

            this.functionsArgs0.Add(name, function);
        }

        void IConfigureFunctions.Add(string name, Func<double, double> function)
        {
            EnsureFunctionNotExist(name, this.functionsArgs1);

            this.functionsArgs1.Add(name, function);
        }

        void IConfigureFunctions.Add(string name, Func<double, double, double> function)
        {
            EnsureFunctionNotExist(name, this.functionsArgs2);
            EnsureFunctionNotExist(name, this.functionsArgsN);

            this.functionsArgs2.Add(name, function);
        }

        void IConfigureFunctions.Add(string name, Func<double, double, double, double> function)
        {
            EnsureFunctionNotExist(name, this.functionsArgs3);
            EnsureFunctionNotExist(name, this.functionsArgsN);

            this.functionsArgs3.Add(name, function);
        }

        void IConfigureFunctions.Add(string name, Func<double, double, double, double, double> function)
        {
            EnsureFunctionNotExist(name, this.functionsArgs4);
            EnsureFunctionNotExist(name, this.functionsArgsN);

            this.functionsArgs4.Add(name, function);
        }

        void IConfigureFunctions.Add(string name, Func<double, double, double, double, double, double> function)
        {
            EnsureFunctionNotExist(name, this.functionsArgs5);
            EnsureFunctionNotExist(name, this.functionsArgsN);

            this.functionsArgs5.Add(name, function);
        }

        void IConfigureFunctions.Add(string name, Func<double[], double> function)
        {
            EnsureFunctionNotExist(name, this.functionsArgs2);
            EnsureFunctionNotExist(name, this.functionsArgs3);
            EnsureFunctionNotExist(name, this.functionsArgs4);
            EnsureFunctionNotExist(name, this.functionsArgs5);
            EnsureFunctionNotExist(name, this.functionsArgsN);

            this.functionsArgsN.Add(name, function);
        }

        void IConfigureFunctions.RemoveAll()
        {
            this.functionsArgs0.Clear();
            this.functionsArgs1.Clear();
            this.functionsArgs2.Clear();
            this.functionsArgs3.Clear();
            this.functionsArgs4.Clear();
            this.functionsArgs5.Clear();
            this.functionsArgsN.Clear();
        }

        void IConfigureFunctions.Remove(string name)
        {
            this.functionsArgs0.Remove(name);
            this.functionsArgs1.Remove(name);
            this.functionsArgs2.Remove(name);
            this.functionsArgs3.Remove(name);
            this.functionsArgs4.Remove(name);
            this.functionsArgs5.Remove(name);
            this.functionsArgsN.Remove(name);
        }


        // IConstantRegistry implementation
        // ////////////////////////////////////////////////////////////////////

        void IConfigureConstants.Add(string name, double value)
        {
            if (constants.ContainsKey(name))
            {
                throw ParserConfigurationException.ConstantAlreadyExists(name);
            }

            this.constants.Add(name, value);
        }

        void IConfigureConstants.RemoveAll()
        {
            this.constants.Clear();
        }

        void IConfigureConstants.Remove(string name)
        {
            this.constants.Remove(name);
        }


        // IConfigurationEvaluation implementation
        // ////////////////////////////////////////////////////////////////////

        double IConfigurationEvaluator.EvaluateFunction(string name, double[] args)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            switch (args.Length)
            {
                case 0:
                    if (this.functionsArgs0.TryGetValue(name, out var fun0))
                        return fun0();
                    break;
                case 1:
                    if (this.functionsArgs1.TryGetValue(name, out var fun1))
                        return fun1(args[0]);
                    break;
                case 2:
                    if (this.functionsArgs2.TryGetValue(name, out var fun2))
                        return fun2(args[0], args[1]);
                    break;
                case 3:
                    if (this.functionsArgs3.TryGetValue(name, out var fun3))
                        return fun3(args[0], args[1], args[2]);
                    break;
                case 4:
                    if (this.functionsArgs4.TryGetValue(name, out var fun4))
                        return fun4(args[0], args[1], args[2], args[3]);
                    break;
                case 5:
                    if (this.functionsArgs5.TryGetValue(name, out var fun5))
                        return fun5(args[0], args[1], args[2], args[3], args[4]);
                    break;
            }

            if (this.functionsArgsN.TryGetValue(name, out var funN))
                return funN(args);

            throw EvaluatingException.UnableToEvaluateNotExistingFunction(name);
        }

        IEnumerable<KeyValuePair<string, double>> IConfigurationEvaluator.EnumerateConstantes()
        {
            return this.constants.AsEnumerable();
        }

        // IConfigureSupportedFeatures
        // ////////////////////////////////////////////////////////////////////

        void IConfigureSupportedFeatures.DisableScientificNotation() => this.IsScientificNotationSupportDisabled = true;
        void IConfigureSupportedFeatures.DisableBinaryNumberNotation() => this.IsBinaryNumberNotationSupportDisabled = true;
        void IConfigureSupportedFeatures.DisableHexadecimalNumberNotation() => this.IsHexadecimalNumberNotationSupportDisabled = true;
        void IConfigureSupportedFeatures.DisableOctalNumberNotation() => this.IsOctalNumberNotationSupportDisabled = true;
        void IConfigureSupportedFeatures.DisableBracket() => this.IsBracketSupportDisabled = true;
        void IConfigureSupportedFeatures.DisablePow() => this.IsPowSupportDisabled = true;
        void IConfigureSupportedFeatures.DisableVariables() => this.IsVariablesSupportDisabled = true;
        void IConfigureSupportedFeatures.DisableFunctions() => this.IsFunctionsSupportDisabled = true;

        // IConfigureValidationBehavior
        // ////////////////////////////////////////////////////////////////////

        void IConfigureValidationBehavior.DisableVariableNameValidation() => this.IsVariableNameValidationDisabled = true;
        void IConfigureValidationBehavior.DisableFunctionNameValidation() => this.IsFunctionNameValidationDisabled = true;

        // Helper
        // ////////////////////////////////////////////////////////////////////

        private static void EnsureFunctionNotExist<TValue>(string name, IDictionary<string, TValue> dict)
        {
            if (dict.ContainsKey(name))
            {
                throw ParserConfigurationException.FunctionAlreadyExists(name);
            }
        }
    }
}
