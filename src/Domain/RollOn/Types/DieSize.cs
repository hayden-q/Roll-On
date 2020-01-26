using System.Collections.Generic;

namespace RollOn
{
	public class DieSize : ValueObject
	{
		public int Value { get; }

		public DieSize(int value)
		{
			Value = value;
		}

		public static implicit operator int(DieSize dieSize)
		{
			return dieSize.Value;
		}

		public static implicit operator DieSize(int value)
		{
			return new DieSize(value);
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Value;
		}
	}
}