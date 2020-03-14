using System;
using System.Collections.Generic;
using System.Linq;

namespace RollOn
{
	public class KeepNode : ValueObject, INode
	{
		public IDiceNode Dice { get; }
		public INode Keep { get; }

		public KeepNode(IDiceNode dice, INode keep)
		{
			Dice = dice ?? throw new ArgumentNullException(nameof(dice), "Node must be set.");
			Keep = keep ?? throw new ArgumentNullException(nameof(keep), "Node must be set.");
		}

		public DiceResult Evaluate(IRoller roller, IVariableInjector variableInjector)
		{
			var diceResult = Dice.Evaluate(roller, variableInjector);
			var keepResult = Keep.Evaluate(roller, variableInjector);
			var diceRolls = new List<DiceRoll>(diceResult.Rolls.Last());

			var diceSum = diceRolls.Sum(roll => roll.Value);
			var keepValue = keepResult.Value.RoundDown();

			if (keepValue < 0)
			{
				keepValue = 0;
			}

			var keepRolls = new List<DiceRoll>(diceRolls).Take(keepValue);
			var keepSum = keepRolls.Sum(roll => roll.Value);
			var sum = (diceResult.Value - diceSum) + keepSum;
			var rolls = new List<IEnumerable<DiceRoll>>(diceResult.Rolls);
			rolls.RemoveAt(rolls.Count - 1);
			rolls.Add(keepRolls);

			var result = new DiceResult(sum, rolls);

			return DiceResult.Merge(result, keepResult);
		}

		public override string ToString() => $"{Dice}K{Keep}";

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Dice;
			yield return Keep;
		}
	}
}