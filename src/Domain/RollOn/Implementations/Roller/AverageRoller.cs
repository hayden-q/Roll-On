using System.Collections.Generic;

namespace RollOn
{
	public class AverageRoller : IRoller
	{
		public IEnumerable<DiceRoll> Roll(int count, int size)
		{
			var average = (size + 1) / 2.0;

			for (var index = 0; index < count; index++)
			{
				yield return new DiceRoll(average, size);
			}
		}
	}
}