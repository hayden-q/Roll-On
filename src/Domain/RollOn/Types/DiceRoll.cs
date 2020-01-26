using System;
using System.Collections.Generic;

namespace RollOn
{
	public class DiceRoll : ValueObject
	{
		public double Value { get; }
		public DieSize Size { get; }

		public DiceRoll(double value, DieSize size)
		{
			if (size is null)
			{
				throw new ArgumentNullException(nameof(size), "Die Size must be defined.");
			}

			if (value <= 0)
			{
				throw new InvalidDiceRollException("Roll cannot be 0 or less.");
			}
			if (value > size.Value)
			{
				throw new InvalidDiceRollException("Roll cannot be greater than Die Size.");
			}

			this.Value = value;
			this.Size = size;
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return this.Value;
			yield return this.Size;
		}
	}
}