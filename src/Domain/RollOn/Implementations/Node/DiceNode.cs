using System;
using System.Collections.Generic;
using System.Linq;

namespace RollOn
{
	public class DiceNode : ValueObject, IDiceNode
	{ 
		public INode Count { get; }
		public INode Size { get; }

		public DiceNode(INode count, INode size)
		{
			Count = count ?? throw new ArgumentNullException(nameof(count), "Node must be set.");
			Size = size ?? throw new ArgumentNullException(nameof(size), "Node must be set.");
		}

		public DiceResult Evaluate(IRoller roller, IVariableInjector variableInjector)
		{
			var count = Count.Evaluate(roller, variableInjector);
			var size = Size.Evaluate(roller, variableInjector);
			var countSign = count.Value < 0 ? -1 : 1;

			var rolls = roller
				.Roll(count.Value.Round(RoundingMode.Down), size.Value.Round(RoundingMode.Down))
				.OrderByDescending(roll => roll.Value)
				.ToList();

			IEnumerable<IEnumerable<DiceRoll>> newRolls = new[] {rolls};

			var result = new DiceResult(rolls.Select(roll => roll.Value).Sum() * countSign, newRolls);

			return DiceResult.Merge(count, result, size);
		}

		public override string ToString() => $"{Count}D{Size}";

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Count;
			yield return Size;
		}
	}
}