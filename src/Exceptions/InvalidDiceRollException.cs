using System;

namespace RollOn
{
	public class InvalidDiceRollException : Exception
	{
		public InvalidDiceRollException(string message)
			: base(message)
		{
		}
	}
}