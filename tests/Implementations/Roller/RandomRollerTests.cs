using System.Linq;
using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class RandomRollerTests
	{
		[Fact]
		public void RandomRoller_Rolls3d6_ReturnsMockedRandomResult()
		{
			// Arrange
			const int count = 3;
			const int value = 6;
			const int size = 6;
			var roller = new RandomRoller(MockHelper.CreateMockedRandom(value).Object);
			var expected = new[]
			{
				new DiceRoll(value, size),
				new DiceRoll(value, size),
				new DiceRoll(value, size)
			};

			// Act
			var actual = roller.Roll(count, size).ToArray();

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}
	}
}