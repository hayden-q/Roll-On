using System.Collections.Generic;
using System.Linq;

namespace RollOn.Tests
{
	public class MaxRollerStub : IRoller
	{
		
		public IEnumerable<DiceRoll> Roll(DieCount count, DieSize size, RoundingMode roundingMode)
		{
			var results = new List<DiceRoll>();
			
			for (var index = 0; index < count.Count; index++)
			{ 
				results.Add(new DiceRoll(size.Value, size));	
			}

			return results
				.OrderByDescending(roll => roll.Value)
				.Take(count.Keep ?? count.Count);
		}
	}
}