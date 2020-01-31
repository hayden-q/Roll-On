using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class MaxRollerTests
	{
		[Fact]
		public void MaxRoller_Rolls3d6_Returns18Result()
		{
			// Arrange
			const int count = 3;
			const double value = 6;
			const int size = 6;
			var roller = new MaxRoller();
			var expected = new[]
			{
				new DiceRoll(value, size),
				new DiceRoll(value, size),
				new DiceRoll(value, size)
			};

			// Act
			var actual = roller.Roll(count, size);

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}
	}
}