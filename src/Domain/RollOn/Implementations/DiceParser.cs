using System;
using System.Collections.Generic;
using System.Linq;

namespace RollOn
{
	public class DiceParser : IDiceParser
	{
		private bool _hasNext = true;
		private readonly IExpressionTokenizer _tokenizer;
		private readonly Dictionary<Type, Func<INode, INode, INode>> _nodeMapper;
		
		public DiceParser() : this(new ExpressionTokenizer())
		{
		}

		public DiceParser(IExpressionTokenizer tokenizer)
		{
			_tokenizer = tokenizer;
			
			_nodeMapper = new Dictionary<Type, Func<INode, INode, INode>>
			{
				{typeof(AddToken), (left, right) => new AddNode(left, right)},
				{typeof(SubtractToken), (left, right) => new SubtractNode(left, right)},
				{typeof(MultiplyToken), (left, right) => new MultiplyNode(left, right)},
				{typeof(DivideToken), (left, right) => new DivideNode(left, right)},
				{typeof(DiceToken), (left, right) => new DiceNode(left, right)},
			};
		}

		public INode Parse(string expression)
		{
			var tokens = _tokenizer.Tokenize(expression);

			return ParseTokens(tokens);
		}

		private bool MoveNext(IEnumerator<Token> tokens)
		{
			_hasNext = tokens.MoveNext();
			return _hasNext;
		}

		private INode ParseTokens(IEnumerable<Token> tokens)
		{
			if (!tokens.Any())
			{
				return null;
			}
			
			using var enumerator = tokens.GetEnumerator();

			if (MoveNext(enumerator))
			{
				return ParseAddSubtract(enumerator);
			}

			return null;
		}

		private INode ParseAddSubtract(IEnumerator<Token> tokens)
		{
			var leftNode = ParseMultiplyDivide(tokens);

			while (_hasNext)
			{
				if (!_hasNext)
				{
					return leftNode;
				}

				INode rightNode;
				var currentOperator = tokens.Current?.GetType();
				if (tokens.Current is AddToken || tokens.Current is SubtractToken)
				{
					MoveNext(tokens);
					rightNode = ParseMultiplyDivide(tokens);
				}
				else
				{
					return leftNode;
				}

				if (currentOperator != null)
				{
					leftNode = _nodeMapper[currentOperator].Invoke(leftNode, rightNode);
				}
				else
				{
					throw new InvalidDiceExpressionException("Cannot get type of operator");
				}
			}

			return leftNode;
		}

		private INode ParseMultiplyDivide(IEnumerator<Token> tokens)
		{
			var leftNode = ParseDice(tokens);

			while (_hasNext)
			{
				if (!_hasNext)
				{
					return leftNode;
				}

				INode rightNode;
				var currentOperator = tokens.Current?.GetType();
				if (tokens.Current is MultiplyToken || tokens.Current is DivideToken)
				{
					MoveNext(tokens);
					rightNode = ParseDice(tokens);
				}
				else
				{
					return leftNode;
				}


				if (currentOperator != null)
				{
					leftNode = _nodeMapper[currentOperator].Invoke(leftNode, rightNode);
				}
				else
				{
					throw new InvalidDiceExpressionException("Cannot get type of operator");
				}
			}

			return leftNode;
		}

		private INode ParseDice(IEnumerator<Token> tokens)
		{
			var leftNode = ParseLeaf(tokens);

			while (_hasNext)
			{
				if (!_hasNext)
				{
					return leftNode;
				}

				INode rightNode;
				var currentOperator = tokens.Current?.GetType();
				if (tokens.Current is DiceToken)
				{
					MoveNext(tokens);
					rightNode = ParseLeaf(tokens);
				}
				else
				{
					return leftNode;
				}


				if (currentOperator != null)
				{
					leftNode = _nodeMapper[currentOperator].Invoke(leftNode, rightNode);
				}
				else
				{
					throw new InvalidDiceExpressionException("Cannot get type of operator");
				}
			}

			return leftNode;
		}
		
		private INode ParseLeaf(IEnumerator<Token> tokens)
		{
			if (tokens.Current is ConstantToken token)
			{
				var node =  new NumberNode(token.Constant);
				MoveNext(tokens);
				return node;
			}

			if (tokens.Current is OpenParenthesisToken)
			{
				MoveNext(tokens);

				var node = ParseAddSubtract(tokens);

				if (!(tokens.Current is CloseParenthesisToken))
				{
					throw new InvalidDiceExpressionException("Close parenthesis not present");
				}

				MoveNext(tokens);

				return node;
			}

			throw new InvalidDiceExpressionException($"Unexpected token: {tokens.Current.Value}");
		}
	}
}