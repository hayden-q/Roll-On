using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace RollOn.Tests
{
	public class DiceParserTests
	{
		private readonly IPrecedenceHandler _precedenceHandler;

		public DiceParserTests()
		{
			var nodeFactory = new NodeFactoryStub();

			_precedenceHandler = new PrecedenceHandler();
			_precedenceHandler.RegisterPrecedence(new OperatorPrecedence(nodeFactory, typeof(AddToken), typeof(SubtractToken)));
			_precedenceHandler.RegisterPrecedence(new OperatorPrecedence(nodeFactory, typeof(MultiplyToken), typeof(DivideToken)));
			_precedenceHandler.RegisterPrecedence(new DicePrecedence(nodeFactory));
			_precedenceHandler.RegisterPrecedence(new UnaryPrecedence(nodeFactory));
			_precedenceHandler.RegisterPrecedence(new OperandPrecedence(nodeFactory));
		}
		
		[Fact]
		public void Parse_ShouldCallTokenizer_WhenCallingParse()
		{
			// Arrange
			const string parameter = "1";
			var tokenizer = new Mock<IExpressionTokenizer>();
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

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
			var expected = $"{parameter}";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new[]
				{
					new NumberToken(parameter.ToString()),
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			var expression = parser.Parse(parameter.ToString());

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpression_1Plus2()
		{
			// Arrange
			const string parameter = "1+2";
			const string expected = "ADD(1,2)";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new NumberToken("1"), 
					new AddToken(), 
					new NumberToken("2"),
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpression_2Minus1()
		{
			// Arrange
			const string parameter = "2-1";
			const string expected = "SUB(2,1)";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new NumberToken("2"), 
					new SubtractToken(), 
					new NumberToken("1"),
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpression_3Minus4Plus5()
		{
			// Arrange
			const string parameter = "3-4+5";
			const string expected = "ADD(SUB(3,4),5)";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new NumberToken("3"), 
					new SubtractToken(), 
					new NumberToken("4"),
					new AddToken(), 
					new NumberToken("5"), 
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpression_4Times5()
		{
			// Arrange
			const string parameter = "4*5";
			const string expected = "MUL(4,5)";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new NumberToken("4"), 
					new MultiplyToken(), 
					new NumberToken("5"),
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpression_3Plus4Times5()
		{
			// Arrange
			const string parameter = "3+4*5";
			const string expected = "ADD(3,MUL(4,5))";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new NumberToken("3"), 
					new AddToken(), 
					new NumberToken("4"), 
					new MultiplyToken(), 
					new NumberToken("5"),
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpression_1Plus2Times3Minus4Times5()
		{
			// Arrange
			const string parameter = "1+2*3-4*5";
			const string expected = "SUB(ADD(1,MUL(2,3)),MUL(4,5))";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new NumberToken("1"),
					new AddToken(), 
					new NumberToken("2"),
					new MultiplyToken(), 
					new NumberToken("3"), 
					new SubtractToken(),  
					new NumberToken("4"), 
					new MultiplyToken(), 
					new NumberToken("5"),
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpression_1Plus2Times3Over4Times5Minus1Plus2Over3()
		{
			// Arrange
			const string parameter = "1+2*3/4*5-1+2/3";
			const string expected = "ADD(SUB(ADD(1,MUL(DIV(MUL(2,3),4),5)),1),DIV(2,3))";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new NumberToken("1"),
					new AddToken(), 
					new NumberToken("2"),
					new MultiplyToken(), 
					new NumberToken("3"),
					new DivideToken(), 
					new NumberToken("4"),
					new MultiplyToken(), 
					new NumberToken("5"),
					new SubtractToken(), 
					new NumberToken("1"),
					new AddToken(), 
					new NumberToken("2"),
					new DivideToken(), 
					new NumberToken("3"),
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpression_1Plus2Times3Times4Times5Minus1Plus2Times3()
		{
			// Arrange
			const string parameter = "1+2*3*4*5-1+2*3";
			const string expected = "ADD(SUB(ADD(1,MUL(MUL(MUL(2,3),4),5)),1),MUL(2,3))";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new NumberToken("1"),
					new AddToken(), 
					new NumberToken("2"),
					new MultiplyToken(), 
					new NumberToken("3"),
					new MultiplyToken(), 
					new NumberToken("4"),
					new MultiplyToken(), 
					new NumberToken("5"),
					new SubtractToken(), 
					new NumberToken("1"),
					new AddToken(), 
					new NumberToken("2"),
					new MultiplyToken(), 
					new NumberToken("3"),
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpression_1Plus2Times3Minus4Times5_Brackets()
		{
			// Arrange
			const string parameter = "((1+2)*3-4)*5";
			const string expected = "MUL(SUB(MUL(ADD(1,2),3),4),5)";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new OpenParenthesisToken(),
					new OpenParenthesisToken(), 
					new NumberToken("1"),
					new AddToken(), 
					new NumberToken("2"),
					new CloseParenthesisToken(), 
					new MultiplyToken(), 
					new NumberToken("3"),
					new SubtractToken(), 
					new NumberToken("4"),
					new CloseParenthesisToken(), 
					new MultiplyToken(), 
					new NumberToken("5"), 
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Fact]
		public void Parse_KeepOperator_ReturnsDiceNode()
		{
			// Arrange
			const string parameter = "4D6K3";
			const string expected = "KEP(DCE(4,6),3)";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new NumberToken("4"), 
					new DiceToken(), 
					new NumberToken("6"), 
					new KeepToken(),
					new NumberToken("3"), 
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpression_1D8Plus1D6Times2()
		{
			// Arrange
			const string parameter = "1D8+1D6*2";
			const string expected = "ADD(DCE(1,8),MUL(DCE(1,6),2))";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new NumberToken("1"),
					new DiceToken(), 
					new NumberToken("8"),
					new AddToken(), 
					new NumberToken("1"),
					new DiceToken(), 
					new NumberToken("6"),
					new MultiplyToken(), 
					new NumberToken("2"), 
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpression_2Times3D8()
		{
			// Arrange
			const string parameter = "(2*3)D8";
			const string expected = "DCE(MUL(2,3),8)";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new OpenParenthesisToken(),
					new NumberToken("2"),
					new MultiplyToken(), 
					new NumberToken("3"),
					new CloseParenthesisToken(), 
					new DiceToken(), 
					new NumberToken("8"), 
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpression_2Times3D8Plus1D6()
		{
			// Arrange
			const string parameter = "(2*3)D8+1D6";
			const string expected = "ADD(DCE(MUL(2,3),8),DCE(1,6))";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new OpenParenthesisToken(),
					new NumberToken("2"),
					new MultiplyToken(), 
					new NumberToken("3"),
					new CloseParenthesisToken(), 
					new DiceToken(), 
					new NumberToken("8"), 
					new AddToken(), 
					new NumberToken("1"),
					new DiceToken(), 
					new NumberToken("6"), 
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpression_2Times3D4D8Plus1D6()
		{
			// Arrange
			const string parameter = "((2*3)D4)D8+1D6";
			const string expected = "ADD(DCE(DCE(MUL(2,3),4),8),DCE(1,6))";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new OpenParenthesisToken(),
					new OpenParenthesisToken(), 
					new NumberToken("2"),
					new MultiplyToken(), 
					new NumberToken("3"),
					new CloseParenthesisToken(), 
					new DiceToken(),
					new NumberToken("4"), 
					new CloseParenthesisToken(), 
					new DiceToken(), 
					new NumberToken("8"), 
					new AddToken(), 
					new NumberToken("1"),
					new DiceToken(), 
					new NumberToken("6"), 
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpression_1D8Plus1DTimes2And3()
		{
			// Arrange
			const string parameter = "1D8+1D(2*3)";
			const string expected = "ADD(DCE(1,8),DCE(1,MUL(2,3)))";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new NumberToken("1"),
					new DiceToken(), 
					new NumberToken("8"),
					new AddToken(), 
					new NumberToken("1"),
					new DiceToken(), 
					new OpenParenthesisToken(), 
					new NumberToken("2"),
					new MultiplyToken(), 
					new NumberToken("3"),
					new CloseParenthesisToken(), 
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpression_1D8Plus1DTimes2And3D4()
		{
			// Arrange
			const string parameter = "1D8+1D((2*3)D4)";
			const string expected = "ADD(DCE(1,8),DCE(1,DCE(MUL(2,3),4)))";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new NumberToken("1"),
					new DiceToken(), 
					new NumberToken("8"),
					new AddToken(), 
					new NumberToken("1"),
					new DiceToken(), 
					new OpenParenthesisToken(), 
					new OpenParenthesisToken(), 
					new NumberToken("2"),
					new MultiplyToken(), 
					new NumberToken("3"),
					new CloseParenthesisToken(),
					new DiceToken(), 
					new NumberToken("4"),
					new CloseParenthesisToken(), 
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpression_2Times3D2Times4Plus1D6()
		{
			// Arrange
			const string parameter = "(2*3)D(2*4)+1D6";
			const string expected = "ADD(DCE(MUL(2,3),MUL(2,4)),DCE(1,6))";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new OpenParenthesisToken(), 
					new NumberToken("2"),
					new MultiplyToken(), 
					new NumberToken("3"),
					new CloseParenthesisToken(), 
					new DiceToken(), 
					new OpenParenthesisToken(), 
					new NumberToken("2"),
					new MultiplyToken(), 
					new NumberToken("4"),
					new CloseParenthesisToken(), 
					new AddToken(), 
					new NumberToken("1"),
					new DiceToken(), 
					new NumberToken("6"), 
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpression_1D4D6()
		{
			// Arrange
			const string parameter = "1D4D6";
			const string expected = "DCE(DCE(1,4),6)";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new NumberToken("1"),
					new DiceToken(),
					new NumberToken("4"),
					new DiceToken(),
					new NumberToken("6"),
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Fact]
		public void Parse_ParseExpression_10PlusMinusMinus10()
		{
			// Arrange
			const string parameter = "10+--10";
			const string expected = "ADD(10,UNA(UNA(10)))";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new NumberToken("10"),
					new AddToken(),
					new SubtractToken(), 
					new SubtractToken(), 
					new NumberToken("10"), 
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Fact]
		public void Parse_ExpressionHasVariable_10PlusFirstVariableD8()
		{
			// Arrange
			const string parameter = "10+{FirstVariable}D8";
			const string expected = "ADD(10,DCE(VAR:FirstVariable,8))";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new NumberToken("10"),
					new AddToken(),
					new VariableToken("FirstVariable"),
					new DiceToken(), 
					new NumberToken("8"),
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Fact]
		public void Parse_KeepOperatorHasNoDiceOperator_ThrowsException()
		{
			// Arrange
			const string parameter = "1D8K3+1K3";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new NumberToken("1"),
					new KeepToken(), 
					new NumberToken("3"),
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			Action action = () => parser.Parse(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Keep operator must be preceded by the Dice Operator.");
		}

		[Fact]
		public void Parse_ExpressionHasIllegalSequentialTokens_ThrowsException()
		{
			// Arrange
			const string parameter = "1+*3";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new NumberToken("1"),
					new AddToken(), 
					new MultiplyToken(), 
					new NumberToken("3"),
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			Action action = () => parser.Parse(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Unexpected token: *");
		}

		[Fact]
		public void Parse_NoNodeAfterOperator_OnlyReturnsLeftNode()
		{
			// Arrange
			const string parameter = "1+";
			const string expected = "1";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new NumberToken("1"),
					new AddToken(),
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			var expression = parser.Parse(parameter);

			// Assert
			expression.ToString().Should().Be(expected);
		}

		[Fact]
		public void Parse_ExpressionHasBracketsNotClosed_ThrowsException()
		{
			// Arrange
			const string parameter = "1+)(";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new NumberToken("1"),
					new AddToken(),
					new CloseParenthesisToken(), 
					new OpenParenthesisToken(), 
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			Action action = () => parser.Parse(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Unexpected token: )");
		}

		[Fact]
		public void Parse_ExpressionHasEmptyBrackets_ThrowsException()
		{
			// Arrange
			const string parameter = "1+()";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new NumberToken("1"),
					new AddToken(),
					new OpenParenthesisToken(),
					new CloseParenthesisToken(),
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			Action action = () => parser.Parse(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Unexpected token: )");
		}

		[Fact]
		public void Parse_ExpressionHasNoClosedBracket_ThrowsException()
		{
			// Arrange
			const string parameter = "(1+2";
			var tokenizer = new Mock<IExpressionTokenizer>();
			tokenizer
				.Setup(token => token.Tokenize(It.IsAny<string>()))
				.Returns(new Token[]
				{
					new OpenParenthesisToken(),
					new NumberToken("1"),
					new AddToken(),
					new NumberToken("2"),
				});
			var parser = new DiceParser(tokenizer.Object, _precedenceHandler);

			// Act
			Action action = () => parser.Parse(parameter);

			// Assert
			action.Should().Throw<InvalidDiceExpressionException>()
				.WithMessage("Close parenthesis not present");
		}
	}
}