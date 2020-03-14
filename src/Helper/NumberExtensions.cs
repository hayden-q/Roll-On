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
			switch (roundingMode)
			{
				case RoundingMode.Round: return Math.Round(number);
				case RoundingMode.Down: return Math.Floor(number);
				case RoundingMode.Up: return Math.Ceiling(number);
				default: return number;
			};
		}
	}
}