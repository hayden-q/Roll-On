using FluentAssertions;
using Moq;
using Xunit;

namespace RollOn.Tests
{
	public class VariableNodeTests
	{
		[Fact]
		public void Evaluate_ValidNode_ReturnsNumber()
		{
			// Arrange
			const string parameter = "FirstVariable";
			const double expected = 1.0;
			var node = new VariableNode(parameter);

			// Act
			var result = node.Evaluate(new MaxRollerStub(), MockHelper.CreateMockedInjector().Object);

			// Assert
			result.Value.Should().Be(expected);
		}

		[Fact]
		public void Evaluate_ValidNode_CallsIVariableInjector()
		{
			// Arrange
			const string parameter = "FirstVariable";
			var node = new VariableNode(parameter);
			var mockedInjector = MockHelper.CreateMockedInjector();

			// Act
			node.Evaluate(new MaxRollerStub(), mockedInjector.Object);

			// Assert
			mockedInjector.Verify(x => x.RetrieveValue(parameter), Times.Once);
		}

		[Fact]
		public void ToString_ValidNode_ReturnsNumber()
		{
			// Arrange
			const string parameter = "FirstParameter";
			var node = new VariableNode(parameter);

			// Act
			var result = node.ToString();

			// Assert
			result.Should().Be(parameter);
		}
	}
}