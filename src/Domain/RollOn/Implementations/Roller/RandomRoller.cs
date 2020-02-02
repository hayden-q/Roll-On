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

			var countSign = (count < 0) ? -1 : 1;
			var sizeSign = (size < 0) ? -1 : 1;

			for (var index = 0; index < count * countSign; index++)
			{
				rolls.Add(new DiceRoll(sizeSign * _random.Next(1, (size * sizeSign) + 1), size));
			}

			return rolls.OrderByDescending(roll => roll.Value);
		}
	}
}