using System.Collections.Generic;

namespace RollOn
{
	public interface IExpressionTokenizer
	{
		IEnumerable<Token> Tokenize(string expression);
	}
}