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

		public abstract bool InputIsToken(string input);

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

		public override bool InputIsToken(string input) => double.TryParse(input, out _);
	}

	public abstract class OperatorToken : Token
	{
		protected OperatorToken(string value) : base(value)
		{
		}

		public override bool InputIsToken(string input) => string.Equals(input.Trim(), Value.Trim(), StringComparison.OrdinalIgnoreCase);
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
}