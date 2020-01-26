using System.Collections.Generic;

namespace RollOn
{
	public class AverageRoller : IRoller
	{
		public IEnumerable<DiceRoll> Roll(DieCount count, DieSize size, RoundingMode roundingMode)
		{
			var maxIndex = count.Keep ?? count.Count;
			var average = ((size + 1) / 2.0).Round(roundingMode);

			for (var index = 0; index < maxIndex; index++)
			{
				yield return new DiceRoll(average, size);
			}
		}
	}
}