using System;

namespace RollOn
{
	public class InvalidExpressionException : Exception
	{
		public InvalidExpressionException(string message)
			: base(message)
		{
		}
	}
}