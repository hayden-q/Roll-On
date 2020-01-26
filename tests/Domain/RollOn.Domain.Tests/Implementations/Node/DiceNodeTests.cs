using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class DiceNodeTests
	{
		[Fact]
		public void Evaluate_ValidValueWithCountNode_ReturnsValue()
		{
			// Arrange
			var size = new DieSize(6);
			var node = new DiceNode(new NumberNode(3), size);
			const int value = 18;
			var expected = new DiceResult(value, CreateRolls());

			// Act
			var actual = node.Evaluate(new MaxRollerStub(), RoundingMode.Default);

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}
		
		[Fact]
		public void Evaluate_ValidValueWithSizeNode_ReturnsValue()
		{
			// Arrange
			const int count = 3;
			var node = new DiceNode(count, new NumberNode(6));
			const int value = 18;
			var expected = new DiceResult(value, CreateRolls());

			// Act
			var actual = node.Evaluate(new MaxRollerStub(), RoundingMode.Default);

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void Evaluate_ValidValueWithNode_ReturnsValue()
		{
			// Arrange
			var node = new DiceNode(new NumberNode(3), new NumberNode(6));
			const int value = 18;
			var expected = new DiceResult(value, CreateRolls());

			// Act
			var actual = node.Evaluate(new MaxRollerStub(), RoundingMode.Default);

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}
		
		[Fact]
		public void Evaluate_ValidValue_ReturnsValue()
		{
			// Arrange
			const int count = 3;
			var size = new DieSize(6);
			var node = new DiceNode(count, size);
			const int value = 18;
			var expected = new DiceResult(value, CreateRolls());

			// Act
			var actual = node.Evaluate(new MaxRollerStub(), RoundingMode.Default);

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void Evaluate_ValidValue_SummedRollsMatchValue()
		{
			// Arrange
			const int count = 3;
			var size = new DieSize(6);
			var node = new DiceNode(count, size);
			const int expected = 18;

			// Act
			var actual = node.Evaluate(new MaxRollerStub(), RoundingMode.Default);

			// Assert
			actual.Rolls.Sum(x => x.Sum(y => y.Value)).Should().Be(expected);
		}

		[Fact]
		public void ToString_ValidNodeWithoutKeep_ReturnsDiceNotation()
		{
			// Arrange
			var count = new DieCount(3);
			var size = new DieSize(6);
			var node = new DiceNode(count, size);
			const string expected = "3D6";

			// Act
			var actual = node.ToString();

			// Assert
			actual.Should().Be(expected);
		}

		[Fact]
		public void ToString_ValidNodeWithKeep_ReturnsDiceNotation()
		{
			// Arrange
			var count = new DieCount(3, 2);
			var size = new DieSize(6);
			var node = new DiceNode(count, size);
			const string expected = "3D6K2";

			// Act
			var actual = node.ToString();

			// Assert
			actual.Should().Be(expected);
		}

		[Fact]
		public void ToString_ValidNodeWithNodeCount_ReturnsDiceNotation()
		{
			// Arrange
			var size = new DieSize(6);
			var node = new DiceNode(new NumberNode(3), size);
			const string expected = "3D6";

			// Act
			var actual = node.ToString();

			// Assert
			actual.Should().Be(expected);
		}

		[Fact]
		public void ToString_ValidNodeWithNodeSize_ReturnsDiceNotation()
		{
			// Arrange
			var count = new DieCount(3);
			var node = new DiceNode(count, new NumberNode(6));
			const string expected = "3D6";

			// Act
			var actual = node.ToString();

			// Assert
			actual.Should().Be(expected);
		}
		
		private static IEnumerable<IEnumerable<DiceRoll>> CreateRolls()
		{
			return new[]
			{
				new[]
				{
					new DiceRoll(6, new DieSize(6)),
					new DiceRoll(6, new DieSize(6)),
					new DiceRoll(6, new DieSize(6)),
				}
			};
		}
	}
}