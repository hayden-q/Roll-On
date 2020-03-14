using System;
using System.Collections.Generic;
using System.Linq;

namespace RollOn
{
	public class OperatorPrecedence : IPrecedence
	{
		private readonly Dictionary<Type, Func<INode, INode, INode>> _nodeMapper;
		private readonly Type[] _operators;

		public OperatorPrecedence(INodeFactory factory, params Type[] operatorTypes)
		{
			_operators = operatorTypes;

			_nodeMapper = new Dictionary<Type, Func<INode, INode, INode>>
			{
				{typeof(AddToken), factory.CreateAdd},
				{typeof(SubtractToken), factory.CreateSubtract},
				{typeof(MultiplyToken), factory.CreateMultiply},
				{typeof(DivideToken), factory.CreateDivide},
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

				if (_operators.Contains(currentOperator))
				{
					tokens.MoveNext();
					rightNode = handler.RecurseToNext(currentLevel, tokens);
				}

				if (rightNode is null)
				{
					return leftNode;
				}

				leftNode = _nodeMapper[currentOperator].Invoke(leftNode, rightNode);
			}

			return leftNode;
		}
	}
}