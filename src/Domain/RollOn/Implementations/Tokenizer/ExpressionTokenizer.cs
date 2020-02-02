using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RollOn
{
	public class ExpressionTokenizer : IExpressionTokenizer
	{
		private StringReader _reader;
		private readonly Dictionary<char, Action<List<Token>>> _operators;

		public ExpressionTokenizer()
		{
			_operators = new Dictionary<char, Action<List<Token>>>
			{
				{ '+', tokens => {tokens.Add(new AddToken());} },
				{ '-', tokens => {tokens.Add(new SubtractToken());} },
				{ '*', tokens => {tokens.Add(new MultiplyToken());} },
				{ '/', tokens => {tokens.Add(new DivideToken());} },
				{ 'D', tokens => {tokens.Add(new DiceToken());} },
				{ 'd', tokens => {tokens.Add(new DiceToken());} },
				{ 'K', tokens => {tokens.Add(new KeepToken());} },
				{ 'k', tokens => {tokens.Add(new KeepToken());} },
				{ '(', tokens => {tokens.Add(new OpenParenthesisToken());} },
				{ ')', tokens => {tokens.Add(new CloseParenthesisToken());} },
			};
		}

		public IEnumerable<Token> Tokenize(string expression)
		{
			_reader = new StringReader(expression);
			var tokens = new List<Token>();

			while (_reader.Peek() != -1)
			{
				var character = (char)_reader.Peek();

				if (char.IsWhiteSpace(character))
				{
					_reader.Read();
				}
				else if (char.IsDigit(character) || character == '.')
				{
					tokens.Add(new NumberToken(ParseNumber()));
				}
				else if (character == '{')
				{
					tokens.Add(new VariableToken(ParseVariable()));
				}
				else if (_operators.ContainsKey(character))
				{
					_operators[character].Invoke(tokens);
					_reader.Read();
				}
				else
				{
					throw new InvalidDiceExpressionException($"Character '{character}' is invalid.");
				}
			}

			return tokens;
		}

		private string ParseVariable()
		{
			var builder = new StringBuilder();
			var hasClosedBrackets = false;
			
			// Skip {
			_reader.Read();
			var nextCharacter = (char) _reader.Peek();
			hasClosedBrackets = nextCharacter == '}';
			while (!hasClosedBrackets && _reader.Peek() != -1)
			{
				var character = (char) _reader.Read();
				builder.Append(character);
				nextCharacter = (char) _reader.Peek();
				hasClosedBrackets = nextCharacter == '}';
			}

			if (!hasClosedBrackets)
			{
				throw new InvalidDiceExpressionException("Variable name doesn't have closed brackets.");
			}

			if (builder.ToString().Trim() == string.Empty)
			{
				throw new InvalidDiceExpressionException("Variable name cannot be empty");
			}

			// Skip }
			_reader.Read();

			return builder.ToString();
		}

		private string ParseNumber()
		{
			var builder = new StringBuilder();

			char nextCharacter;
			do
			{
				var character = (char) _reader.Read();
				builder.Append(character);
				nextCharacter = (char) _reader.Peek();
			} while(char.IsDigit(nextCharacter) || nextCharacter == '.');

			return builder.ToString();
		}
	}
}