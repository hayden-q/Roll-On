using System;
using System.Collections.Generic;

namespace RollOn
{
	public class MultiplyNode : ValueObject, INode
	{
		public INode First { get; }
		public INode Second { get; }
		
		public MultiplyNode(INode first, INode second)
		{
			First = first ?? throw new ArgumentNullException(nameof(first), "Node must be set.");
			Second = second ?? throw new ArgumentNullException(nameof(second), "Node must be set.");
		}
		
		public DiceResult Evaluate(IRoller roller, RoundingMode roundingMode)
		{
			var firstEval = First.Evaluate(roller, roundingMode);
			var secondEval = Second.Evaluate(roller, roundingMode);
			
			return DiceResult.Multiply(firstEval, secondEval, roundingMode);
		}

		public override string ToString() => $"{First} * {Second}";
		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return First;
			yield return Second;
		}
	}
}