namespace ThinkSharp.FormulaParsing
{
    public interface IConfigureSupportedFeatures
    {
        /// <summary>
        /// Prevents the usage of scientific notation (e.g. 2e3 = 4000)
        /// </summary>
        void DisableScientificNotation();

        /// <summary>
        /// Prevents the usage of brackets.
        /// </summary>
        void DisableBracket();

        /// <summary>
        /// Prevents the usage of pow (e.g. 3^2 = 9)
        /// </summary>
        void DisablePow();

        /// <summary>
        /// Ptevents the usage of variables.
        /// </summary>
        void DisableVariables();

        /// <summary>
        /// Prevents the usage of functions.
        /// </summary>
        void DisableFunctions();
    }
}
