using System;

namespace RollOn
{
	public static class StringExtensions
	{
		public static string RemoveWhitespace(this string input)
		{
			if (input is null)
			{
				return null;
			}

			return string.Join(string.Empty, input.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
		}

		public static int? TryParseNullable(this string input)
		{
			try
			{
				return int.Parse(input);
			}
			catch
			{
				return null;
			}
		}
	}
}