using System.Text;

namespace RollOn
{
	public class ExpressionFormatter : IFormatter<string, string>
	{
		private static readonly char[] _diceTokens = {'D', 'K'};
		private static readonly char[] _operators = {'+', '-', '*', '/'};

		public string Format(string input)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				return input;
			}

			input = input.Trim().ToUpper().RemoveWhitespace().Replace("+-", "-").Replace("-+", "-");

			var tokenBuilder = new StringBuilder();

			for (var index = 0; index < input.Length; index++)
			{
				if (input[index].In(_diceTokens) && !char.IsDigit(input[index - 1]) && input[index - 1] != ')')
				{
					tokenBuilder.Append($"1{input[index]}");
				}

				else if (!index.In(0, input.Length - 1) || !input[index].In(_operators))
				{
					tokenBuilder.Append(input[index]);
				}
			}

			return tokenBuilder.ToString();
		}
	}
}