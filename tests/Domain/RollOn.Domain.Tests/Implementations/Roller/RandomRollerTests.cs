using System.Linq;
using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class RandomRollerTests
	{
		[Fact]
		public void RandomRoller_RollWithoutKeep_ReturnsValidResult()
		{
			// Arrange
			const int max = 6;
			const int count = 3;
			var roller = new RandomRoller(MockHelper.CreateMockedRandom(max).Object);
			var expected = new[]
			{
				new DiceRoll(max, max),
				new DiceRoll(max, max),
				new DiceRoll(max, max),
			};

			// Act
			var actual = roller.Roll(count, max, RoundingMode.Default).ToArray();

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void RandomRoller_RollWithKeep_ReturnsValidResult()
		{
			// Arrange
			const int max = 6;
			const int count = 4;
			const int keep = 3;
			var roller = new RandomRoller(MockHelper.CreateMockedRandom(max).Object);
			var expected = new[]
			{
				new DiceRoll(max, max),
				new DiceRoll(max, max),
				new DiceRoll(max, max),
			};

			// Act
			var actual = roller.Roll(new DieCount(count, keep), max, RoundingMode.Default).ToArray();

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}
	}
}