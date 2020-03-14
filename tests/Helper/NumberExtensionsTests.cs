using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class NumberExtensionsTests
	{
		[Theory]
		[InlineData(10.3, 10.3)]
		[InlineData(10.5, 10.5)]
		[InlineData(10.51, 10.51)]
		[InlineData(10.7, 10.7)]
		public void Round_RoundingModeNone_NoRoundingOfValue(double value, double expected)
		{
			// Act
			var sut = value.Round(RoundingMode.None);

			// Assert
			sut.Should().Be(expected);
		}

		[Theory]
		[InlineData(10.3, 10)]
		[InlineData(10.5, 10)]
		[InlineData(10.51, 11)]
		[InlineData(10.7, 11)]
		public void Round_RoundingModeRound_RoundsResult(double value, double expected)
		{
			// Act
			var sut = value.Round(RoundingMode.Round);

			// Assert
			sut.Should().Be(expected);
		}

		[Fact]
		public void Round_RoundingModeDown_FloorsResult()
		{
			// Arrange
			const double value = 10.5;
			const double expected = 10;

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
			const double expected = 11;

			// Act
			var sut = value.Round(RoundingMode.Up);

			// Assert
			sut.Should().Be(expected);
		}
	}
}