using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class AverageRollerTests
	{
		[Fact]
		public void AverageRoller_Rolls3d6_ReturnsAverageResult()
		{
			// Arrange
			const int count = 3;
			const double value = 3.5;
			const int size = 6;
			var roller = new AverageRoller();
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