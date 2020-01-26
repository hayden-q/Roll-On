using System.Collections.Generic;

namespace RollOn.Tests
{
	public sealed class ExampleValueObject : ValueObject
	{
		public string First { get;  }
		public string Second { get;  }

		public ExampleValueObject(string first, string second)
		{
			First = first;
			Second = second;
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return First;
			yield return Second;
		}
	}
}