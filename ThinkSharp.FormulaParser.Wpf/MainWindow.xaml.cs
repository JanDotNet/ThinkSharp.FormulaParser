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
            .ConfigureParsingBehavior(parsingBehavior =>
            {
                parsingBehavior.DisableVariableNameValidation();
            })
            .Build();
        public MainWindow()
        {
            InitializeComponent();
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
