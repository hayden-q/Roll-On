using System.Collections.Generic;

namespace RollOn
{
	public class MinRoller : IRoller
	{
		public IEnumerable<DiceRoll> Roll(DieCount count, DieSize size, RoundingMode roundingMode)
		{
			var maxIndex = count.Keep ?? count.Count;
			
			for (var index = 0; index < maxIndex; index++)
			{
				yield return new DiceRoll(1, size);
			}
		}
	}
}