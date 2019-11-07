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
    public class FormularParserVariableNamesTest
    {
        [TestMethod]
        public void TestVariableNames()
        {
            var parser = FormulaParser.CreateBuilder()
                .ConfigureValidationBehavior(v => v.DisableVariableNameValidation())
                .Build();

            Assert.AreEqual(true, parser.Parse("abc").Success);
            Assert.AreEqual(true, parser.Parse("_abc").Success);
            Assert.AreEqual(true, parser.Parse("_a_bc").Success);
            Assert.AreEqual(true, parser.Parse("$abc").Success);
            Assert.AreEqual(true, parser.Parse("$a$bc").Success);
            Assert.AreEqual(true, parser.Parse("$3a3$b534c").Success);

            Assert.AreEqual(false, parser.Parse("1abc").Success);
        }
    }
}
