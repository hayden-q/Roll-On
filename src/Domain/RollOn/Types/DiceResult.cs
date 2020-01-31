using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace RollOn
{
	public class DiceResult : ValueObject
	{
		public DiceResult(double value, IEnumerable<IEnumerable<DiceRoll>> rolls)
		{
			Value = value;
			Rolls = rolls;
		}

		public double Value { get; }
		public IEnumerable<IEnumerable<DiceRoll>> Rolls { get; }

		public static DiceResult operator +(DiceResult first, DiceResult second)
		{
			return new DiceResult(first.Value + second.Value, Concatenate(first, second));
		}

		public static DiceResult operator -(DiceResult first, DiceResult second)
		{
			return new DiceResult(first.Value - second.Value, Concatenate(first, second));
		}

		public static DiceResult operator *(DiceResult first, DiceResult second)
		{
			return new DiceResult(first.Value * second.Value, Concatenate(first, second));
		}

		public static DiceResult operator /(DiceResult first, DiceResult second)
		{
			return new DiceResult(first.Value / second.Value, Concatenate(first, second));
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