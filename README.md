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
* Contants: Use build-in constats (pi, e) or define your own. Constants are available independent of the provided variables.
* Functions: Use build-in functions (rnd, abs, log, sin, cos, min, max...) or define your owns.
* Error Handling: The parser provides an expressive error message in case of invalid formulas.
* Customizable: The parser may be configured to disable features and add custom functions / constants.

## Installation

ThinkSharp.FormulaParser can be installed via [Nuget](https://www.nuget.org/packages/ThinkSharp.FormulaParser)

      Install-Package ThinkSharp.FormulaParser
      
## API Reference

### Evaluation

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

#### Usage

to be continued...
          
## License

ThinkSharp.FormulaParser is released under [The MIT license (MIT)](LICENSE.TXT)

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/JanDotNet/ThinkSharp.FormulaParser/tags). 
    
## Donation
If you like ThinkSharp.FormulaParser and use it in your project(s), feel free to give me a cup of coffee :) 

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=MSBFDUU5UUQZL)
