using System.Collections.Generic;
using System.Linq;

namespace RollOn
{
	public class ExpressionValidator : IValidator<string>
	{
		private static readonly char[] _operators = {'+', '-', '*', '/'};
		private static readonly char[] _brackets = {'(', ')'};
		private static readonly char[] _diceTokens = {'D', 'K'};

		private static IEnumerable<char> ValidTokens => _operators.Concat(_brackets).Concat(_diceTokens);
		private static IEnumerable<char> ValidOperatorTokens => _operators.Concat(_diceTokens);

		public void Validate(string input)
		{
			ValidateIllegalCharacters(input);
			ValidateBrackets(input);
			ValidateSequentialOperators(input);
			ValidateDiceOperator(input);
		}

		private static void ValidateIllegalCharacters(string input)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				throw new InvalidDiceExpressionException("Expression can't be null or whitespace.");
			}

			if (input.Any(token => !char.IsDigit(token) && !token.In(ValidTokens)))
			{
				throw new InvalidDiceExpressionException("Expression contains illegal character(s).");
			}
		}

		private static void ValidateBrackets(string input)
		{
			if (input.Contains("()"))
			{
				throw new InvalidDiceExpressionException("Expression contains empty bracket(s).");
			}

			var openBrackets = 0;

			foreach (var token in input)
			{
				if (_brackets.Contains(token))
				{
					openBrackets += token == '(' ? 1 : -1;
				}

				if (openBrackets < 0)
				{
					throw new InvalidDiceExpressionException(
						"Expression contains bracket(s) which haven't been closed.");
				}
			}

			if (openBrackets > 0)
			{
				throw new InvalidDiceExpressionException("Expression has too many open brackets.");
			}
		}

		private static void ValidateSequentialOperators(string input)
		{
			var previous = input.First();

			foreach (var current in input.Skip(1))
			{
				if (ValidTokens.Contains(previous) && _operators.Contains(current) &&
					!ValidSequentialBrackets(previous, current))
				{
					throw new InvalidDiceExpressionException(
						"Dice Expression contains tokens which illegally follow one another.");
				}

				previous = current;
			}
		}

		private static void ValidateDiceOperator(string input)
		{
			var diceIndexes = input.AllIndexesOf("D").ToArray();
			var keepIndexes = input.AllIndexesOf("K").ToArray();

			if (diceIndexes.Any(index => index == input.Length - 1))
			{
				throw new InvalidDiceExpressionException("Dice operator must be proceeded by number.");
			}

			foreach (var keepIndex in keepIndexes)
			{
				if (keepIndex == input.Length - 1)
				{
					throw new InvalidDiceExpressionException("Keep operator must be proceeded by number.");
				}

				var diceIndex = diceIndexes.Where(index => index + 1 <= keepIndex).MaxOrNull();

				if (!diceIndex.HasValue ||
					!int.TryParse(input.Substring(diceIndex.Value + 1, keepIndex - diceIndex.Value - 1), out _))
				{
					throw new InvalidDiceExpressionException("Keep operator must be preceded by the Dice operator.");
				}
			}
		}

		private static bool ValidSequentialBrackets(char previous, char current)
		{
			var open = ValidOperatorTokens.Contains(previous) && current == '(' || previous == '(' && current == '(';
			var close = ValidOperatorTokens.Contains(current) && previous == ')' || previous == ')' && current == ')';

			return open || close;
		}
	}
}