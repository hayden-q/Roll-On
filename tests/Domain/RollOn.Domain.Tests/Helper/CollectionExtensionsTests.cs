using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class CollectionExtensionsTests
	{
		[Theory]
		[InlineData(null, "Hello")]
		[InlineData("1D8", "D", 1)]
		[InlineData("Hello World", "ll", 2)]
		public void AllIndexesOf_ValidValues_ReturnsIndexes(string input, string searchString, params int[] indexes)
		{
			// Act
			var sut = input.AllIndexesOf(searchString);
			
			// Assert
			sut.Should().BeEquivalentTo(indexes);
		}

		[Fact]
		public void PushRange_InputIsNull_PushesNoElements()
		{
			// Arrange
			Stack<int> sut = new Stack<int>();
			
			// Act
			sut.PushRange(null);
			
			// Assert
			sut.Should().BeEmpty();
		}

		[Fact]
		public void PushRange_ValidInput_PushesElementsToStack()
		{
			// Arrange
			var sut = new Stack<int>();
			var inputs = new[] {1, 2, 3, 4, 5};
			
			// Act
			sut.PushRange(inputs);
			
			// Assert
			sut.Should().BeEquivalentTo(inputs.OrderByDescending(x => x));
		}
	}
}