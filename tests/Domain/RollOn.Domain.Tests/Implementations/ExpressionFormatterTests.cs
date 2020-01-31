using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class ExpressionFormatterTests
	{
		[Theory]
		[InlineData("1 +- 2", "1-2")]
		[InlineData("1 -+ 2", "1-2")]
		public void Format_ValidSequentualOperators_ValueIsFormatted(string parameter, string expected)
		{
			// Arrange
			var formatter = new ExpressionFormatter();

			// Act
			var expression = formatter.Format(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Theory]
		[InlineData("+1 + 2", "1+2")]
		[InlineData("-1 + 2", "1+2")]
		[InlineData("*1 + 2", "1+2")]
		[InlineData("/1 + 2", "1+2")]
		[InlineData("1 + 2 +", "1+2")]
		[InlineData("1 + 2 -", "1+2")]
		[InlineData("1 + 2 *", "1+2")]
		[InlineData("1 + 2 /", "1+2")]
		public void Format_ValueHasOperatorsAtStart_ValueIsFormatted(string parameter, string expected)
		{
			// Arrange
			var formatter = new ExpressionFormatter();

			// Act
			var expression = formatter.Format(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Format_ValidExpressionWithoutNumberInFrontOfDiceOperator_ValueIsFormatted()
		{
			// Arrange
			var formatter = new ExpressionFormatter();
			const string parameter = "1d8 + d8 + (d6 * 2) + 3d10 + 30";
			const string expected = "1D8+1D8+(1D6*2)+3D10+30";

			// Act
			var expression = formatter.Format(parameter);

			// Assert
			expression.Should().Be(expected);
		}
	}
}