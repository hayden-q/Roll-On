using System;
using System.Collections.Generic;

namespace RollOn
{
	public class DiceRoll : ValueObject
	{
		public DiceRoll(double value, int size)
		{
			if (value <= 0)
			{
				throw new InvalidDiceRollException("Roll cannot be 0 or less.");
			}

			if (value > size)
			{
				throw new InvalidDiceRollException("Roll cannot be greater than Die Size.");
			}

			Value = value;
			Size = size;
		}

		public double Value { get; }
		public int Size { get; }

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Value;
			yield return Size;
		}
	}
}