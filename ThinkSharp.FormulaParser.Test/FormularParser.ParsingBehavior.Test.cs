using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing.Test
{
    [TestClass]
    public class ConfigureParsingBehaviorTest
    {      
        [TestMethod]
        public void TestNotConfigured_InvalidVariable()
        {
            var parser = FormulaParser.Create();

            var result = parser.Parse("1 * TEST");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("column 4: Unknown variable 'TEST'.", (string)result.Error);
        }

        [TestMethod]
        public void TestNotConfigured_InvalidVariableIgnoreVariableValidation()
        {
            var parser = FormulaParser
                .CreateBuilder()
                .ConfigureParsingBehavior(parsingBehavior =>
                {
                    parsingBehavior.DisableVariableNameValidation();
                })
                .Build();

            var result = parser.Parse("1 * TEST");
            Assert.IsTrue(result.Success, string.Join(", ", (string)result.Error));
            var variableNames = result.Value.Visit(new CollectVariableNamesVisitor());
            Assert.AreEqual(1, variableNames.Count);
            Assert.AreEqual("TEST", variableNames[0]);
        }

        [TestMethod]
        public void TestNotConfigured_InvalidFunctions()
        {
            var parser = FormulaParser.Create();

            var result = parser.Parse("1 * test()");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("column 4: Unknown function 'test'.", (string)result.Error);
        }

        [TestMethod]
        public void TestNotConfigured_InvalidFunctionIgnoreFunctionValidation()
        {
            var parser = FormulaParser
                .CreateBuilder()
                .ConfigureParsingBehavior(parsingBehavior =>
                {
                    parsingBehavior.DisableFunctionNameValidation();
                })
                .Build();

            var result = parser.Parse("1 * TEST()");
            Assert.IsTrue(result.Success, string.Join(", ", (string)result.Error));
            var functionNames = result.Value.Visit(new CollectFunctionNamesVisitor());
            Assert.AreEqual(1, functionNames.Count);
            Assert.AreEqual("TEST", functionNames[0]);
        }

        private class CollectFunctionNamesVisitor : NodeVisitor<List<string>>
        {
            private readonly List<string> functionNames = new List<string>();

            public override List<string> Visit(FunctionNode node)
            {
                functionNames.Add(node.FunctionName);
                return base.Visit(node);
            }

            protected override List<string> DefaultResult() => functionNames;
        }

        private class CollectVariableNamesVisitor : NodeVisitor<List<string>>
        {
            private readonly List<string> variableNames = new List<string>();

            public override List<string> Visit(VariableNode node)
            {
                variableNames.Add(node.Name);
                return variableNames;
            }

            protected override List<string> DefaultResult() => variableNames;
        }
    }
}
