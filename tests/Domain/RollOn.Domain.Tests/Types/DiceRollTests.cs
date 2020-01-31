using System;
using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class DiceRollTests
	{
		[Fact]
		public void DiceRoll_ValueBelowZero_ThrowsException()
		{
			// Arrange
			const int value = -1;
			const int dieSize = 8;

			//Act
			Action action = () => new DiceRoll(value, dieSize);

			// Assert
			action.Should().Throw<InvalidDiceRollException>()
				.WithMessage("Roll cannot be 0 or less.");
		}

		[Fact]
		public void DiceRoll_ValueGreaterThanDieSize_ThrowsException()
		{
			// Arrange
			const int value = 10;
			const int dieSize = 8;

			// Act
			Action action = () => new DiceRoll(value, dieSize);

			// Assert
			action.Should().Throw<InvalidDiceRollException>()
				.WithMessage("Roll cannot be greater than Die Size.");
		}

		[Fact]
		public void DiceRoll_ValueIsValid_PropertyDieSizeGetsSet()
		{
			// Arrange
			const int value = 8;
			const int dieSize = 8;

			var sut = new DiceRoll(value, dieSize);

			// Assert
			sut.Size.Should().Be(dieSize);
		}

		[Fact]
		public void DiceRoll_ValueIsValid_PropertyValueGetsSet()
		{
			// Arrange
			const int value = 8;
			const int dieSize = 8;

			var sut = new DiceRoll(value, dieSize);

			// Assert
			sut.Value.Should().Be(value);
		}

		[Fact]
		public void DiceRoll_ValueIsZero_ThrowsException()
		{
			// Arrange
			const int value = 0;
			const int dieSize = 8;

			// Act
			Action action = () => new DiceRoll(value, dieSize);

			// Assert
			action.Should().Throw<InvalidDiceRollException>()
				.WithMessage("Roll cannot be 0 or less.");
		}
	}
}