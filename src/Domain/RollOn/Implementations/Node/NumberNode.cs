using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace RollOn
{
	public class NumberNode : ValueObject, INode
	{
		public double Value { get; }
		
		public NumberNode(double value) => Value = value;

		public DiceResult Evaluate(IRoller roller, RoundingMode roundingMode)
		{
			return new DiceResult(Value, Enumerable.Empty<IEnumerable<DiceRoll>>());
		}

		public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
		
		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Value;
		}
	}
}