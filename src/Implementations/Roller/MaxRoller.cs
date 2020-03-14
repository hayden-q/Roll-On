using System.Collections.Generic;

namespace RollOn
{
	public class MaxRoller : IRoller
	{
		public IEnumerable<DiceRoll> Roll(int count, int size)
		{
			for (var index = 0; index < count; index++)
			{
				yield return new DiceRoll(size, size);
			}
		}
	}
}