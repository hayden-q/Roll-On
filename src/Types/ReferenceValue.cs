using System;

namespace RollOn
{
	public sealed class ReferenceValue
	{
		private readonly Func<double> _value;

		public ReferenceValue(Func<double> value)
		{
			_value = value;
		}

		public double Value => _value.Invoke();
	}
}