using System;
using System.Collections.Generic;

namespace RollOn
{
	public class DicePrecedence : IPrecedence
	{
		private readonly Dictionary<Type, Func<INode, INode, INode>> _nodeMapper;

		public DicePrecedence(INodeFactory factory)
		{
			_nodeMapper = new Dictionary<Type, Func<INode, INode, INode>>
			{
				{typeof(DiceToken), factory.CreateDice},
				{typeof(KeepToken), (left, right) => factory.CreateKeep(left as IDiceNode, right)},
			};
		}

		public INode Parse(PrecedenceLevel currentLevel, IPrecedenceHandler handler, IStateEnumerator<Token> tokens)
		{
			var leftNode = handler.RecurseToNext(currentLevel, tokens);

			while (tokens.HasNext)
			{
				INode rightNode = null;
				var currentOperator = tokens.Current?.GetType();

				if (currentOperator is null)
				{
					throw new InvalidDiceExpressionException("Cannot get type of operator");
				}

				if (_nodeMapper.ContainsKey(currentOperator))
				{
					tokens.MoveNext();
					rightNode = handler.RecurseToNext(currentLevel, tokens);
				}

				if (!_nodeMapper.ContainsKey(currentOperator) || rightNode is null)
				{
					return leftNode;
				}

				if (currentOperator == typeof(KeepToken) && !(leftNode is IDiceNode))
				{
					throw new InvalidDiceExpressionException("Keep operator must be preceded by the Dice Operator.");
				}

				leftNode = _nodeMapper[currentOperator].Invoke(leftNode, rightNode);
			}

			return leftNode;
		}
	}
}