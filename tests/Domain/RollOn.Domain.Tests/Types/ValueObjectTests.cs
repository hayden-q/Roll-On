using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class ValueObjectTests
	{
		[Fact]
		public void Equals_NullParameter_ReturnsFalse()
		{
			// Arrange
			const bool expected = false;
			var sut = new ExampleValueObject("First", "Second");

			// Act
			var actual = sut.Equals(null);

			// Assert
			actual.Should().Be(expected);
		}

		[Fact]
		public void Equals_ParameterIsDifferentType_ReturnsFalse()
		{
			// Arrange
			const bool expected = false;
			var parameter = new AnotherValueObject("First", "Second");
			var sut = new ExampleValueObject("First", "Second");

			// Act
			var actual = sut.Equals(parameter);

			// Assert
			actual.Should().Be(expected);
		}

		[Theory]
		[InlineData(null, "Second")]
		[InlineData("First", null)]
		[InlineData("First", "Second")]
		public void Equals_SamePropertiesNull_ReturnsTrue(string first, string second)
		{
			// Arrange  
			const bool expected = true;
			var parameter = new ExampleValueObject(first, second);
			var sut = new ExampleValueObject(first, second);

			// Act
			var actual = sut.Equals(parameter);

			// Assert
			actual.Should().Be(expected);
		}

		[Theory]
		[InlineData("First", "Second", "1st", "Second")]
		[InlineData("First", "Second", "1st", "2nd")]
		[InlineData("First", "Second", "First", "2nd")]
		public void Equals_PropertiesDifferent_ReturnsFalse(string sutFirst, string sutSecond, string paramFirst, string paramSecond)
		{
			// Arrange  
			const bool expected = false;
			var parameter = new ExampleValueObject(paramFirst, paramSecond);
			var sut = new ExampleValueObject(sutFirst, sutSecond);

			// Act
			var actual = sut.Equals(parameter);

			// Assert
			actual.Should().Be(expected);
		}

		[Fact]
		public void Equals_PropertiesSame_ReturnsTrue()
		{
			// Arrange
			const bool expected = true;
			var parameter = new ExampleValueObject("First", "Second");
			var sut = new ExampleValueObject("First", "Second");

			// Act
			var actual = sut.Equals(parameter);

			// Assert
			actual.Should().Be(expected);
		}

		[Fact]
		public void Equals_GetAtomicNull_ReturnsFalse()
		{
			// Arrange
			const bool expected = false;
			var parameter = new AnotherValueObject("First", "Second", true);
			var sut = new AnotherValueObject("First", "Second", false);
			
			// Act
			var actual = sut.Equals(parameter);
			
			// Assert
			actual.Should().Be(expected);
		}

		[Fact]
		public void GetHashCode_ObjectsAreEqual_ReturnsSameHashCode()
		{
			// Arrange
			var parameter = new ExampleValueObject("First", "Second");
			var sut = new ExampleValueObject("First", "Second");

			// Act
			var first = sut.GetHashCode();
			var second = parameter.GetHashCode();

			first.Should().Be(second);
		}

		[Fact]
		public void GetHashCode_ObjectsNotEqual_ReturnsDifferentHashCode()
		{
			// Arrange
			var parameter = new ExampleValueObject("First", "Second");
			var sut = new ExampleValueObject("1st", "Second");

			// Act
			var first = sut.GetHashCode();
			var second = parameter.GetHashCode();

			first.Should().NotBe(second);
		}
	}
}