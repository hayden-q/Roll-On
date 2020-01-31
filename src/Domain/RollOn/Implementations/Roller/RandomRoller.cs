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

		public IEnumerable<DiceRoll> Roll(int count, int size)
		{
			var rolls = new List<DiceRoll>();

			for (var index = 0; index < count; index++)
			{
				rolls.Add(new DiceRoll(_random.Next(1, size + 1), size));
			}

			return rolls.OrderByDescending(roll => roll.Value);
		}
	}
}