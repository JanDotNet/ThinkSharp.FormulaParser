//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from FormulaGrammer.g4 by ANTLR 4.7.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="FormulaGrammerParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.2")]
[System.CLSCompliant(false)]
public interface IFormulaGrammerVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="FormulaGrammerParser.formula"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFormula([NotNull] FormulaGrammerParser.FormulaContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FormulaGrammerParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpression([NotNull] FormulaGrammerParser.ExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FormulaGrammerParser.multiplyingExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMultiplyingExpression([NotNull] FormulaGrammerParser.MultiplyingExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FormulaGrammerParser.powExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPowExpression([NotNull] FormulaGrammerParser.PowExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>PlusAtom</c>
	/// labeled alternative in <see cref="FormulaGrammerParser.signedAtom"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPlusAtom([NotNull] FormulaGrammerParser.PlusAtomContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>NegativeAtom</c>
	/// labeled alternative in <see cref="FormulaGrammerParser.signedAtom"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNegativeAtom([NotNull] FormulaGrammerParser.NegativeAtomContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Function</c>
	/// labeled alternative in <see cref="FormulaGrammerParser.signedAtom"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunction([NotNull] FormulaGrammerParser.FunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>UnsignedAtom</c>
	/// labeled alternative in <see cref="FormulaGrammerParser.signedAtom"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUnsignedAtom([NotNull] FormulaGrammerParser.UnsignedAtomContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FormulaGrammerParser.atom"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAtom([NotNull] FormulaGrammerParser.AtomContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ScientificNumber</c>
	/// labeled alternative in <see cref="FormulaGrammerParser.number"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitScientificNumber([NotNull] FormulaGrammerParser.ScientificNumberContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>DecimalNumber</c>
	/// labeled alternative in <see cref="FormulaGrammerParser.number"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDecimalNumber([NotNull] FormulaGrammerParser.DecimalNumberContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>BinaryNumber</c>
	/// labeled alternative in <see cref="FormulaGrammerParser.number"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBinaryNumber([NotNull] FormulaGrammerParser.BinaryNumberContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>HexadecimalNumber</c>
	/// labeled alternative in <see cref="FormulaGrammerParser.number"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitHexadecimalNumber([NotNull] FormulaGrammerParser.HexadecimalNumberContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FormulaGrammerParser.func"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunc([NotNull] FormulaGrammerParser.FuncContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FormulaGrammerParser.variable"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariable([NotNull] FormulaGrammerParser.VariableContext context);
}
