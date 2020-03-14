using System.Collections.Generic;

namespace RollOn
{
	public class PrecedenceHandler : IPrecedenceHandler
	{
		private readonly List<IPrecedence> _precedences;
		public PrecedenceHandler()
		{
			_precedences = new List<IPrecedence>();
		}

		public void RegisterPrecedence(IPrecedence precedence)
		{
			_precedences.Add(precedence);
		}

		public INode RecurseToNext(PrecedenceLevel currentLevel, IStateEnumerator<Token> tokens)
		{
			var newLevel = new PrecedenceLevel(currentLevel + 1);

			if (newLevel < _precedences.Count)
			{
				var node = _precedences[newLevel].Parse(newLevel, this, tokens);
				return node;
			}

			return null;
		}

		public INode RecurseToFirst(IStateEnumerator<Token> tokens)
		{
			var newLevel = new PrecedenceLevel(0);

			if (newLevel < _precedences.Count)
			{
				var node = _precedences[newLevel].Parse(newLevel, this, tokens);
				return node;
			}

			return null;
		}
	}
}