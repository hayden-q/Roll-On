using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace RollOn
{
	public class NumberNode : ValueObject, INode
	{
		public NumberNode(double value)
		{
			Value = value;
		}

		public double Value { get; }

		public DiceResult Evaluate(IRoller roller) => new DiceResult(Value, Enumerable.Empty<IEnumerable<DiceRoll>>());
		public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Value;
		}
	}
}