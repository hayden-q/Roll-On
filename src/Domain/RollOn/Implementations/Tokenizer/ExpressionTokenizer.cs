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
				else if (char.IsDigit(character))
				{
					tokens.Add(new ConstantToken(ParseNumber()));
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