using System.Collections.Generic;

namespace RollOn
{
	public class MaxRoller : IRoller
	{
		public IEnumerable<DiceRoll> Roll(DieCount count, DieSize size, RoundingMode roundingMode)
		{
			var maxIndex = count.Keep ?? count.Count;

			for (int index = 0; index < maxIndex; index++)
			{
				yield return new DiceRoll(size.Value, size);
			}
		}
	}
}