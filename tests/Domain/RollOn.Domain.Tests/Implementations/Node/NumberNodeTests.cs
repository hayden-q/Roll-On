using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class NumberNodeTests
	{
		[Fact]
		public void Evaluate_ValidNode_ReturnsNumber()
		{
			// Arrange
			const int parameter = 10;
			var node = new NumberNode(parameter);

			// Act
			var result = node.Evaluate(new MaxRollerStub());

			// Assert
			result.Value.Should().Be(parameter);
		}

		[Fact]
		public void ToString_ValidNode_ReturnsNumber()
		{
			// Arrange
			const int parameter = 10;
			var node = new NumberNode(parameter);

			// Act
			var result = node.ToString();

			// Assert
			result.Should().Be(parameter.ToString());
		}
	}
}