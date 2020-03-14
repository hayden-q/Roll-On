using System;
using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class UnaryNodeTests
	{
		[Fact]
		public void UnaryNode_ParameterNull_ThrowsException()
		{
			// Arrange
			INode parameter = null;

			// Act
			Action sut = () => new UnaryNode(parameter);

			// Assert
			sut.Should().Throw<ArgumentNullException>()
				.WithMessage("*Node must be set.*");
		}

		[Fact]
		public void Evaluate_ValidNode_ReturnsNegativeNumber()
		{
			// Arrange
			const int value = 10;
			var valueNode = MockHelper.CreateMockedNode(value).Object;
			var node = new UnaryNode(valueNode);

			// Act
			var result = node.Evaluate(new MaxRollerStub(), MockHelper.CreateMockedInjector().Object);

			// Assert
			result.Value.Should().Be(-value);
		}

		[Fact]
		public void ToString_ValidNode_ReturnsNumber()
		{
			// Arrange
			const int value = 10;
			var valueNode = MockHelper.CreateMockedNode(value).Object;
			var node = new UnaryNode(valueNode);

			// Act
			var result = node.ToString();

			// Assert
			result.Should().Be($"-{value}");
		}
	}
}