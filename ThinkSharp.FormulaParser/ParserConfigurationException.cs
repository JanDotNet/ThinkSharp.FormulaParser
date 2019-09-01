using System;
using System.Collections.Generic;
using System.Text;

namespace ThinkSharp.FormulaParsing
{
    public class ParserConfigurationException : Exception
    {
        private ParserConfigurationException(string message) : base(message)
        { }

        public static Exception ConstantAlreadyExists(string name)
            => new ParserConfigurationException($"Constant with name '{name}' already exists.");

        public static Exception FunctionAlreadyExists(string name)
            => new ParserConfigurationException($"Function with name '{name}' already exists.");
    }
}
