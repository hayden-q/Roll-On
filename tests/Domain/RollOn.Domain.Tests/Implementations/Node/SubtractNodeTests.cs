using System;
using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class SubtractNodeTests
	{
		[Fact]
		public void Evaluate_ValidNode_ReturnsNumber()
		{
			// Arrange
			const int value = 10;
			var valueNode = MockHelper.CreateMockedNode(value).Object;
			var node = new SubtractNode(valueNode, valueNode);

			// Act
			var result = node.Evaluate(new MaxRollerStub());

			// Assert
			result.Value.Should().Be(value - value);
		}

		[Fact]
		public void SubtractNode_FirstParameterNull_ThrowsException()
		{
			// Arrange
			var parameter = MockHelper.CreateMockedNode(10).Object;

			// Act
			Action sut = () => new SubtractNode(null, parameter);

			// Assert
			sut.Should().Throw<ArgumentNullException>()
				.WithMessage("*Node must be set.*");
		}

		[Fact]
		public void SubtractNode_SecondParameterNull_ThrowsException()
		{
			// Arrange
			var parameter = MockHelper.CreateMockedNode(10).Object;

			// Act
			Action sut = () => new SubtractNode(parameter, null);

			// Assert
			sut.Should().Throw<ArgumentNullException>()
				.WithMessage("*Node must be set.*");
		}

		[Fact]
		public void ToString_ValidNode_ReturnsNumber()
		{
			// Arrange
			const int value = 10;
			var valueNode = MockHelper.CreateMockedNode(value).Object;
			var node = new SubtractNode(valueNode, valueNode);

			// Act
			var result = node.ToString();

			// Assert
			result.Should().Be($"{value} - {value}");
		}
	}
}