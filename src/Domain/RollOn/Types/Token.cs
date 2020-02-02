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

	public class ConstantToken : Token
	{
		public double Constant { get; }

		public ConstantToken(string value) : base(value)
		{
			if (!double.TryParse(value, out var constant))
			{
				throw new ArgumentException($"Cannot convert expression '{value}' to a number");
			}

			Constant = constant;
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
		
		public int Precedence { get; protected set; }
	}

	public class AddToken : OperatorToken
	{
		public AddToken() : base("+")
		{
			Precedence = 1;
		}
	}

	public class SubtractToken : OperatorToken
	{
		public SubtractToken() : base("-")
		{
			Precedence = 1;
		}
	}

	public class MultiplyToken : OperatorToken
	{
		public MultiplyToken() : base("*")
		{
			Precedence = 2;
		}
	}

	public class DivideToken : OperatorToken
	{
		public DivideToken() : base("/")
		{
			Precedence = 2;
		}
	}

	public class DiceToken : OperatorToken
	{
		public DiceToken() : base("D")
		{
			Precedence = 3;
		}
	}
}