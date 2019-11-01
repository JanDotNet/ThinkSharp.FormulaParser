namespace ThinkSharp.FormulaParsing
{
    /// <summary>
    /// Interface encapsulating the API for configuring supported features.
    /// </summary>
    public interface IConfigureSupportedFeatures
    {
        /// <summary>
        /// Prevents the usage of scientific notation (e.g. 2e3 = 4000)
        /// </summary>
        void DisableScientificNotation();

        /// <summary>
        /// Prevents the usage of binary notation (e.g. 0b101 = 5)
        /// </summary>
        void DisableBinaryNumberNotation();

        /// <summary>
        /// Prevents the usage of hexadecimal notation (e.g. 0x20 = 32)
        /// </summary>
        void DisableHexadecimalNumberNotation();

        /// <summary>
        /// Prevents the usage of octal notation (e.g. 0o10 = 8)
        /// </summary>
        void DisableOctalNumberNotation();

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
