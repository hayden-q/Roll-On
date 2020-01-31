using System;
using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class ExpressionValidatorTests
	{
		[Theory]
		[InlineData("1K3")]
		[InlineData("1D8K3+1K3")]
		public void Validate_KeepOperatorNoDiceOperatorPreceding_ThrowsException(string parameter)
		{
			// Arrange
			var validator = new ExpressionValidator();

			// Act
			Action action = () => validator.Validate(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Keep operator must be preceded by the Dice Operator.");
		}

		[Theory]
		[InlineData("1+*2")]
		[InlineData("1+/2")]
		[InlineData("1*/2")]
		[InlineData("1(/2)")]
		public void Validate_ValueHasIllegalSequentialTokens_ThrowsException(string parameter)
		{
			// Arrange
			var validator = new ExpressionValidator();

			// Act
			Action action = () => validator.Validate(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Dice Expression contains tokens which illegally follow one another.");
		}

		[Fact]
		public void Validate_KeepOperatorNoNumberProceeding_ThrowsException()
		{
			// Arrange
			var validator = new ExpressionValidator();
			const string parameter = "1D8K";

			// Act
			Action action = () => validator.Validate(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Keep operator must be proceeded by number.");
		}

		[Fact]
		public void Validate_ValueContainsDiceOperatorWithoutNumberProceeding_ThrowsException()
		{
			// Arrange
			var validator = new ExpressionValidator();
			const string parameter = "1D";

			// Act
			Action action = () => validator.Validate(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Dice operator must be proceeded by number.");
		}

		[Fact]
		public void Validate_ValueHasBracketsNotClosed_ThrowsException()
		{
			// Arrange
			var validator = new ExpressionValidator();
			const string parameter = "1+)(";

			// Act
			Action action = () => validator.Validate(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Expression contains bracket(s) which haven't been closed.");
		}

		[Fact]
		public void Validate_ValueHasEmptyBrackets_ThrowsException()
		{
			// Arrange
			var validator = new ExpressionValidator();
			const string parameter = "1+()";

			// Act
			Action action = () => validator.Validate(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Expression contains empty bracket(s).");
		}

		[Fact]
		public void Validate_ValueHasIllegalCharacters_ThrowsException()
		{
			// Arrange
			var validator = new ExpressionValidator();
			const string parameter = ";1+2";

			//Act
			Action action = () => validator.Validate(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Expression contains illegal character(s).");
		}

		[Fact]
		public void Validate_ValueHasTooManyCloseBrackets_ThrowsException()
		{
			// Arrange
			var validator = new ExpressionValidator();
			const string parameter = "1+2)";

			//Act
			Action action = () => validator.Validate(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Expression contains bracket(s) which haven't been closed.");
		}

		[Fact]
		public void Validate_ValueHasTooManyOpenBrackets_ThrowsException()
		{
			// Arrange
			var validator = new ExpressionValidator();
			const string parameter = "(1+2";

			//Act
			Action action = () => validator.Validate(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Expression has too many open brackets.");
		}

		[Fact]
		public void Validate_ValueIsNull_ThrowsException()
		{
			// Arrange
			var validator = new ExpressionValidator();
			string parameter = null;

			//Act
			Action action = () => validator.Validate(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Expression can't be null or whitespace.");
		}

		[Fact]
		public void Validate_ValueIsWhitespace_ThrowsException()
		{
			// Arrange
			var validator = new ExpressionValidator();
			const string parameter = " ";

			//Act
			Action action = () => validator.Validate(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Expression can't be null or whitespace.");
		}
	}
}