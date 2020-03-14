using System;
using FluentAssertions;
using Xunit;

namespace RollOn.Tests
{
	public class VariableInjectorTests
	{
		[Fact]
		public void RegisterVariable_NothingBeenRegistered_RegistersVariable()
		{
			// Arrange
			const string name = "MyVariable";
			const double value = 2.0;
			var reference = new ReferenceValue(() => value);
			var sut = new VariableInjector();

			// Act
			sut.RegisterVariable(name, reference);

			// Assert
			sut.RetrieveValue(name).Should().Be(value);
		}

		[Fact]
		public void RegisterVariable_NameAlreadyRegistered_ThrowsException()
		{
			// Arrange
			const string name = "MyVariable";
			const double value = 2.0;
			var reference = new ReferenceValue(() => value);
			var sut = new VariableInjector();
			sut.RegisterVariable(name, reference);

			// Act
			Action action = () => sut.RegisterVariable(name, new ReferenceValue(() => value));

			// Assert
			action.Should().Throw<InvalidVariableException>()
				.WithMessage($"'{name}' already registered.");
		}

		[Fact]
		public void RetrieveValue_WithVariableRegistered_ReturnsValue()
		{
			// Arrange
			const string name = "MyVariable";
			const double value = 2.0;
			var reference = new ReferenceValue(() => value);
			var sut = new VariableInjector();

			// Act
			sut.RegisterVariable(name, reference);

			// Assert
			sut.RetrieveValue(name).Should().Be(value);
		}

		[Fact]
		public void RetrieveValue_WithVariableNotRegistered_ThrowsException()
		{
			// Arrange
			const string name = "MyVariable";
			const string badName = "YourVariable";
			const double value = 2.0;
			var reference = new ReferenceValue(() => value);
			var sut = new VariableInjector();
			sut.RegisterVariable(name, reference);

			// Act
			Action action = () => sut.RetrieveValue(badName);
			// Assert
			action.Should().Throw<InvalidVariableException>()
				.WithMessage($"'{badName}' doesn't exist.");
		}
	}
}