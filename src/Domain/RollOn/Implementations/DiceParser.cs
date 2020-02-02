using System;
using System.Collections.Generic;
using System.Linq;

namespace RollOn
{
	public class DiceParser : IDiceParser
	{
		private bool _hasNext = true;
		private readonly IExpressionTokenizer _tokenizer;
		private readonly INodeFactory _nodeFactory;
		private readonly Dictionary<Type, Func<INode, INode, INode>> _nodeMapper;
		
		public DiceParser() : this(new ExpressionTokenizer(), new NodeFactory())
		{
		}

		public DiceParser(IExpressionTokenizer tokenizer, INodeFactory nodeFactory)
		{
			_tokenizer = tokenizer;
			_nodeFactory = nodeFactory;

			_nodeMapper = new Dictionary<Type, Func<INode, INode, INode>>
			{
				{typeof(AddToken), (left, right) => _nodeFactory.CreateAdd(left, right)},
				{typeof(SubtractToken), (left, right) => _nodeFactory.CreateSubtract(left, right)},
				{typeof(MultiplyToken), (left, right) => _nodeFactory.CreateMultiply(left, right)},
				{typeof(DivideToken), (left, right) => _nodeFactory.CreateDivide(left, right)},
				{typeof(DiceToken), (left, right) => _nodeFactory.CreateDice(left, right)},
				{typeof(KeepToken), (left, right) => _nodeFactory.CreateKeep(left as IDiceNode, right)},
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

				if (currentOperator is null)
				{
					throw new InvalidDiceExpressionException("Cannot get type of operator");
				}

				leftNode = _nodeMapper[currentOperator].Invoke(leftNode, rightNode);
			}

			return leftNode;
		}

		private INode ParseMultiplyDivide(IEnumerator<Token> tokens)
		{
			var leftNode = ParseDiceKeep(tokens);

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
					rightNode = ParseDiceKeep(tokens);
				}
				else
				{
					return leftNode;
				}

				if (currentOperator is null)
				{
					throw new InvalidDiceExpressionException("Cannot get type of operator");
				}

				leftNode = _nodeMapper[currentOperator].Invoke(leftNode, rightNode);
			}

			return leftNode;
		}

		private INode ParseDiceKeep(IEnumerator<Token> tokens)
		{
			var leftNode = ParseUnary(tokens);

			while (_hasNext)
			{
				if (!_hasNext)
				{
					return leftNode;
				}

				INode rightNode;
				var currentOperator = tokens.Current?.GetType();
				if (tokens.Current is DiceToken || tokens.Current is KeepToken)
				{
					MoveNext(tokens);
					rightNode = ParseUnary(tokens);
				}
				else
				{
					return leftNode;
				}

				if (currentOperator is null)
				{
					throw new InvalidDiceExpressionException("Cannot get type of operator");
				}
			
				leftNode = _nodeMapper[currentOperator].Invoke(leftNode, rightNode);
			}

			return leftNode;
		}

		private INode ParseUnary(IEnumerator<Token> tokens)
		{
			while (_hasNext)
			{
				if (tokens.Current is AddToken)
				{
					MoveNext(tokens);
					continue;
				}

				if (tokens.Current is SubtractToken)
				{
					MoveNext(tokens);

					var rightNode = ParseUnary(tokens);

					return _nodeFactory.CreateUnary(rightNode);
				}

				return ParseLeaf(tokens);
			}

			return null;
		}

		private INode ParseLeaf(IEnumerator<Token> tokens)
		{
			if (tokens.Current is NumberToken numberToken)
			{
				var node = _nodeFactory.CreateNumber(numberToken.Constant);
				MoveNext(tokens);
				return node;
			}

			if (tokens.Current is VariableToken)
			{
				var node = _nodeFactory.CreateVariable(tokens.Current.Value);
				MoveNext(tokens);
				return node;
			}

			if (tokens.Current is OpenParenthesisToken)
			{
				// Skip token
				MoveNext(tokens);

				// Parse sub expression
				var node = ParseAddSubtract(tokens);

				if (!(tokens.Current is CloseParenthesisToken))
				{
					throw new InvalidDiceExpressionException("Close parenthesis not present");
				}

				// Skip over close parenthesis
				MoveNext(tokens);

				return node;
			}

			throw new InvalidDiceExpressionException($"Unexpected token: {tokens.Current.Value}");
		}
	}
}