using System.Collections.Generic;

namespace RollOn
{
	public interface IPrecedenceHandler
	{
		void RegisterPrecedence(IPrecedence precedence);
		INode RecurseToNext(PrecedenceLevel currentLevel, IStateEnumerator<Token> tokens);
		INode RecurseToFirst(IStateEnumerator<Token> tokens);
	}
}