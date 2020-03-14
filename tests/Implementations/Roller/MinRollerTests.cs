using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class MinRollerTests
	{
		[Fact]
		public void MinRoller_Rolls3d6_Returns3Result()
		{
			// Arrange
			const int count = 3;
			const double value = 1;
			const int size = 6;
			var roller = new MinRoller();
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