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

		public DiceResult Evaluate(IRoller roller, RoundingMode roundingMode = RoundingMode.Default)
		{
			return _node.Evaluate(roller, roundingMode);
		}

		public override string ToString()
		{
			return _node.ToString();
		}
	}
}