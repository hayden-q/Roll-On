using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class StringExtensionsTests
	{
		[Theory]
		[InlineData("Hello World", "HelloWorld")]
		[InlineData("Hello\tWorld", "HelloWorld")]
		[InlineData("Hello    World", "HelloWorld")]
		public void RemoveWhitespace_InputIsNotNull_RemovesWhitespace(string parameter, string expected)
		{
			// Act
			var sut = parameter.RemoveWhitespace();

			// Assert
			sut.Should().Be(expected);
		}

		[Fact]
		public void RemoveWhitespace_InputIsNull_ReturnsNull()
		{
			// Arrange
			string parameter = null;

			// Act
			var sut = parameter.RemoveWhitespace();

			// Assert
			sut.Should().BeNull();
		}
	}
}