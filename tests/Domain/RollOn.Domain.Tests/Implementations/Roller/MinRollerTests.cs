using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class MinRollerTests
	{
		[Fact]
		public void MinRoller_RollWithoutKeep_ReturnsValidResult()
		{
			// Arrange
			var roller = new MinRoller();
			var expected = new[]
			{
				new DiceRoll(1, 6),
				new DiceRoll(1, 6),
				new DiceRoll(1, 6),
			};

			// Act
			var actual = roller.Roll(3, 6, RoundingMode.Default);

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void MinRoller_RollWithKeep_ReturnsValidResult()
		{
			// Arrange
			var roller = new MinRoller();
			var expected = new[]
			{
				new DiceRoll(1, 6),
				new DiceRoll(1, 6),
				new DiceRoll(1, 6),
			};

			// Act
			var actual = roller.Roll(new DieCount(4, 3), 6, RoundingMode.Default);

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}
	}
}