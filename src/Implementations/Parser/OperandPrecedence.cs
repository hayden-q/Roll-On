namespace RollOn
{
	public class OperandPrecedence : IPrecedence
	{
		private readonly INodeFactory _factory;

		public OperandPrecedence(INodeFactory factory)
		{
			_factory = factory;
		}

		public INode Parse(PrecedenceLevel currentLevel, IPrecedenceHandler handler, IStateEnumerator<Token> tokens)
		{
			if (tokens.Current is NumberToken numberToken)
			{
				var node = _factory.CreateNumber(numberToken.Constant);
				tokens.MoveNext();
				return node;
			}

			if (tokens.Current is VariableToken)
			{
				var node = _factory.CreateVariable(tokens.Current.Value);
				tokens.MoveNext();
				return node;
			}

			if (tokens.Current is OpenParenthesisToken)
			{
				// Skip token
				tokens.MoveNext();

				// Parse sub expression
				var node = handler.RecurseToFirst(tokens);

				if (!tokens.HasNext || !(tokens.Current is CloseParenthesisToken))
				{
					throw new InvalidDiceExpressionException("Close parenthesis not present");
				}

				// Skip over close parenthesis
				tokens.MoveNext();

				return node;
			}

			throw new InvalidDiceExpressionException($"Unexpected token: {tokens.Current.Value}");
		}
	}
}