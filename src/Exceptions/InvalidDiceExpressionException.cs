using System;

namespace RollOn
{
	public class InvalidDiceExpressionException : Exception
	{
		public InvalidDiceExpressionException(string message)
			: base(message)
		{
		}
	}
}