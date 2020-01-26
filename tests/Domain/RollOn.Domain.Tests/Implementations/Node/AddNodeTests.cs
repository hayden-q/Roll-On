using System;
using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class AddNodeTests
	{
		[Fact]
		public void AddNode_FirstParameterNull_ThrowsException()
		{
			// Arrange
			var parameter = MockHelper.CreateMockedNode(10).Object;
			
			// Act
			Action sut = () => new AddNode(null, parameter);
			
			// Assert
			sut.Should().Throw<ArgumentNullException>()
				.WithMessage("*Node must be set.*");
		}
		
		[Fact]
		public void AddNode_SecondParameterNull_ThrowsException()
		{
			// Arrange
			var parameter = MockHelper.CreateMockedNode(10).Object;
			
			// Act
			Action sut = () => new AddNode(parameter, null);
			
			// Assert
			sut.Should().Throw<ArgumentNullException>()
				.WithMessage("*Node must be set.*");
		}
		
		[Fact]
		public void Evaluate_ValidNode_ReturnsNumber()
		{
			// Arrange
			const int value = 10;
			var valueNode = MockHelper.CreateMockedNode(value).Object;
			var node = new AddNode(valueNode, valueNode);

			// Act
			var result = node.Evaluate(new MaxRollerStub(), RoundingMode.Default);

			// Assert
			result.Value.Should().Be(value + value);
		}

		[Fact]
		public void ToString_ValidNode_ReturnsNumber()
		{
			// Arrange
			const int value = 10;
			var valueNode = MockHelper.CreateMockedNode(value).Object;
			var node = new AddNode(valueNode, valueNode);
			
			// Act
			var result = node.ToString();
			
			// Assert
			result.Should().Be($"{value} + {value}");
		}
	}
}