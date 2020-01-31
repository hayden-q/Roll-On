using System.Collections.Generic;

namespace RollOn.Tests
{
	public sealed class AnotherValueObject : ValueObject
	{
		private readonly bool _useNull;

		public AnotherValueObject(string first, string second, bool useNull = false)
		{
			First = first;
			Second = second;
			_useNull = useNull;
		}

		public string First { get; }
		public string Second { get; }

		protected override IEnumerable<object> GetAtomicValues()
		{
			if (_useNull)
			{
				yield return null;
			}
			else
			{
				yield return First;
				yield return Second;
			}
		}
	}
}