using System.Collections.Generic;

namespace RollOn
{
	public interface IRoller
	{
		IEnumerable<DiceRoll> Roll(int count, int size);
	}
}