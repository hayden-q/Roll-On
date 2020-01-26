using System;
using System.Collections.Generic;
using System.Linq;

namespace RollOn
{
	public class DiceNode : ValueObject, INode
	{
		public DieCount Count { get; }
		public DieSize Size { get; }

		public INode CountNode { get; }
		public INode SizeNode { get; }
	
		public DiceNode(DieCount count, DieSize size)
		{
			this.Count = count;
			this.Size = size;
		}

		public DiceNode(INode count, DieSize size)
		{
			this.CountNode = count;
			this.Size = size;
		}

		public DiceNode(DieCount count, INode size)
		{
			this.Count = count;
			this.SizeNode = size;
		}

		public DiceNode(INode count, INode size)
		{
			this.CountNode = count;
			this.SizeNode = size;
		}

		public DiceResult Evaluate(IRoller roller, RoundingMode roundingMode)
		{
			var count = this.Count ?? this.CountNode.Evaluate(roller, roundingMode).Value.Round(roundingMode);
			var size = this.Size ?? this.SizeNode.Evaluate(roller, roundingMode).Value.Round(roundingMode);

			var rolls = roller.Roll(count, size, roundingMode)
				.OrderByDescending(roll => roll.Value)
				.ToList();

			return new DiceResult(rolls.Select(roll => roll.Value).Sum(), new[] { rolls });
		}

		public override string ToString()
		{
			string count = string.Empty;
			string keep = string.Empty;
			string size = string.Empty;
			
			if (CountNode is null)
			{
				count = Count.Count.ToString();
				keep = Count.Keep.HasValue ? $"K{Count.Keep.Value}" : string.Empty;
			}
			else
			{
				count = CountNode.ToString();
			}

			if (SizeNode is null)
			{
				size = Size.Value.ToString();
			}
			else
			{
				size = SizeNode.ToString();
			}
			
			return $"{count}D{size}{keep}";
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			if (this.Count is null)
			{
				yield return this.CountNode;
			}
			else
			{
				yield return this.Count;

			}

			if (this.Size is null)
			{
				yield return this.SizeNode;
			}
			else
			{
				yield return this.Size;
			}
		}
	}
}