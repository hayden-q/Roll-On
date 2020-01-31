using System;
using System.Collections.Generic;
using System.Linq;

namespace RollOn
{
	public class DiceNode : ValueObject, INode
	{ 
		public INode Count { get; }
		public INode Size { get; }

		public DiceNode(INode count, INode size)
		{
			Count = count ?? throw new ArgumentNullException(nameof(count), "Node must be set.");
			Size = size ?? throw new ArgumentNullException(nameof(size), "Node must be set.");
		}

		public DiceResult Evaluate(IRoller roller)
		{
			var rolls = roller
				.Roll(Count.Evaluate(roller).Value.Round(RoundingMode.Down), Size.Evaluate(roller).Value.Round(RoundingMode.Down))
				.OrderByDescending(roll => roll.Value)
				.ToList();

			return new DiceResult(rolls.Select(roll => roll.Value).Sum(), new[] {rolls});
		}

		public override string ToString() => $"{Count}D{Size}";

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Count;
			yield return Size;
		}
	}
}