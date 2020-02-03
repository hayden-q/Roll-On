using System.Collections.Generic;

namespace RollOn
{
	public class PrecedenceLevel : ValueObject
	{
		public int Level { get; }

		public PrecedenceLevel(int level)
		{
			Level = level;
		}

		public static implicit operator int(PrecedenceLevel precedenceLevel) => precedenceLevel.Level;

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Level;
		}
	}
}