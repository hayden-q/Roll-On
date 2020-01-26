using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class ThreadSafeRandomTests
	{
		[Fact]
		public void Next_GeneratingRandomValuesInRange_ValuesDontGoOutsideOfRange()
		{
			// Arrange
			const int iterations = 100;
			var min = 10;
			var max = 0;
			var random = new ThreadSafeRandom();
			
			// Act
			for (var index = 0; index < iterations; index++)
			{
				var result = random.Next(1, 3);

				if (result > max)
				{
					max = result;
				}
				else if (result < min)
				{
					min = result;
				}
			}
			
			// Assert
			min.Should().BeGreaterOrEqualTo(1);
			max.Should().BeLessOrEqualTo(3);
		}
	}
}