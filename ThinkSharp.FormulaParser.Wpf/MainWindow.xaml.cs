using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThinkSharp.FormulaParsing;
using ThinkSharp.FormulaParsing.Ast.Visitors;

namespace FormulaParser.Wpf
{
    using ThinkSharp.FormulaParsing.Ast.Nodes;
    using FormulaParser = ThinkSharp.FormulaParsing.FormulaParser;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IFormulaParser parser = FormulaParser
            .CreateBuilder()
            .ConfigureValidationBehavior(parsingBehavior =>
            {
                parsingBehavior.DisableVariableNameValidation();
            })
            .Build();
        public MainWindow()
        {
            InitializeComponent();
            TestConfiguration();
        }

        private void TestEvaluation()
        {
            // The simples way to create a formula parser is the static method 'Create'. 
            var parser = FormulaParser.Create();

            // Parsing a simple mathematical formula
            var result1 = parser.Evaluate("1+3E2").Value; // result1 = 2.0

            // Usage of variables
            var variables = new Dictionary<string, double> { ["x"] = 2.0 };
            var result2 = parser.Evaluate("1+x", variables).Value; // result2 = 3.0

            // Usage of functions
            var result3 = parser.Evaluate("1+min(3,4)").Value; // result2 = 4.0

            // Handle errors
            var parsingResult = parser.Evaluate("2*?");
            if (!parsingResult.Success) Console.WriteLine(parsingResult.Error); // "column 2: token recognition error at: '?'"
        }

        private void TestParsing()
        {
            // The simples way to create a formula parser is the static method 'Create'. 
            var parser = FormulaParser.Create();
            var variables = new Dictionary<string, double> { ["x"] = 2.0 };

            // Parsing a simple mathematical formula
            var node1 = parser.Parse("1+x", variables).Value;
            // |FormulaNode("1+x")
            // |- Child: BinaryOperatorNode(+)
            //           |- LeftNode:  NumericNode(1.0)
            //           |- RightNode: VariableNode("x")
            var result1 = parser.Evaluate(node1, variables).Value; // result = 3.0


            var node2 = parser.Parse("pi * sqrt(3*x)", variables).Value;
            // |FormulaNode("pi * sqrt(3*x)")
            // |- Child: BinaryOperatorNode(*)
            //           |- LeftNode:  ConstantNode(3.14...)
            //           |- RightNode: FunctionNode("sqrt")
            //                         |- Parameters: [BinaryOperationNode(*)]
            //                                         |- LeftNode:  Numeric(3.0)
            //                                         |- RightNode: VariableNode("x")                       
            var result2 = parser.Evaluate(node2, variables).Value; // result = 7.695...
        }

        private void TestConfiguration()
        {
    var parser = FormulaParser
        .CreateBuilder()
        .ConfigureConstats(constants =>
        {
            constants.Add("h", 6.62607015e-34);
        })
        .ConfigureFunctions(functions =>
        {
            // functions.RemoveAll()    for removing all default functions
            // functions.Remove("sum")  for removing function by name

            // define function with 2 to n number of parameters (typeof(nums) = double[])
            functions.Add("product", nums => nums.Aggregate((p1, p2) => p1 * p2));

            // define functions with specified number of parameters (1-5 parameters are supported)
            functions.Add("celsiusToFarenheit", celsius => celsius * 1.8 + 32);
            functions.Add("fahrenheitToCelsius", farenheit => (farenheit - 32) * 5 / 9);
            functions.Add("p1_plus_p2_plus_p3", (p1, p2, p3) => p1 + p2 + p3);
        }).Build();

    var poolTemperatureInCelsius = parser.Evaluate("celsiusToFarenheit(fahrenheitToCelsius(30))").Value; // poolTemperatureInCelsius = 30
    var result2 = parser.Evaluate("product(2, 2, 2, 2, 2, 2, 2)").Value;    // result2 = 2^6 = 128
    var result3 = parser.Evaluate("p1_plus_p2_plus_p3(1, 2, 3)").Value;     // result3 = 6

    string error1 = parser.Evaluate("celsiusToFarenheit(1, 2)").Error;      // column 0: There is no function 'celsiusToFarenheit' that takes 2 argument(s).
    string error2 = parser.Evaluate("product()").Error;                     // column 0: There is no function 'product' that takes 0 argument(s).
    string error3 = parser.Evaluate("p1_plus_p2_plus_p3(1, 2)").Error;      // column 0: There is no function 'p1_plus_p2_plus_p3' that takes 2 argument(s).
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            var result = parser.Parse(Input.Text);            

            if (!result.Success)
            {
                this.Result.Text = string.Join(Environment.NewLine, result.Error);
                this.Result.Foreground = Brushes.Red;
            }
            else
            {
                var variableNames = result.Value.Visit(new CollectVariableNamesVisitor());
                this.Result.Text = "Variables: " + string.Join("|", variableNames);
                this.Result.Foreground = Brushes.Black;
                var evalResult = parser.Evaluate(result.Value);
                if (!evalResult.Success)
                {
                    this.ResultValue.Text = string.Join(Environment.NewLine, evalResult.Error);
                    this.ResultValue.Foreground = Brushes.Red;
                }
                else
                {
                    this.ResultValue.Text = evalResult.Value.ToString();
                    this.ResultValue.Foreground = Brushes.Black;
                }
            }
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
