using System.Collections.Generic;

namespace RollOn
{
	public class DieCount : ValueObject
	{
		public int Count { get; }
		public int? Keep { get; }

		public DieCount(int count, int? keep = null)
		{
			Count = count;
			Keep = keep;
		}

		public static implicit operator DieCount(int count)
		{
			return new DieCount(count);
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Count;
			yield return Keep;
		}
	}
}