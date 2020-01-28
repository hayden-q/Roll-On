using System;
using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class DiceParserTests
	{
		[Fact]
		public void Parse_ValueIsNull_ThrowsException()
		{
			// Arrange
			var parser = new DiceParser();
			string parameter = null;

			//Act
			Action action = () => parser.Parse(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Expression can't be null or whitespace.");
		}

		[Fact]
		public void Parse_ValueIsWhitespace_ThrowsException()
		{
			// Arrange
			var parser = new DiceParser();
			const string parameter = " ";

			//Act
			Action action = () => parser.Parse(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Expression can't be null or whitespace.");
		}
		
		[Fact]
		public void Parse_ValueHasIllegalCharacters_ThrowsException()
		{
			// Arrange
			var parser = new DiceParser();
			const string parameter = ";1 + 2";

			//Act
			Action action = () => parser.Parse(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Expression contains illegal character(s).");
		}

		[Fact]
		public void Parse_ValueHasTooManyOpenBrackets_ThrowsException()
		{
			// Arrange
			var parser = new DiceParser();
			const string parameter = "(1 + 2";

			//Act
			Action action = () => parser.Parse(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Expression has too many open brackets.");
		}

		[Fact]
		public void Parse_ValueHasTooManyCloseBrackets_ThrowsException()
		{
			// Arrange
			var parser = new DiceParser();
			const string parameter = "1 + 2)";

			//Act
			Action action = () => parser.Parse(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Expression contains bracket(s) which haven't been closed.");
		}

		[Theory]
		[InlineData("1 +- 2", "1 - 2")]
		[InlineData("1 -+ 2", "1 - 2")]
		public void Parse_ValidSequentualOperators_ValueIsFormatted(string parameter, string expected)
		{
			// Arrange
			var parser = new DiceParser();
			
			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Theory]
		[InlineData("1 +- 2")]
		[InlineData("1 -+ 2")]
		public void Parse_ValidSequentualOperators_NodeIsFormatted(string parameter)
		{
			// Arrange
			var parser = new DiceParser();
			var expected = new SubtractNode(new NumberNode(1), new NumberNode(2));
			
			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}
		
		[Fact]
		public void Parse_ValueContainsDiceOperatorWithoutNumberProceeding_ThrowsException()
		{
			// Arrange
			var parser = new DiceParser();
			const string parameter = "1D";

			// Act
			Action action = () => parser.Parse(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Dice operator must be proceeded by number.");
		}

		[Fact]
		public void Parse_ValidExpressionWithoutNumberInFrontOfDiceOperator_ValueIsFormatted()
		{
			// Arrange
			var parser = new DiceParser();
			const string parameter = "1d8 + d8 + (d6 * 2) + 3d10 + 30";
			const string expected = "1D8 + 1D8 + 1D6 * 2 + 3D10 + 30";

			// Act
			var expression = parser.Parse(parameter).ToString();

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_KeepOperatorNoNumberProceeding_ThrowsException()
		{
			// Arrange
			var parser = new DiceParser();
			const string parameter = "1d8k";

			// Act
			Action action = () => parser.Parse(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Keep operator must be proceeded by number.");
		}

		[Theory]
		[InlineData("1k3")]
		[InlineData("1d8k3 + 1k3")]
		public void Parse_KeepOperatorNoDiceOperatorPreceding_ThrowsException(string parameter)
		{
			// Arrange
			var parser = new DiceParser();

			// Act
			Action action = () => parser.Parse(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Keep operator must be preceded by the Dice Operator.");
		}

		[Theory]
		[InlineData("+1 + 2", "1 + 2")]
		[InlineData("-1 + 2", "1 + 2")]
		[InlineData("*1 + 2", "1 + 2")]
		[InlineData("/1 + 2", "1 + 2")]
		[InlineData("1 + 2 +", "1 + 2")]
		[InlineData("1 + 2 -", "1 + 2")]
		[InlineData("1 + 2 *", "1 + 2")]
		[InlineData("1 + 2 /", "1 + 2")]
		public void Parse_ValueHasOperatorsAtStart_ValueIsFormatted(string parameter, string expected)
		{
			// Arrange
			var parser = new DiceParser();

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Theory]
		[InlineData("1 +* 2")]
		[InlineData("1 +/ 2")]
		[InlineData("1 */ 2")]
		[InlineData("1 (/2)")]
		public void Parse_ValueHasIllegalSequentialTokens_ThrowsException(string parameter)
		{
			// Arrange
			var parser = new DiceParser();
			
			// Act
			Action action = () => parser.Parse(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Dice Expression contains tokens which illegally follow one another.");
		}

		[Fact]
		public void Parse_ValueHasEmptyBrackets_ThrowsException()
		{
			// Arrange
			var parser = new DiceParser();
			const string parameter = "1 + ()";

			// Act
			Action action = () => parser.Parse(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Expression contains empty bracket(s).");
		}

		[Fact]
		public void Parse_ValueHasBracketsNotClosed_ThrowsException()
		{
			// Arrange
			var parser = new DiceParser();
			const string parameter = "1 + )(";

			// Act
			Action action = () => parser.Parse(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Expression contains bracket(s) which haven't been closed.");
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_1Plus2()
		{
			// Arrange
			var parser = new DiceParser();
			var parameter = "1 + 2";
			var expected = new AddNode(new NumberNode(1), new NumberNode(2));

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_2Minus1()
		{
			// Arrange
			var parser = new DiceParser();
			var parameter = "2 - 1";
			var expected = new SubtractNode(new NumberNode(2), new NumberNode(1));

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_3Minus4Plus5()
		{
			// Arrange
			var parser = new DiceParser();
			var parameter = "3 - 4 + 5";
			var expected = new AddNode(new SubtractNode(new NumberNode(3), new NumberNode(4)), new NumberNode(5));

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_4Times5()
		{
			// Arrange
			var parser = new DiceParser();
			var parameter = "4 * 5";
			var expected = new MultiplyNode(new NumberNode(4), new NumberNode(5));

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_3Plus4Times5()
		{
			// Arrange
			var parser = new DiceParser();
			var parameter = "3 + 4 * 5";
			var expected = new AddNode(new NumberNode(3), new MultiplyNode(new NumberNode(4), new NumberNode(5)));

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_1Plus2Times3Minus4Times5()
		{
			// Arrange
			var parser = new DiceParser();
			var parameter = "1 + 2 * 3 - 4 * 5";
			var expected = new SubtractNode
			(
				new AddNode
				(
					new NumberNode(1), 
					new MultiplyNode(new NumberNode(2), new NumberNode(3))
				), 
				new MultiplyNode(new NumberNode(4), new NumberNode(5))
			);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_1Plus2Times3Times4Times5Minus1Plus2Times3()
		{
			// Arrange
			var parser = new DiceParser();
			var parameter = "1 + 2 * 3 * 4 * 5 - 1 + 2 * 3";
			var expected = new AddNode
			(
				new SubtractNode
				(
					new AddNode
					(
						new NumberNode(1),
						new MultiplyNode
						(
							new MultiplyNode
							(
								new MultiplyNode(new NumberNode(2), new NumberNode(3)),
								new NumberNode(4)
							),
							new NumberNode(5)
						)
					),
					new NumberNode(1)
				),
				new MultiplyNode(new NumberNode(2), new NumberNode(3))
			);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_1Plus2Times3Over4Times5Minus1Plus2Over3()
		{
			// Arrange
			var parser = new DiceParser();
			var parameter = "1 + 2 * 3 / 4 * 5 - 1 + 2 / 3";
			var expected = new AddNode
			(
				new SubtractNode
				(
					new AddNode
					(
						new NumberNode(1),
						new MultiplyNode
						(
							new DivideNode
							(
								new MultiplyNode(new NumberNode(2), new NumberNode(3)),
								new NumberNode(4)
							),
							new NumberNode(5)
						)
					),
					new NumberNode(1)
				),
				new DivideNode(new NumberNode(2), new NumberNode(3))
			);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_1Plus2Times3Minus4Times5_Brackets()
		{
			// Arrange
			var parser = new DiceParser();
			var parameter = "((1 + 2) * 3 - 4) * 5";
			var expected = new MultiplyNode
			(
				new SubtractNode
				(
					new MultiplyNode
					(
						new AddNode(new NumberNode(1), new NumberNode(2)),
						new NumberNode(3)
					),
					new NumberNode(4)
				),
				new NumberNode(5)
			);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}
		
		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_1D8Plus1D6Times2()
		{
			// Arrange
			var parser = new DiceParser();
			var parameter = "1D8 + 1D6 * 2";
			var expected = new AddNode
			(
				new DiceNode(1, 8),
				new MultiplyNode(new DiceNode(1, 6), new NumberNode(2))
			);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_1D8Plus1DTimes2And3()
		{
			// Arrange
			var parser = new DiceParser();
			var parameter = "1D8 + 1D(2 * 3)";
			var expected = new AddNode
			(
				new DiceNode(1, 8),
				new DiceNode(1, new MultiplyNode(new NumberNode(2), new NumberNode(3)))
			);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_1D8Plus1DTimes2And3D4()
		{
			// Arrange
			var parser = new DiceParser();
			var parameter = "1D8 + 1D((2 * 3)D4)";
			var expected = new AddNode
			(
				new DiceNode(1, 8),
				new DiceNode(1, new DiceNode(new MultiplyNode(new NumberNode(2), new NumberNode(3)), new DieSize(4)))
			);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_2Times3D8()
		{
			// Arrange
			var parser = new DiceParser();
			var parameter = "(2 * 3)D8";
			var expected = new DiceNode(new MultiplyNode(new NumberNode(2), new NumberNode(3)), 8);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_2Times3D8Plus1D6()
		{
			// Arrange
			var parser = new DiceParser();
			var parameter = "(2 * 3)D8 + 1D6";
			var expected = new AddNode
			(
				new DiceNode(new MultiplyNode(new NumberNode(2), new NumberNode(3)), 8),
				new DiceNode(1, 6)
			);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_2Times3D4D8Plus1D6()
		{
			// Arrange
			var parser = new DiceParser();
			var parameter = "((2 * 3)D4)D8 + 1D6";
			var expected = new AddNode
			(
				new DiceNode(new DiceNode(new MultiplyNode(new NumberNode(2), new NumberNode(3)), new DieSize(4)), 8),
				new DiceNode(1, 6)
			);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_2Times3D2Times4Plus1D6()
		{
			// Arrange
			var parser = new DiceParser();
			var parameter = "(2 * 3)D(2 * 4) + 1D6";
			var expected = new AddNode
			(
				new DiceNode(new MultiplyNode(new NumberNode(2), new NumberNode(3)), new MultiplyNode(new NumberNode(2), new NumberNode(4))),
				new DiceNode(1, 6)
			);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_KeepOperator_ReturnsDiceNode()
		{
			// Arrange
			var parser = new DiceParser();
			var parameter = "4D6K3";
			var expected = new DiceNode(new DieCount(4, 3), 6);
			
			// Act
			var sut = parser.Parse(parameter);
			
			// Assert
			sut.Should().Be(expected);
		}
	}
}