namespace ThinkSharp.FormulaParsing
{
    public interface IConfigureSupportedFeatures
    {
        void DisableScientificNotation();

        void DisableBracket();

        void DisablePow();

        void DisableVariables();

        void DisableFunctions();
    }
}
