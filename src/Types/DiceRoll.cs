using System.Collections.Generic;

namespace RollOn
{
	public class DiceRoll : ValueObject
	{
		public DiceRoll(double value, int size)
		{
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