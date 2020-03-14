using System;
using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class ExpressionTokenizerTests
	{
		[Theory]
		[InlineData("1")]
		[InlineData("2")]
		[InlineData("10")]
		[InlineData("3.14")]
		public void Tokenize_InputIsNumber_ReturnsTokenWithNumber(string expression)
		{
			// Arrange
			var expected = new[] {new NumberToken(expression)};
			var sut = new ExpressionTokenizer();

			// Act
			var result = sut.Tokenize(expression);

			// Assert
			result.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void Tokenize_InputIsAdd_ReturnsAddToken()
		{
			// Arrange
			const string expression = "+";
			var expected = new[] {new AddToken()};
			var sut = new ExpressionTokenizer();

			// Act
			var result = sut.Tokenize(expression);

			// Assert
			result.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void Tokenize_InputIsSubtract_ReturnsSubtractToken()
		{
			// Arrange
			const string expression = "-";
			var expected = new[] { new SubtractToken() };
			var sut = new ExpressionTokenizer();

			// Act
			var result = sut.Tokenize(expression);

			// Assert
			result.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void Tokenize_InputIsMultiply_ReturnsMultiplyToken()
		{
			// Arrange
			const string expression = "*";
			var expected = new[] { new MultiplyToken() };
			var sut = new ExpressionTokenizer();

			// Act
			var result = sut.Tokenize(expression);

			// Assert
			result.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void Tokenize_InputIsDivide_ReturnsDivideToken()
		{
			// Arrange
			const string expression = "/";
			var expected = new[] { new DivideToken() };
			var sut = new ExpressionTokenizer();

			// Act
			var result = sut.Tokenize(expression);

			// Assert
			result.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void Tokenize_InputIsDice_ReturnsDiceToken()
		{
			// Arrange
			const string expression = "D";
			var expected = new[] { new DiceToken() };
			var sut = new ExpressionTokenizer();

			// Act
			var result = sut.Tokenize(expression);

			// Assert
			result.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void Tokenize_InputIsExpression_ReturnsListOfTokens()
		{
			// Arrange
			const string expression = "1+2-3*4.5";
			Token[] expected = 
			{
				new NumberToken("1"),
				new AddToken(), 
				new NumberToken("2"),
				new SubtractToken(), 
				new NumberToken("3"),
				new MultiplyToken(), 
				new NumberToken("4.5"),
			};
			var sut = new ExpressionTokenizer();

			// Act
			var result = sut.Tokenize(expression);

			// Assert
			result.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void Tokenize_InputIsVariable_ReturnsVariableToken()
		{
			// Arrange
			const string expression = "{Variable_Name}";
			var expected = new[] {new VariableToken("Variable_Name"),};
			var sut = new ExpressionTokenizer();

			// Act
			var result = sut.Tokenize(expression);

			// Assert
			result.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void Tokenize_InputIsEmptyVariable_ThrowsException()
		{
			// Arrange
			const string expression = "{}";
			var sut = new ExpressionTokenizer();

			// Act
			Action action = () => sut.Tokenize(expression);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Variable name cannot be empty");
		}

		[Fact]
		public void Tokenize_InputIsUnclosedVariableBracket_ThrowsException()
		{
			// Arrange
			const string expression = "1+{First";
			var sut = new ExpressionTokenizer();

			// Act
			Action action = () => sut.Tokenize(expression);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Variable name doesn't have closed brackets.");
		}

		[Fact]
		public void Tokenize_InputHasInvalidCharacters_ThrowsException()
		{
			// Arrange
			const string expression = "S";
			var sut = new ExpressionTokenizer();

			// Act
			Action action = () => sut.Tokenize(expression);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Character 'S' is invalid.");
		}
	}
}