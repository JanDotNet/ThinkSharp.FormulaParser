using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThinkSharp.FormulaParsing.Ast.Nodes;

namespace ThinkSharp.FormulaParsing
{
    [TestClass]
    public class FormulaParserTest
    {
        [TestMethod]
        public void TestAddOneAndOne()
        {
            AsserEval(1.0, "1");
            AsserEval(2.0, "1+1");
            AsserEval(3.0, "1+1+1");
        }

        [TestMethod]
        public void TestSignedNumber()
        {
            AsserEval(1.0, "+1");
            AsserEval(-1.0, "-1");
        }

        [TestMethod]
        public void TestSignedNumber2()
        {
            AssertFailure("column 1:", "++1");
            AssertFailure("column 1:", "--1");
        }

        [TestMethod]
        public void TestMulDivFirst()
        {
            AsserEval(6.0, "1+1*5");
            AsserEval(6.0, "1*1+5");
            AsserEval(11.0, "1+2*3+4");

            AsserEval(3.0, "1+10/5");
            AsserEval(5.1, "1/10+5");
        }

        [TestMethod]
        public void TestBrackets()
        {
            AsserEval(3.0, "(1+1+1)");            
            AsserEval(10.0, "(1+1)*5");
            AsserEval(7.0, "1*(2+5)");

            AsserEval(2.2, "(1+10)/5");
            AsserEval(10.0, "150/(10+5)");
        }

        [TestMethod]
        public void TestBrackets_MultipleBrackets()
        {
            AsserEval(2.0, "((1+1))");
            AsserEval(3118.76093, "((233*4323-23/432 * 234 -34 +32) +3234)/324");
        }

        [TestMethod]
        public void TestVariables()
        {
            AsserEval(2.0, "1+TEST", "TEST:1");
            AsserEval(11.0, "1+TEST*5", "TEST:2");
            AsserEval(5.0, "1+TEST*TEST", "TEST:2");

            AsserEval(14.0, "TEST1 * (TEST2 + TEST3)", "TEST1:2", "TEST2:3", "TEST3:4");
        }

        [TestMethod]
        public void Test_ConstPi()
        {
            AsserEval(3.0 * Math.PI, "3*pi");
        }

        [TestMethod]
        public void Test_ConstE()
        {
            AsserEval(3.0 * Math.E, "3*e");
        }

        [TestMethod]
        public void Test_Pow()
        {
            AsserEval(4.0, "2^2");
        }

        [TestMethod]
        [Ignore]
        public void Test_ScientificNumber()
        {
            AsserEval(2000, "2e3");
            AsserEval(0.002, "2e-3");
            AsserEval(2000, "2E3");
            AsserEval(0.002, "2E-3");
        }


        [TestMethod]
        public void Test_VariableNotExist()
        {
            var parser = FormulaParser.CreateBuilder().ConfigureValidationBehavior(pb => pb.DisableVariableNameValidation()).Build();

            var node = parser.Parse("2 * X").Value;

            var variables = new Dictionary<string, double>
            {
                ["X"] = 3.0
            };
            Assert.AreEqual(6.0, parser.Evaluate(node, variables));
            var result = parser.Evaluate(node);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Variable 'X' does not exist.", (string)result.Error);
        }

        [TestMethod]
        public void Test_ConstantNotExist()
        {
            var parser = FormulaParser.Create();
            var parser2 = FormulaParser.CreateBuilder().ConfigureConstats(c => c.RemoveAll()).Build();

            var node = parser.Parse("2 * pi").Value;
            parser.Evaluate(node);

            Assert.AreEqual(Math.PI * 2, parser.Evaluate(node));
            var result = parser2.Evaluate(node);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Constant 'pi' does not exist.", (string)result.Error);
        }

        [TestMethod]
        public void Test_NameOfVariableConflictsWithNameOfConstant()
        {
            var parser = FormulaParser.Create();

            var variables = new Dictionary<string, double>
            {
                ["pi"] = 3.0
            };
            var result = parser.Evaluate("2 * pi", variables);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Variable name 'pi' conflicts with the name of an existing constant.", (string)result.Error);
        }

        [TestMethod]
        public void TestComplexFormulas()
        {
            AsserEval(1053542.88171, "(1+1)*5/3.4*2 +32 - 234 * 54 + ((233*4323-23/432 * 234 -34 +32) +3234)/324 * 343 - 3234 + (3-2-4-3) * 5*3*4");
        }

        [TestMethod]
        public void TestSpecialFormulas()
        {
            AsserEval(100, "100");
            AsserEval(-100, "-100");
            AsserEval(-97, "3 + -100");
        }

        [TestMethod]
        public void TestInvalidFormulas()
        {
            AssertFailure("column 90", "(1+1)*5/3.4*2 +32 - 234 * 54 + ((233*4323-23/432 * 234 -34 +32) +3234)/324 * 343 - 3234 + /(3-2-4-3) * 5*3*4");
            AssertFailure("column 2", "1+");

            AssertFailure("column 5", "(1+1))");
        }

        [TestMethod]
        public void TestFunctionMin()
        {
            AsserEval(11, "min(12, 11)");
        }

        [TestMethod]
        public void TestUnrecognizedCharacter()
        {
            AssertFailure("column 3", "1+1$");
        }

        [TestMethod]
        public void TestFunctionFunctionOneValue()
        {
            AsserEval(12.0, "min(12)");
        }

        [TestMethod]
        public void TestRxistingFunctionWithoutParameters()
        {
            AssertFailure("column 0: There is no function 'min' that takes 0 argument(s).", "min()");
        }

        [TestMethod]
        public void TestNotExistingFunction()
        {
            AssertFailure("column 0: Unknown function 'moep'.", "moep()");
        }

