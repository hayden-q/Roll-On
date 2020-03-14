using System.Collections.Generic;

namespace RollOn
{
	public interface IPrecedence
	{
		INode Parse(PrecedenceLevel currentLevel, IPrecedenceHandler handler, IStateEnumerator<Token> tokens);
	}
}