using System;
using NUnit.Framework;

namespace Kean.Math.Algebra.Test
{
	public class Parse :
	Kean.Test.Fixture<Parse>
	{
		protected override void Run()
		{
			this.Run(
				this.ConstantZero,
				this.ConstantSingleDigit,
				this.ConstantWithoutDecimals,
				this.ConstantWithDecimals,
				this.ConstantWithSpace,
				this.VariableSingleCharacter,
				this.VariableWithSpace,
				this.VariableMultipleCharacters,
				this.AddingConstants,
				this.AddingConstantsWithoutSpace,
				this.AddingConstantsWithDecimals,
				this.AddingVariables,
				this.AddingVariableConstant,
				this.AddingMultipleTerms,
				this.SubstractingConstants,
				this.SubstractingConstantsWithoutSpace,
				this.SubstractingConstantsWithDecimals,
				this.SubstractingVariables,
				this.SubstractingVariableConstant,
				this.SubstractingMultipleTerms,
				this.SubstractingAddingMultipleTerms,
				this.NegateConstant,
				this.NegateVariable,
				this.NegateFirstTermAddition,
				this.NegateSecondTermAddition,
				this.NegateFirstTermSubstraction,
				this.NegateSecondTermSubstraction,
				this.ParenthesisAroundConstant,
				this.ParenthesisAroundAddition,
				this.ParenthesisWithSubstraction,
				this.ParenthesisWithNegation,
				this.ParenthesisWithinSubstractions,
				this.FunctionConstant,
				this.FunctionVariable,
				this.FunctionAddition,
				this.FunctionAsFirstTerm,
				this.FunctionAsSecondTerm,
				this.FunctionSine,
				this.FunctionCosine,
				this.FunctionSquareRoot,
				this.FunctionLogarithm,
				this.FunctionBinaryLogarithm,
				this.FunctionNaturalLogarithm,
				this.FunctionDecaLogarithm,
				this.Parsing4,
				this.Parsing5,
				this.Parsing6,
				this.Parsing7,
				this.Parsing8,
				this.Parsing9,
				this.Parsing10,
				this.Parsing11,
				this.Parsing12
			);
		}
		#region Constant
		[Test]
		public void ConstantZero()
		{
			var expression = (Expression)"0";
			Verify(expression, Is.EqualTo((Expression)0f));
		}
		[Test]
		public void ConstantSingleDigit()
		{
			var expression = (Expression)"3";
			Verify(expression, Is.EqualTo((Expression)3f));
		}
		[Test]
		public void ConstantWithoutDecimals()
		{
			var expression = (Expression)"42";
			Verify(expression, Is.EqualTo((Expression)42f));
		}
		[Test]
		public void ConstantWithDecimals()
		{
			var expression = (Expression)"13.37";
			Verify(expression, Is.EqualTo((Expression)13.37f));
		}
		[Test]
		public void ConstantWithSpace()
		{
			var expression = (Expression)" 13.37 ";
			Verify(expression, Is.EqualTo((Expression)13.37f));
		}
		[Test]
		public void ConstantE()
		{
			var expression = (Expression)"e";
			Verify(expression, Is.EqualTo(Constant.E));
		}
		[Test]
		public void ConstantPi()
		{
			var expression = (Expression)"pi";
			Verify(expression, Is.EqualTo(Constant.Pi));
		}
		#endregion
		#region Variable
		[Test]
		public void VariableSingleCharacter()
		{
			var expression = (Expression)"x";
			Verify(expression, Is.EqualTo((Expression)"x"));
		}
		[Test]
		public void VariableWithSpace()
		{
			var expression = (Expression)" x ";
			Verify(expression, Is.EqualTo((Expression)"x"));
		}
		[Test]
		public void VariableMultipleCharacters()
		{
			var expression = (Expression)"variable";
			Verify(expression, Is.EqualTo((Expression)"variable"));
		}
		#endregion
		#region Adding
		[Test]
		public void AddingConstants()
		{
			var expression = (Expression)"2 + 3";
			Verify(expression, Is.EqualTo((Expression)2 + 3));
		}
		[Test]
		public void AddingConstantsWithoutSpace()
		{
			var expression = (Expression)"2+3";
			Verify(expression, Is.EqualTo((Expression)2 + 3));
		}
		[Test]
		public void AddingConstantsWithDecimals()
		{
			var expression = (Expression)"4.2 + 13.37";
			Verify(expression, Is.EqualTo((Expression)4.2f + 13.37f));
		}
		[Test]
		public void AddingVariables()
		{
			var expression = (Expression)"x + y";
			Verify(expression, Is.EqualTo(Variable.Create("x") + Variable.Create("y")));
		}
		[Test]
		public void AddingVariableConstant()
		{
			var expression = (Expression)"x + 2";
			Verify(expression, Is.EqualTo(Variable.Create("x") + 2));
		}
		[Test]
		public void AddingMultipleTerms()
		{
			var expression = (Expression)"1 + 2 + 3 + 4 + 5 + 6";
			Verify(expression, Is.EqualTo((Expression)1 + 2 + 3 + 4 + 5 + 6));
		}
		#endregion
		#region Substraction
		[Test]
		public void SubstractingConstants()
		{
			var expression = (Expression)"2 - 3";
			Verify(expression, Is.EqualTo((Expression)2 - 3));
		}
		[Test]
		public void SubstractingConstantsWithoutSpace()
		{
			var expression = (Expression)"2-3";
			Verify(expression, Is.EqualTo((Expression)2 - 3));
		}
		[Test]
		public void SubstractingConstantsWithDecimals()
		{
			var expression = (Expression)"4.2 - 13.37";
			Verify(expression, Is.EqualTo((Expression)4.2f - 13.37f));
		}
		[Test]
		public void SubstractingVariables()
		{
			Expression expression = (Expression)"x - y";
			Verify(expression, Is.EqualTo(Variable.Create("x") - Variable.Create("y")));
		}
		[Test]
		public void SubstractingVariableConstant()
		{
			Expression expression = (Expression)"x - 2";
			Verify(expression, Is.EqualTo(Variable.Create("x") - 2));
		}
		[Test]
		public void SubstractingMultipleTerms()
		{
			Expression expression = (Expression)"1 - 2 - 3 - 4 - 5 - 6";
			Verify(expression, Is.EqualTo((Expression)1 - 2 - 3 - 4 - 5 - 6));
		}
		[Test]
		public void SubstractingAddingMultipleTerms()
		{
			Expression expression = (Expression)"1 - 2 + 3 - 4 + 5 - 6";
			Verify(expression, Is.EqualTo((Expression)1 - 2 + 3 - 4 + 5 - 6));
		}
		#endregion
		#region Negation
		[Test]
		public void NegateConstant()
		{
			Expression expression = (Expression)"-13.37";
			Verify(expression, Is.EqualTo(-(Expression)13.37));
		}
		[Test]
		public void NegateVariable()
		{
			Expression expression = (Expression)"-x";
			Verify(expression, Is.EqualTo(-Variable.Create("x")));
		}
		[Test]
		public void NegateFirstTermAddition()
		{
			Expression expression = (Expression)"-42 + 13.37";
			Verify(expression, Is.EqualTo(-(Expression)42 + 13.37f));
		}
		[Test]
		public void NegateSecondTermAddition()
		{
			Expression expression = (Expression)"42 + -13.37";
			Verify(expression, Is.EqualTo((Expression)42 + -(Expression)13.37f));
		}
		[Test]
		public void NegateFirstTermSubstraction()
		{
			Expression expression = (Expression)"-42 - 13.37";
			Verify(expression, Is.EqualTo(-(Expression)42 - 13.37f));
		}
		[Test]
		public void NegateSecondTermSubstraction()
		{
			Expression expression = (Expression)"42 - -13.37";
			Verify(expression, Is.EqualTo((Expression)42 - -(Expression)13.37f));
		}
		#endregion
		#region Parenthesis
		[Test]
		public void ParenthesisAroundConstant()
		{
			Expression expression = (Expression)"(13.37)";
			Verify(expression, Is.EqualTo((Expression)13.37));
		}
		[Test]
		public void ParenthesisAroundAddition()
		{
			Expression expression = (Expression)"(42 + 13.37)";
			Verify(expression, Is.EqualTo((Expression)42 + 13.37f));
		}
		[Test]
		public void ParenthesisWithSubstraction()
		{
			Expression expression = (Expression)"1337 - (42 + 13.37)";
			Verify(expression, Is.EqualTo((Expression)1337 - ((Expression)42 + 13.37f)));
		}
		[Test]
		public void ParenthesisWithNegation()
		{
			Expression expression = (Expression)"-(42 + 13.37)";
			Verify(expression, Is.EqualTo(-((Expression)42 + 13.37f)));
		}
		[Test]
		public void ParenthesisWithinSubstractions()
		{
			Expression expression = (Expression)"4.2 - (42 + 13.37) - 1337";
			Verify(expression, Is.EqualTo((Expression)4.2f - ((Expression)42 + 13.37f) - 1337));
		}
		#endregion
		#region Function
		[Test]
		public void FunctionConstant()
		{
			var expression = (Expression)"sin(13.37)";
			Verify(expression, Is.EqualTo(new Sine(13.37f)));
		}
		[Test]
		public void FunctionVariable()
		{
			var expression = (Expression)"sin(x)";
			Verify(expression, Is.EqualTo(new Sine(Variable.Create("x"))));
		}
		[Test]
		public void FunctionAddition()
		{
			var expression = (Expression)"sin(4.2 + x)";
			Verify(expression, Is.EqualTo(new Sine((Expression)4.2f + Variable.Create("x"))));
		}
		[Test]
		public void FunctionAsFirstTerm()
		{
			var expression = (Expression)"sin(13.37) + 4.2";
			Verify(expression, Is.EqualTo(new Sine(13.37f) + 4.2f));
		}
		[Test]
		public void FunctionAsSecondTerm()
		{
			var expression = (Expression)"13.37 + sin(4.2)";
			Verify(expression, Is.EqualTo((Expression)13.37 + new Sine(4.2f)));
		}
		[Test]
		public void FunctionSine()
		{
			var expression = (Expression)"sin(4.2)";
			Verify(expression, Is.EqualTo(new Sine(4.2f)));
		}
		[Test]
		public void FunctionCosine()
		{
			var expression = (Expression)"cos(4.2)";
			Verify(expression, Is.EqualTo(new Cosine(4.2f)));
		}
		[Test]
		public void FunctionSquareRoot()
		{
			var expression = (Expression)"sqrt(4.2)";
			Verify(expression, Is.EqualTo(new SquareRoot(4.2f)));
		}
		[Test]
		public void FunctionLogarithm()
		{
			var expression = (Expression)"log13.37(4.2)";
			Verify(expression, Is.EqualTo(new Logarithm(13.37f, 4.2f)));
		}
		[Test]
		public void FunctionBinaryLogarithm()
		{
			var expression = (Expression)"lb(4.2)";
			Verify(expression, Is.EqualTo(new Logarithm(2, 4.2f)));
		}
		[Test]
		public void FunctionNaturalLogarithm()
		{
			var expression = (Expression)"ln(4.2)";
			Verify(expression, Is.EqualTo(new Logarithm(Single.E, 4.2f)));
		}
		[Test]
		public void FunctionDecaLogarithm()
		{
			var expression = (Expression)"lg(4.2)";
			Verify(expression, Is.EqualTo(new Logarithm(10, 4.2f)));
		}
		#endregion
		[Test]
		public void Parsing4()
		{
			var expression = (Expression)"x - 2 * x - 3";
			Verify(expression, Is.EqualTo(Variable.Create("x") - 2 * Variable.Create("x") - 3));
		}
		[Test]
		public void Parsing5()
		{
			var expression = (Expression)"x + 2 * x / 3 - y";
			Verify(expression, Is.EqualTo(Variable.Create("x") + 2 * Variable.Create("x") / 3 - Variable.Create("y")));
		}
		[Test]
		public void Parsing6()
		{
			var expression = (Expression)"x + 2 ^ y / 3 - y";
			Verify(expression, Is.EqualTo(Variable.Create("x") + ((Expression)2 ^ Variable.Create("y")) / 3 - Variable.Create("y")));
		}
		[Test]
		public void Parsing7()
		{
			var expression = (Expression)"x + 2 + y / 3 - y";
			Verify(expression, Is.EqualTo(Variable.Create("x") + 2 + Variable.Create("y") / 3 - Variable.Create("y")));
		}
		[Test]
		public void Parsing8()
		{
			var expression = (Expression)"x * 2 - y / 3 / y * 5 / 6 / 65";
			Verify(expression, Is.EqualTo(Variable.Create("x") * 2 - Variable.Create("y") / 3 / Variable.Create("y") * (Expression)5 / (Expression)6 / (Expression)65));
		}
		[Test]
		public void Parsing9()
		{
			var expression = (Expression)"-x + -2 + y / 3 - y";
			Verify(expression, Is.EqualTo(-Variable.Create("x") + -(Expression)2 + Variable.Create("y") / 3 - Variable.Create("y")));
		}
		[Test]
		public void Parsing10()
		{
			var expression = (Expression)"x + (2 + y / 3) - y";
			Verify(expression, Is.EqualTo(Variable.Create("x") + ((Expression)2 + Variable.Create("y") / 3) - Variable.Create("y")));
		}
		[Test]
		public void Parsing11()
		{
			var expression = (Expression)"x + ((-2 + y / 3) - y)";
			Verify(expression, Is.EqualTo(Variable.Create("x") + ((-(Expression)2 + Variable.Create("y") / 3) - Variable.Create("y"))));
		}
		[Test]
		public void Parsing12()
		{
			var expression = (Expression)"x + ((-2 + y / 3) - y) + 2 * y - z";
			Verify(expression, Is.EqualTo(Variable.Create("x") + ((-(Expression)2 + Variable.Create("y") / 3) - Variable.Create("y")) + (Expression)2 * Variable.Create("y") - Variable.Create("z")));
		}
	}
}

