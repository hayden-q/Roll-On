using System;
using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class DivideNodeTests
	{
		[Fact]
		public void DivideNode_FirstParameterNull_ThrowsException()
		{
			// Arrange
			var parameter = MockHelper.CreateMockedNode(10).Object;
			
			// Act
			Action sut = () => new DivideNode(null, parameter);
			
			// Assert
			sut.Should().Throw<ArgumentNullException>()
				.WithMessage("*Node must be set.*");
		}
		
		[Fact]
		public void DivideNode_SecondParameterNull_ThrowsException()
		{
			// Arrange
			var parameter = MockHelper.CreateMockedNode(10).Object;
			
			// Act
			Action sut = () => new DivideNode(parameter, null);
			
			// Assert
			sut.Should().Throw<ArgumentNullException>()
				.WithMessage("*Node must be set.*");
		}
		
		[Fact]
		public void Evaluate_ValidNode_ReturnsNumber()
		{
			// Arrange
			const double value = 10;
			var valueNode = MockHelper.CreateMockedNode(value).Object;
			var node = new DivideNode(valueNode, valueNode);

			// Act
			var result = node.Evaluate(new MaxRollerStub(), RoundingMode.Default);

			// Assert
			result.Value.Should().Be(value / value);
		}

		[Fact]
		public void ToString_ValidNode_ReturnsNumber()
		{
			// Arrange
			const int value = 10;
			var valueNode = MockHelper.CreateMockedNode(value).Object;
			var node = new DivideNode(valueNode, valueNode);
			
			// Act
			var result = node.ToString();
			
			// Assert
			result.Should().Be($"{value} / {value}");
		}
	}
}