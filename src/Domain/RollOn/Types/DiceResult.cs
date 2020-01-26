using System;
using System.Collections.Generic;

namespace RollOn
{
	public class DiceResult : ValueObject
	{
		public double Value { get; }
		public IEnumerable<IEnumerable<DiceRoll>> Rolls { get; }
		
		public DiceResult(double value, IEnumerable<IEnumerable<DiceRoll>> rolls)
		{
			Value = value;
			Rolls = rolls;
		}

		public static DiceResult Add(DiceResult first, DiceResult second, RoundingMode roundingMode)
		{
			var value = roundingMode == RoundingMode.Default ? Math.Floor(first.Value + second.Value) : Math.Ceiling(first.Value + second.Value);

			return new DiceResult(value, Concatenate(first, second));
		}

		public static DiceResult Subtract(DiceResult first, DiceResult second, RoundingMode roundingMode)
		{
			var value = roundingMode == RoundingMode.Default ? Math.Floor(first.Value - second.Value) : Math.Ceiling(first.Value - second.Value);

			return new DiceResult(value, Concatenate(first, second));
		}

		public static DiceResult Multiply(DiceResult first, DiceResult second, RoundingMode roundingMode)
		{
			var value = roundingMode == RoundingMode.Default ? Math.Floor(first.Value * second.Value) : Math.Ceiling(first.Value * second.Value);

			return new DiceResult(value, Concatenate(first, second));
		}

		public static DiceResult Divide(DiceResult first, DiceResult second, RoundingMode roundingMode)
		{
			var value = roundingMode == RoundingMode.Default ? Math.Floor(first.Value / second.Value) : Math.Ceiling(first.Value / second.Value);

			return new DiceResult(value, Concatenate(first, second));
		}

		private static IEnumerable<IEnumerable<DiceRoll>> Concatenate(DiceResult first, DiceResult second)
		{
			foreach (var roll in first.Rolls)
			{
				yield return roll;
			}

			foreach (var roll in second.Rolls)
			{
				yield return roll;
			}
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Value;

			foreach (var rollCollection in Rolls)
			{
				foreach (var roll in rollCollection)
				{
					yield return roll;
				}
			}
		}
	}
}