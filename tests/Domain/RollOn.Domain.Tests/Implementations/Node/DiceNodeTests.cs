using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class DiceNodeTests
	{
		private static IEnumerable<IEnumerable<DiceRoll>> CreateRolls()
		{
			return new[]
			{
				new[]
				{
					new DiceRoll(6, 6),
					new DiceRoll(6, 6),
					new DiceRoll(6, 6)
				}
			};
		}

		[Fact]
		public void Evaluate_ValidValue_ReturnsValue()
		{
			// Arrange
			var number = new NumberNode(3);
			var size = new NumberNode(6);
			var node = new DiceNode(number, size);
			const int value = 18;
			var expected = new DiceResult(value, CreateRolls());

			// Act
			var actual = node.Evaluate(new MaxRollerStub());

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void Evaluate_ValidValue_SummedRollsMatchValue()
		{
			// Arrange
			var number = new NumberNode(3);
			var size = new NumberNode(6);
			var node = new DiceNode(number, size);
			const int expected = 18;

			// Act
			var actual = node.Evaluate(new MaxRollerStub());

			// Assert
			actual.Rolls.Sum(x => x.Sum(y => y.Value)).Should().Be(expected);
		}

		[Fact]
		public void ToString_WhenUsingNumberNode_ReturnsDiceNotation()
		{
			// Arrange
			var number = new NumberNode(3);
			var size = new NumberNode(6);
			var node = new DiceNode(number, size);
			const string expected = "3D6";

			// Act
			var actual = node.ToString();

			// Assert
			actual.Should().Be(expected);
		}
	}
}