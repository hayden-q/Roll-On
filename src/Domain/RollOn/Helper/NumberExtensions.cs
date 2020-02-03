using System;

namespace RollOn
{
	public static class NumberExtensions
	{
		public static int RoundDown(this double number)
		{
			return (int) number.Round(RoundingMode.Down);
		}

		public static double Round(this double number, RoundingMode roundingMode = RoundingMode.None)
		{
			return roundingMode switch
			{
				RoundingMode.Round => Math.Round(number),
				RoundingMode.Down => Math.Floor(number),
				RoundingMode.Up => Math.Ceiling(number),
				_ => number,
			};
		}
	}
}