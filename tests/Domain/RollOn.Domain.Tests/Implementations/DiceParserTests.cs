using FluentAssertions;
using Moq;
using Xunit;

namespace RollOn.Tests
{
	public class DiceParserTests
	{
		[Fact]
		public void Parse_ShouldCallTokenizer_WhenCallingParse()
		{
			// Arrange
			const string parameter = "1";
			var tokenizer = new Mock<IExpressionTokenizer>();
			var parser = new DiceParser(tokenizer.Object);

			// Act
			parser.Parse(parameter);

			// Assert
			tokenizer.Verify(x => x.Tokenize(parameter), Times.Once);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		public void Parse_ExpressionIs1_Returns1AsNumberNode(int parameter)
		{
			// Arrange
			var expected = new NumberNode(parameter);
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new[] {new ConstantToken(parameter.ToString()),});
			var parser = new DiceParser(tokenizer.Object);

			// Act
			var expression = parser.Parse(parameter.ToString());

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_1Plus2()
		{
			// Arrange
			const string parameter = "1+2";
			var expected = new AddNode(new NumberNode(1), new NumberNode(2));
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new[] { new ConstantToken(parameter), });
			var parser = new DiceParser(tokenizer.Object);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_2Minus1()
		{
			// Arrange
			const string parameter = "2-1";
			var expected = new SubtractNode(new NumberNode(2), new NumberNode(1));
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new[] { new ConstantToken(parameter), });
			var parser = new DiceParser(tokenizer.Object);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_3Minus4Plus5()
		{
			// Arrange
			const string parameter = "3-4+5";
			var expected = new AddNode(new SubtractNode(new NumberNode(3), new NumberNode(4)), new NumberNode(5));
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new[] { new ConstantToken(parameter), });
			var parser = new DiceParser(tokenizer.Object);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_4Times5()
		{
			// Arrange
			const string parameter = "4*5";
			var expected = new MultiplyNode(new NumberNode(4), new NumberNode(5));
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new[] { new ConstantToken(parameter.ToString()), });
			var parser = new DiceParser(tokenizer.Object);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_3Plus4Times5()
		{
			// Arrange
			const string parameter = "3+4*5";
			var expected = new AddNode(new NumberNode(3), new MultiplyNode(new NumberNode(4), new NumberNode(5)));
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new[] { new ConstantToken(parameter), });
			var parser = new DiceParser(tokenizer.Object);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_1Plus2Times3Minus4Times5()
		{
			// Arrange
			const string parameter = "1+2*3-4*5";
			var expected = new SubtractNode
			(
				new AddNode
				(
					new NumberNode(1),
					new MultiplyNode(new NumberNode(2), new NumberNode(3))
				),
				new MultiplyNode(new NumberNode(4), new NumberNode(5))
			);
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new[] { new ConstantToken(parameter), });
			var parser = new DiceParser(tokenizer.Object);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_1Plus2Times3Over4Times5Minus1Plus2Over3()
		{
			// Arrange
			const string parameter = "1+2*3/4*5-1+2/3";
			var expected = new AddNode
			(
				new SubtractNode
				(
					new AddNode
					(
						new NumberNode(1),
						new MultiplyNode
						(
							new DivideNode
							(
								new MultiplyNode(new NumberNode(2), new NumberNode(3)),
								new NumberNode(4)
							),
							new NumberNode(5)
						)
					),
					new NumberNode(1)
				),
				new DivideNode(new NumberNode(2), new NumberNode(3))
			);
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new[] { new ConstantToken(parameter), });
			var parser = new DiceParser(tokenizer.Object);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_1Plus2Times3Times4Times5Minus1Plus2Times3()
		{
			// Arrange
			const string parameter = "1+2*3*4*5-1+2*3";
			var expected = new AddNode
			(
				new SubtractNode
				(
					new AddNode
					(
						new NumberNode(1),
						new MultiplyNode
						(
							new MultiplyNode
							(
								new MultiplyNode(new NumberNode(2), new NumberNode(3)),
								new NumberNode(4)
							),
							new NumberNode(5)
						)
					),
					new NumberNode(1)
				),
				new MultiplyNode(new NumberNode(2), new NumberNode(3))
			);
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new[] { new ConstantToken(parameter), });
			var parser = new DiceParser(tokenizer.Object);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_1Plus2Times3Minus4Times5_Brackets()
		{
			// Arrange
			const string parameter = "((1+2)*3-4)*5";
			var expected = new MultiplyNode
			(
				new SubtractNode
				(
					new MultiplyNode
					(
						new AddNode(new NumberNode(1), new NumberNode(2)),
						new NumberNode(3)
					),
					new NumberNode(4)
				),
				new NumberNode(5)
			);
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new[] { new ConstantToken(parameter), });
			var parser = new DiceParser(tokenizer.Object);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		//[Fact]
		//public void Parse_KeepOperator_ReturnsDiceNode()
		//{
		//	// Arrange
		//	const string parameter = "4D6K3";
		//	var expected = new DiceNode(new DieCount(4, 3), new NumberNode(6));

		//	// Act
		//	var sut = parser.Parse(parameter);

		//	// Assert
		//	sut.Should().Be(expected);
		//}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_1D8Plus1D6Times2()
		{
			// Arrange
			const string parameter = "1D8+1D6*2";
			var expected = new AddNode
			(
				new DiceNode(new NumberNode(1), new NumberNode(8)),
				new MultiplyNode(new DiceNode(new NumberNode(1), new NumberNode(6)), new NumberNode(2))
			);
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new[] { new ConstantToken(parameter), });
			var parser = new DiceParser(tokenizer.Object);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_2Times3D8()
		{
			// Arrange
			const string parameter = "(2*3)D8";
			var expected = new DiceNode(new MultiplyNode(new NumberNode(2), new NumberNode(3)), new NumberNode(8));
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new[] { new ConstantToken(parameter), });
			var parser = new DiceParser(tokenizer.Object);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_2Times3D8Plus1D6()
		{
			// Arrange
			const string parameter = "(2*3)D8+1D6";
			var expected = new AddNode
			(
				new DiceNode(new MultiplyNode(new NumberNode(2), new NumberNode(3)), new NumberNode(8)),
				new DiceNode(new NumberNode(1), new NumberNode(6))
			);
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new[] { new ConstantToken(parameter), });
			var parser = new DiceParser(tokenizer.Object);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_2Times3D4D8Plus1D6()
		{
			// Arrange
			const string parameter = "((2*3)D4)D8+1D6";
			var expected = new AddNode
			(
				new DiceNode(new DiceNode(new MultiplyNode(new NumberNode(2), new NumberNode(3)), new NumberNode(4)), new NumberNode(8)),
				new DiceNode(new NumberNode(1), new NumberNode(6))
			);
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new[] { new ConstantToken(parameter), });
			var parser = new DiceParser(tokenizer.Object);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_1D8Plus1DTimes2And3()
		{
			// Arrange
			const string parameter = "1D8+1D(2*3)";
			var expected = new AddNode
			(
				new DiceNode(new NumberNode(1), new NumberNode(8)),
				new DiceNode(new NumberNode(1), new MultiplyNode(new NumberNode(2), new NumberNode(3)))
			);
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new[] { new ConstantToken(parameter), });
			var parser = new DiceParser(tokenizer.Object);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_1D8Plus1DTimes2And3D4()
		{
			// Arrange
			const string parameter = "1D8+1D((2*3)D4)";
			var expected = new AddNode
			(
				new DiceNode(new NumberNode(1), new NumberNode(8)),
				new DiceNode(new NumberNode(1), new DiceNode(new MultiplyNode(new NumberNode(2), new NumberNode(3)), new NumberNode(4)))
			);
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new[] { new ConstantToken(parameter), });
			var parser = new DiceParser(tokenizer.Object);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpressionToNodeViaPostfix_2Times3D2Times4Plus1D6()
		{
			// Arrange
			const string parameter = "(2*3)D(2*4)+1D6";
			var expected = new AddNode
			(
				new DiceNode(new MultiplyNode(new NumberNode(2), new NumberNode(3)),
					new MultiplyNode(new NumberNode(2), new NumberNode(4))),
				new DiceNode(new NumberNode(1), new NumberNode(6))
			);
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new[] { new ConstantToken(parameter), });
			var parser = new DiceParser(tokenizer.Object);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.Should().Be(expected);
		}
	}
}