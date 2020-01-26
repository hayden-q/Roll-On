using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class NumberExtensionsTests
	{
		[Fact]
		public void Round_RoundingModeDown_FloorsResult()
		{
			// Arrange
			const double value = 10.5;
			const int expected = 10;

			// Act
			var sut = value.Round(RoundingMode.Down);

			// Assert
			sut.Should().Be(expected);
		}

		[Fact]
		public void Round_RoundingModeUp_CeilingResult()
		{
			// Arrange
			const double value = 10.5;
			const int expected = 11;

			// Act
			var sut = value.Round(RoundingMode.Up);

			// Assert
			sut.Should().Be(expected);
		}

		[Theory]
		[InlineData(10.3, 10)]
		[InlineData(10.5, 10)]
		[InlineData(10.51, 11)]
		[InlineData(10.7, 11)]
		public void Round_RoundingModeDefault_RoundsResult(double value, int expected)
		{
			// Act
			var sut = value.Round(RoundingMode.Default);
			
			// Assert
			sut.Should().Be(expected);
		}
	}
}