        [TestMethod]
        public void TestFunctionMax()
        {
            AsserEval(12, "max(12, 11, 3, 4, 5, 2, 11 ,4)");
        }

        [TestMethod]
        public void TestSignedFunctionMax()
        {
            AsserEval(-12, "-max(12, 11, 3, 4, 5, 2, 11 ,4)");
            AsserEval(12, "+max(12, 11, 3, 4, 5, 2, 11 ,4)");
        }

        [TestMethod]
        public void TestConfigureRegistry()
        {
            var parser = FormulaParser
                .CreateBuilder()
                .ConfigureFunctions(functions =>
                {
                    functions.RemoveAll();
                    functions.Add("sum", args => args.Aggregate((x, y) => x + y));
                    functions.Add("avg", args => args.Average());
                })
                .ConfigureConstats(constants =>
                {
                    constants.RemoveAll();
                    constants.Add("X", 1);
                    constants.Add("Y", 2);
                    constants.Add("Z", 3);
                })
                .Build();
            
            var result = parser.Evaluate("sum(X, Y, Z, 100)");
            Assert.IsTrue(result.Success, string.Join(", ", (string)result.Error));
            Assert.AreEqual(106.0, result.Value);
        }

        [TestMethod]
        public void TestFunctionWithoutParameters()
        {
            var parser = FormulaParser
                .CreateBuilder()
                .ConfigureFunctions(functions =>
                {
                    functions.RemoveAll();
                    functions.Add("get5", () => 5.0);                    
                })
                .Build();

            Assert.AreEqual(5.0, parser.Evaluate("get5()").Value);
            Assert.AreEqual(5.0, parser.Evaluate("get5(   )").Value);
        }

        [TestMethod]
        public void TestDisablePow()
        {
            var parser = FormulaParser
                .CreateBuilder()
                .ConfigureSupportedFeatures(supportedFeatures =>
                {
                    supportedFeatures.DisablePow();
                })
                .Build();

            var result = parser.Evaluate("2^3");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("column 1: Invalid token '^'.", (string)result.Error);
        }

        [TestMethod]
        [Ignore]
        public void TestDisableScientificNotation()
        {
            var parser = FormulaParser
                .CreateBuilder()
                .ConfigureSupportedFeatures(supportedFeatures =>
                {
                    supportedFeatures.DisableScientificNotation();
                })
                .Build();

            var result = parser.Evaluate("2e3");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("column 0: Invalid token '2e3'.", (string)result.Error);
        }

        [TestMethod]
        public void TestDisableVariables()
        {
            var parser = FormulaParser
                .CreateBuilder()
                .ConfigureSupportedFeatures(supportedFeatures =>
                {
                    supportedFeatures.DisableVariables();
                })
                .Build();

            var variables = new Dictionary<string, double>
            {
                ["TEST"] = 3.0
            };
            var result = parser.Evaluate("2 * TEST", variables);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("column 4: Invalid token 'TEST'.", (string)result.Error);
        }

        [TestMethod]
        public void TestDisableFunctions()
        {
            var parser = FormulaParser
                .CreateBuilder()
                .ConfigureSupportedFeatures(supportedFeatures =>
                {
                    supportedFeatures.DisableFunctions();
                })
                .Build();
            var result = parser.Evaluate("2 * max(1,2)");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("column 4: Invalid token 'max'.", (string)result.Error);
        }

        [TestMethod]
        public void TestDisableBracket()
        {
            var parser = FormulaParser
                .CreateBuilder()
                .ConfigureSupportedFeatures(supportedFeatures =>
                {
                    supportedFeatures.DisableBracket();
                })
                .Build();
            var result = parser.Evaluate("2 * (1 + 2)");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("column 4: Invalid token '('.", (string)result.Error);
        }


        private static void AssertFailure(string message, string formular, params string[] variables)
        {
            var variableDict = variables.Select(i => i.Split(':')).ToDictionary(i => i[0], i => double.Parse(i[1]));
            var parsers = new IFormulaParser[] { FormulaParser.Create() };
            foreach (var parser in parsers)
            {
                var parsingResult = parser.Evaluate(formular, variableDict);
                Assert.IsFalse(parsingResult.Success, "Error excepted.");
                Assert.IsTrue(parsingResult.Error.ToString().StartsWith(message), $"Exptected to start with '{message}' but message was: '{parsingResult.Error}'");
            }
        }

        private static void AsserEval(double expectedResult, string formular, params string[] variables)
        {
            var variableDict = variables.Select(i => i.Split(':')).ToDictionary(i => i[0], i => double.Parse(i[1]));
            var parsers = new IFormulaParser[] { FormulaParser.Create() };
            foreach (var parser in parsers)
            {
                var result = parser.Evaluate(formular, variableDict);
                Assert.IsTrue(result.Success, string.Join("; ", (string)result.Error));
                Assert.AreEqual(Math.Round(expectedResult, 5), Math.Round(result.Value, 5));
            }
        }

        private static void AsserEval(double expectedResult, string formular, IFormulaParser formulaParser, params string[] variables)
        {
            var variableDict = variables.Select(i => i.Split(':')).ToDictionary(i => i[0], i => double.Parse(i[1]));
            var parsers = new IFormulaParser[] { FormulaParser.Create(), FormulaParser.CreateBuilder().Build() };
            foreach (var parser in parsers)
            {
                var result = parser.Evaluate(formular, variableDict);
                Assert.IsTrue(result.Success, string.Join("; ", (string)result.Error));
                Assert.AreEqual(Math.Round(expectedResult, 5), Math.Round(result.Value, 5));
            }
        }
    }
}
