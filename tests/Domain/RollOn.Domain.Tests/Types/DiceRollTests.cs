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
			var dieSize = new DieSize(8);
			const int roll = -1;

			//Act
			Action action = () => new DiceRoll(roll, dieSize);

			// Assert
			action.Should().Throw<InvalidDiceRollException>()
				.WithMessage("Roll cannot be 0 or less.");
		}

		[Fact]
		public void DiceRoll_ValueIsZero_ThrowsException()
		{
			// Arrange
			var dieSize = new DieSize(8);
			const int roll = 0;

			// Act
			Action action = () => new DiceRoll(roll, dieSize);

			// Assert
			action.Should().Throw<InvalidDiceRollException>()
				.WithMessage("Roll cannot be 0 or less.");
		}

		[Fact]
		public void DiceRoll_DieSizeNull_ThrowsException()
		{
			// Arrange
			const int roll = 6;

			// Act
			Action action = () => new DiceRoll(roll, null);

			// Assert
			action.Should().Throw<ArgumentNullException>()
				.WithMessage("*Die Size must be defined.*")
				.And.ParamName.Should().Be("size");
		}

		[Fact]
		public void DiceRoll_ValueGreaterThanDieSize_ThrowsException()
		{
			// Arrange
			var dieSize = new DieSize(8);
			const int roll = 10;

			// Act
			Action action = () => new DiceRoll(roll, dieSize);

			// Assert
			action.Should().Throw<InvalidDiceRollException>()
				.WithMessage("Roll cannot be greater than Die Size.");
		}

		[Fact]
		public void DiceRoll_ValueIsValid_PropertyValueGetsSet()
		{
			// Arrange
			const int expected = 8;
			var dieSize = new DieSize(expected);

			var sut = new DiceRoll(expected, dieSize);

			// Assert
			sut.Value.Should().Be(expected);
		}

		[Fact]
		public void DiceRoll_ValueIsValid_PropertyDieSizeGetsSet()
		{
			// Arrange
			const int roll = 8;
			var expected = new DieSize(roll);

			var sut = new DiceRoll(roll, expected);

			// Assert
			sut.Size.Should().Be(expected);
		}
	}
}