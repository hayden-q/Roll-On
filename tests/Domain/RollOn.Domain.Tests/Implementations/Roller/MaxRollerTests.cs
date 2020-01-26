using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class MaxRollerTests
	{
		[Fact]
		public void MaxRoller_RollWithoutKeep_ReturnsValidResult()
		{
			// Arrange
			var roller = new MaxRoller();
			var expected = new[]
			{
				new DiceRoll(6, 6),
				new DiceRoll(6, 6),
				new DiceRoll(6, 6),
			};

			// Act
			var actual = roller.Roll(3, 6, RoundingMode.Default);

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void MaxRoller_RollWithKeep_ReturnsValidResult()
		{
			// Arrange
			var roller = new MaxRoller();
			var expected = new[]
			{
				new DiceRoll(6, 6),
				new DiceRoll(6, 6),
				new DiceRoll(6, 6),
			};

			// Act
			var actual = roller.Roll(new DieCount(4, 3), 6, RoundingMode.Default);

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}
	}
}