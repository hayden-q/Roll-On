using System.Collections.Generic;

namespace RollOn
{
	public class MinRoller : IRoller
	{
		public IEnumerable<DiceRoll> Roll(int count, int size)
		{
			for (var index = 0; index < count; index++)
			{
				yield return new DiceRoll(1, size);
			}
		}
	}
}