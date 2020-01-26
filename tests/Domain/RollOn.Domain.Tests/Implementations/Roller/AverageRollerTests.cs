using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class AverageRollerTests
	{
		[Theory]
		[InlineData(4.0, 0)]
		[InlineData(3.0, 1)]
		[InlineData(4.0, 2)]
		public void AverageRoller_RollWithoutKeep_ReturnsValidResult(double expectedValue, int roundingEnum)
		{
			// Arrange
			var roller = new AverageRoller();
			var expected = new[]
			{
				new DiceRoll(expectedValue, 6),
				new DiceRoll(expectedValue, 6),
				new DiceRoll(expectedValue, 6),
			};

			// Act
			var actual = roller.Roll(3, 6, (RoundingMode)roundingEnum);

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}

		[Theory]
		[InlineData(4.0, 0)]
		[InlineData(3.0, 1)]
		[InlineData(4.0, 2)]
		public void AverageRoller_RollWithKeep_ReturnsValidResult(double expectedValue, int roundingEnum)
		{
			// Arrange
			var roller = new AverageRoller();
			var expected = new[]
			{
				new DiceRoll(expectedValue, 6),
				new DiceRoll(expectedValue, 6),
				new DiceRoll(expectedValue, 6),
			};

			// Act
			var actual = roller.Roll(new DieCount(4, 3), 6, (RoundingMode)roundingEnum);

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}
	}
}