using System.Collections.Generic;
using System.Linq;

namespace RollOn.Tests
{
	public class MaxRollerStub : IRoller
	{
		public IEnumerable<DiceRoll> Roll(int count, int size)
		{
			var results = new List<DiceRoll>();

			for (var index = 0; index < count; index++)
			{
				results.Add(new DiceRoll(size, size));
			}

			return results.OrderByDescending(roll => roll.Value);
		}
	}
}