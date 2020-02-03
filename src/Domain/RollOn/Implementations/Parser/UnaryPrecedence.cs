namespace RollOn
{
	public class UnaryPrecedence : IPrecedence
	{
		private readonly INodeFactory _factory;

		public UnaryPrecedence(INodeFactory factory)
		{
			_factory = factory;
		}

		public INode Parse(PrecedenceLevel currentLevel, IPrecedenceHandler handler, IStateEnumerator<Token> tokens)
		{
			while (tokens.HasNext)
			{
				if (tokens.Current is AddToken)
				{
					tokens.MoveNext();
					continue;
				}

				if (tokens.Current is SubtractToken)
				{
					tokens.MoveNext();

					var rightNode = Parse(currentLevel, handler, tokens);

					return _factory.CreateUnary(rightNode);
				}

				return handler.RecurseToNext(currentLevel, tokens);
			}

			return null;
		}
	}
}