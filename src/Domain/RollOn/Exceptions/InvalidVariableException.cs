using System;

namespace RollOn
{
	public class InvalidVariableException : Exception
	{
		public InvalidVariableException(string message)
			: base(message)
		{
		}
	}
}