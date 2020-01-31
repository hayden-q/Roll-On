using System;
using FluentAssertions;
using Xunit;

namespace RollOn.Tests.Tokenize
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
			var expected = new[] {new ConstantToken(expression)};
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
				new ConstantToken("1"),
				new AddToken(), 
				new ConstantToken("2"),
				new SubtractToken(), 
				new ConstantToken("3"),
				new MultiplyToken(), 
				new ConstantToken("4.5"),
			};
			var sut = new ExpressionTokenizer();

			// Act
			var result = sut.Tokenize(expression);

			// Assert
			result.Should().BeEquivalentTo(expected);
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