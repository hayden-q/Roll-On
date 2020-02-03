using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class DiceExpressionTests
	{
		[Fact]
		public void DiceExpression_NullParameter_ThrowsException()
		{
			// Arrange
			INode node = null;

			// Act
			Action sut = () => new DiceExpression(node);

			// Assert
			sut.Should().Throw<ArgumentNullException>()
				.WithMessage("*Node must be set.*");
		}

		[Fact]
		public void Evaluate_ValidExpression_ReturnsNodeEvaluation()
		{
			// Arrange
			const double value = 10;
			var node = MockHelper.CreateMockedNode(value).Object;
			var expression = new DiceExpression(node);
			var expected = new DiceResult(value, Enumerable.Empty<IEnumerable<DiceRoll>>());

			// Act
			var sut = expression.Evaluate(new MaxRollerStub(), MockHelper.CreateMockedInjector().Object, RoundingMode.None);

			// Assert
			sut.Should().Be(expected);
		}

		[Fact]
		public void ToString_ValidExpression_ReturnsNodeToString()
		{
			// Arrange
			const double value = 10;
			var node = MockHelper.CreateMockedNode(value).Object;
			var expression = new DiceExpression(node);
			var expected = value.ToString(CultureInfo.InvariantCulture);

			// Act
			var sut = expression.ToString();

			// Assert
			sut.Should().Be(expected);
		}
	}
}