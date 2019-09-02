# ThinkSharp.FormulaParser

[![Build status](https://ci.appveyor.com/api/projects/status/l3aagqmbfmgxwv3t?svg=true)](https://ci.appveyor.com/project/JanDotNet/thinksharp-licensing)
[![NuGet](https://img.shields.io/nuget/v/ThinkSharp.FormulaParser.svg)](https://www.nuget.org/packages/ThinkSharp.FormulaParser/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.txt)
[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=MSBFDUU5UUQZL)

## Introduction

**ThinkSharp.FormulaParser** is a simple library with fluent API for parsing and evaluating mathematical formulas.

Mathematical functions can be parsed to a parsing tree (hereinafter called "parsing") or directly evaluated to a numeric value (hereinafter called "evaluating").

The following features are supported
* Basic Mathematical Operations: +, -, /, *, ^, ( .. ), scientific notation (3e2 = 300)
* Variables: Detect variables on parsing or use a dictionary to provide variables for evaluation.
* Constants: Use build-in constants (pi, e) or define your own. Constants are available independent of the provided variables.
* Functions: Use build-in functions (sqrt, sum, rnd, abs, log, ln, sin, cos, tan, min, max) or define your own.
* Error Handling: The parser provides an expressive error message in case of invalid formulas.
* Customizable: The parser may be configured to disable features and add custom functions / constants.

## Installation

ThinkSharp.FormulaParser can be installed via [Nuget](https://www.nuget.org/packages/ThinkSharp.FormulaParser)

      Install-Package ThinkSharp.FormulaParser
      
## Examples

### Evaluating formulas

    // The simples way to create a formula parser is the static method 'Create'. 
    var parser = FormulaParser.Create();
    
    // Parsing a simple mathematical formula
    var result1 = parser.Evaluate("1+1").Value; // result1 = 2.0
    
    // Usage of variables
    var variables = new Dictionary<string, double> { ["x"] = 2.0 };
    var result2 = parser.Evaluate("1+x", variables).Value; // result2 = 3.0
    
    // Usage of functions
    var result3 = parser.Evaluate("1+min(3,4)").Value; // result2 = 4.0
    
    // Handle errors
    var parsingResult = parser.Evaluate("2*?");
    if (!parsingResult.Success) Console.WriteLine(parsingResult.Error); // "column 2: token recognition error at: '?'"

#### Creating and evaluating a parsing tree

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
    //           |- LeftNode:  ConstantNode("pi")
    //           |- RightNode: FunctionNode("sqrt")
    //                         |- Parameters: [BinaryOperationNode(*)]
    //                                         |- LeftNode:  Numeric(3.0)
    //                                         |- RightNode: VariableNode("x")                       
    var result2 = parser.Evaluate(node2, variables).Value; // result = 7.695...
    
#### Configure custom functions / constants

    var parser = FormulaParser
        .CreateBuilder()
        .ConfigureConstats(constants =>
        {
            // constants.RemoveAll()    for removing all default constants
            // constants.Remove("pi")   for removing constants by name
            
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
          
## License

ThinkSharp.FormulaParser is released under [The MIT license (MIT)](LICENSE.TXT)

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/JanDotNet/ThinkSharp.FormulaParser/tags). 
    
## Donation
If you like ThinkSharp.FormulaParser and use it in your project(s), feel free to give me a cup of coffee :) 

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=MSBFDUU5UUQZL)
