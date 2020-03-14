using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class KeepNodeTests
	{
		private readonly IRoller _roller;

		public KeepNodeTests()
		{
			_roller = new MaxRollerStub();
		}

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
			var number = new NumberNode(4);
			var size = new NumberNode(6);
			var diceNode = new DiceNode(number, size);
			var keep = new NumberNode(3);
			var node = new KeepNode(diceNode, keep);
			const int value = 18;
			var expected = new DiceResult(value, CreateRolls());

			// Act
			var actual = node.Evaluate(_roller, MockHelper.CreateMockedInjector().Object);

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void Evaluate_ValidValue_SummedRollsMatchValue()
		{
			// Arrange
			var number = new NumberNode(4);
			var size = new NumberNode(6);
			var diceNode = new DiceNode(number, size);
			var keep = new NumberNode(3);
			var node = new KeepNode(diceNode, keep);
			const int expected = 18;

			// Act
			var actual = node.Evaluate(_roller, MockHelper.CreateMockedInjector().Object);

			// Assert
			actual.Rolls.Sum(x => x.Sum(y => y.Value)).Should().Be(expected);
		}

		[Fact]
		public void ToString_WhenUsingNumberNode_ReturnsDiceNotation()
		{
			// Arrange
			var number = new NumberNode(4);
			var size = new NumberNode(6);
			var diceNode = new DiceNode(number, size);
			var keep = new NumberNode(3);
			var node = new KeepNode(diceNode, keep);
			const string expected = "4D6K3";

			// Act
			var actual = node.ToString();

			// Assert
			actual.Should().Be(expected);
		}
	}
}