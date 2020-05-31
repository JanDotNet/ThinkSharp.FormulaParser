using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkSharp.FormulaParsing.Ast.Nodes;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace ThinkSharp.FormulaParsing
{
    [TestClass]
    public class FormularParserConfigurationTest
    {      
        [TestMethod]
        public void TestDefaultConfiguration()
        {
            var parser = FormulaParser.Create();

            Assert.AreEqual(1.0, parser.Evaluate("min(1, 3.4)").Value);
            Assert.AreEqual(3.4, parser.Evaluate("max(1, 3.4)").Value);
            Assert.AreEqual(100.0, parser.Evaluate("max(100, 2, 3, 4 )").Value);
            Assert.AreEqual(15.0, parser.Evaluate("sum(1, 2, 3, 4, 5)").Value);

            Assert.AreEqual(2.0, parser.Evaluate("abs(2)").Value);
            Assert.AreEqual(2.0, parser.Evaluate("abs(-2)").Value);


            Assert.AreEqual(Math.Log(2), parser.Evaluate("ln(2)").Value);
            Assert.AreEqual(Math.Log(2, 3), parser.Evaluate("log(2, 3)").Value);
            Assert.AreEqual(Math.Log10(4), parser.Evaluate("log(4)").Value);
            Assert.AreEqual(Math.Sin(3), parser.Evaluate("sin(3)").Value);
            Assert.AreEqual(Math.Cos(3), parser.Evaluate("cos(3)").Value);
            Assert.AreEqual(Math.Tan(3), parser.Evaluate("tan(3)").Value);
            Assert.AreEqual(Math.Sqrt(3), parser.Evaluate("sqrt(3)").Value);
        }

        [TestMethod]
        public void TestRandom()
        {
            var parser = FormulaParser.Create();

            var rnd1 = parser.Evaluate("rnd()").Value;
            var rnd2 = parser.Evaluate("rnd()").Value;
            
            Assert.IsTrue(rnd1 != rnd2);
            Assert.IsTrue(rnd1 <= 1.0);
            Assert.IsTrue(rnd1 >= 0.0);
            Assert.IsTrue(rnd2 <= 1.0);
            Assert.IsTrue(rnd2 >= 0.0);
        }

        [TestMethod]
        public void TestRemoveFunction()
        {
            var parser = FormulaParser.CreateBuilder()
                .ConfigureFunctions(functions => functions.Remove("min"))
                .Build();

            Assert.AreEqual(2.0, parser.Evaluate("max(1, 2)").Value);
            Assert.IsFalse(parser.Evaluate("min(1, 2)").Success);
        }

        [TestMethod]
        public void TestAddFunction()
        {
            var parser = FormulaParser.CreateBuilder()
                .ConfigureFunctions(functions =>
                {
                    functions.Add("MyFunction0", () => 0);
                    functions.Add("MyFunction1", a => a);
                    functions.Add("MyFunction2", (a, b) => a + b);
                    functions.Add("MyFunction3", (a, b, c) => a + b + c);
                    functions.Add("MyFunction4", (a, b, c, d) => a + b + c + d);
                    functions.Add("MyFunction5", (a, b, c, d, e) => a + b + c + d + e);
                    functions.Add("MyFunctionN", args => args.Aggregate((x, y) => x + y));
                })
                .Build();

            Assert.AreEqual(0.0, parser.Evaluate("MyFunction0()").Value);
            Assert.AreEqual(1.0, parser.Evaluate("MyFunction1(1)").Value);
            Assert.AreEqual(3.0, parser.Evaluate("MyFunction2(1, 2)").Value);
            Assert.AreEqual(6.0, parser.Evaluate("MyFunction3(1, 2, 3)").Value);
            Assert.AreEqual(10.0, parser.Evaluate("MyFunction4(1, 2, 3, 4)").Value);
            Assert.AreEqual(15.0, parser.Evaluate("MyFunction5(1, 2, 3, 4, 5)").Value);
            Assert.AreEqual(55.0, parser.Evaluate("MyFunctionN(1, 2, 3, 4, 5, 6, 7, 8, 9, 10)").Value);
        }

        [TestMethod]
        public void TestRemoveAllFunctions()
        {
            var parser = FormulaParser.CreateBuilder()
                .ConfigureFunctions(functions =>
                {
                    functions.RemoveAll();
                })
                .Build();

            Assert.IsFalse(parser.Evaluate("min(1, 3.4)").Success);
            Assert.IsFalse(parser.Evaluate("abs(2)").Success);
        }

        [TestMethod]
        [ExpectedException(typeof(ParserConfigurationException), "Function with name 'sum' already exists.")]
        public void TestAddFunctionsTwice()
        {
            var parser = FormulaParser.CreateBuilder()
                .ConfigureFunctions(functions =>
                {
                    functions.Add("sum", (a, b) => a + b);
                })
                .Build();
        }

        [TestMethod]
        public void TestSumWithOneArg()
        {
            var parser = FormulaParser.Create();

            Assert.AreEqual(3.0, parser.Evaluate("sum(3)"));
        }

        [TestMethod]
        public void TestAddFunctionsWithLessThan2Args()
        {
            var parser = FormulaParser.CreateBuilder()
                .ConfigureFunctions(functions =>
                {
                    functions.Add("sum", a => a);
                })
                .Build();

            Assert.AreEqual(3.0, parser.Evaluate("sum(3)"));
            Assert.AreEqual(7.0, parser.Evaluate("sum(3, 4)"));
        }


        [TestMethod]
        public void TestConstants()
        {
            var parser = FormulaParser.Create();

            Assert.AreEqual(Math.PI, parser.Evaluate("pi").Value);
            Assert.AreEqual(Math.E, parser.Evaluate("e").Value);
        }

        [TestMethod]
        public void TestRemoveConstants()
        {
            var parser = FormulaParser.CreateBuilder()
                .ConfigureConstats(constants => constants.Remove("e"))
                .Build();

            Assert.AreEqual(Math.PI, parser.Evaluate("pi").Value);
            Assert.IsFalse(parser.Evaluate("e").Success);
        }

        [TestMethod]
        public void TestAddConstants()
        {
            var parser = FormulaParser.CreateBuilder()
                .ConfigureConstats(constants =>
                {
                    constants.Add("MyPi", Math.PI);
                    constants.Add("My3", 3);
                })
                .Build();

            Assert.AreEqual(Math.PI, parser.Evaluate("pi").Value);
            Assert.AreEqual(Math.E, parser.Evaluate("e").Value);
            Assert.AreEqual(Math.PI, parser.Evaluate("MyPi").Value);
            Assert.AreEqual(3.0, parser.Evaluate("My3").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ParserConfigurationException), "Constant with name 'pi' already exists.")]
        public void TestAddExistingConstant()
        {
            var parser = FormulaParser.CreateBuilder()
                .ConfigureConstats(constants =>
                {
                    constants.Add("pi", Math.PI);
                })
                .Build();
        }

        [TestMethod]
        public void TestRemoveAllConstants()
        {
            var parser = FormulaParser.CreateBuilder()
                .ConfigureConstats(constants =>
                {
                    constants.RemoveAll();
                })
                .Build();

            Assert.IsFalse(parser.Evaluate("pi").Success);
            Assert.IsFalse(parser.Evaluate("e").Success);
        }
    }
}
