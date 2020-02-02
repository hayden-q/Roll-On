using System.Collections.Generic;
using System.Linq;

namespace RollOn
{
	public class VariableNode : ValueObject, INode
	{ 
		public string Name { get; }

		public VariableNode(string name)
		{
			Name = name;
		}

		public DiceResult Evaluate(IRoller roller, IVariableInjector variableInjector)
		{
			return new DiceResult(variableInjector.RetrieveValue(Name), Enumerable.Empty<IEnumerable<DiceRoll>>());
		}

		public override string ToString() => Name;

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Name;
		}
	}
}