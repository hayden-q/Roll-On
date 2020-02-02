using System;
using System.Collections.Generic;

namespace RollOn
{
	public abstract class Token : ValueObject
	{
		public string Value { get; }

		public Token(string value)
		{
			Value = value;
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Value;
		}
	}

	public class NumberToken : Token
	{
		public double Constant { get; }

		public NumberToken(string value) : base(value)
		{
			if (!double.TryParse(value, out var constant))
			{
				throw new ArgumentException($"Cannot convert expression '{value}' to a number");
			}

			Constant = constant;
		}
	}

	public class VariableToken : Token
	{
		public VariableToken(string value) : base(value)
		{
		}
	}

	public class OpenParenthesisToken : Token
	{
		public OpenParenthesisToken() : base("(")
		{
		}
	}

	public class CloseParenthesisToken : Token
	{
		public CloseParenthesisToken() : base(")")
		{
		}
	}

	public abstract class OperatorToken : Token
	{
		protected OperatorToken(string value) : base(value)
		{
		}
	}

	public class AddToken : OperatorToken
	{
		public AddToken() : base("+")
		{
		}
	}

	public class SubtractToken : OperatorToken
	{
		public SubtractToken() : base("-")
		{
		}
	}

	public class MultiplyToken : OperatorToken
	{
		public MultiplyToken() : base("*")
		{
		}
	}

	public class DivideToken : OperatorToken
	{
		public DivideToken() : base("/")
		{
		}
	}

	public class DiceToken : OperatorToken
	{
		public DiceToken() : base("D")
		{
		}
	}

	public class KeepToken : OperatorToken
	{
		public KeepToken() : base("K")
		{
		}
	}
}