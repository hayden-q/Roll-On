using System.Collections.Generic;

namespace RollOn
{
	public interface IRoller
	{
		IEnumerable<DiceRoll> Roll(DieCount count, DieSize size, RoundingMode roundingMode);
	}
}