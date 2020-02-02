using System;

namespace RollOn
{
	public class DiceExpression : IDiceExpression
	{
		private readonly INode _node;

		public DiceExpression(INode node)
		{
			_node = node ?? throw new ArgumentNullException(nameof(node), "Node must be set.");
		}

		public DiceResult Evaluate(IRoller roller, IVariableInjector variableInjector, RoundingMode roundingMode)
		{
			var result = _node.Evaluate(roller, variableInjector);

			return new DiceResult(result.Value.Round(roundingMode), result.Rolls);
		}

		public override string ToString()
		{
			return _node.ToString();
		}
	}
}