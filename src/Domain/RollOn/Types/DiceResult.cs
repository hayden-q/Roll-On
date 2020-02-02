using System.Collections.Generic;
using System.Linq;

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
			return new DiceResult(first.Value + second.Value, Concatenate(first.Rolls, second.Rolls));
		}

		public static DiceResult operator -(DiceResult first, DiceResult second)
		{
			return new DiceResult(first.Value - second.Value, Concatenate(first.Rolls, second.Rolls));
		}

		public static DiceResult operator *(DiceResult first, DiceResult second)
		{
			return new DiceResult(first.Value * second.Value, Concatenate(first.Rolls, second.Rolls));
		}

		public static DiceResult operator /(DiceResult first, DiceResult second)
		{
			return new DiceResult(first.Value / second.Value, Concatenate(first.Rolls, second.Rolls));
		}

		public static DiceResult Merge(DiceResult count, DiceResult dice, DiceResult size)
		{
			var rolls = Concatenate(count.Rolls, size.Rolls, dice.Rolls);

			return new DiceResult(dice.Value, rolls);
		}

		public static DiceResult Merge(DiceResult dice, DiceResult keep)
		{
			var rolls = Concatenate(keep.Rolls, dice.Rolls);

			return new DiceResult(dice.Value, rolls);
		}

		private static IEnumerable<IEnumerable<DiceRoll>> Concatenate(params IEnumerable<IEnumerable<DiceRoll>>[] rollArray)
		{
			return rollArray?.Where(rolls => rolls != null).SelectMany(rolls => rolls);
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