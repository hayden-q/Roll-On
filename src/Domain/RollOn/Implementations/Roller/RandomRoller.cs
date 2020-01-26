using System.Collections.Generic;
using System.Linq;

namespace RollOn
{
	public class RandomRoller : IRoller
	{
		private readonly IRandom _random;

		public RandomRoller() : this(new ThreadSafeRandom())
		{
		}
		
		public RandomRoller(IRandom random)
		{
			_random = random;
		}
		
		public IEnumerable<DiceRoll> Roll(DieCount count, DieSize size, RoundingMode roundingMode)
		{
			var rolls = new List<DiceRoll>();

			for (var index = 0; index < count.Count; index++)
			{
				rolls.Add(new DiceRoll(_random.Next(1, size.Value + 1), size));
			}

			return rolls
				.OrderByDescending(roll => roll.Value)
				.Take(count.Keep ?? count.Count);
		}
	}
}