using System;

namespace RollOn
{
	public static class NumberExtensions
	{
		public static int Round(this double number, RoundingMode roundingMode = RoundingMode.Default)
		{
			return (int) (roundingMode switch
			{
				RoundingMode.Default => Math.Round(number),
				RoundingMode.Down => Math.Floor(number),
				RoundingMode.Up => Math.Ceiling(number),
				_ => throw new NotImplementedException(),
			});
		}
	}
}