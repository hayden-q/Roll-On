using System;
using System.Collections.Generic;

namespace RollOn
{
	public class UnaryNode : ValueObject, INode
	{
		public INode Node { get; }

		public UnaryNode(INode node)
		{
			Node = node ?? throw new ArgumentNullException(nameof(node), "Node must be set.");
		}

		public DiceResult Evaluate(IRoller roller, IVariableInjector variableInjector)
		{
			var result = Node.Evaluate(roller, variableInjector);

			return new DiceResult(-result.Value, result.Rolls);
		}

		public override string ToString() => $"-{Node}";

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Node;
		}
	}
}