using System.Collections.Generic;
using System.Linq;

namespace RollOn
{
	public abstract class ValueObject
	{
		protected abstract IEnumerable<object> GetAtomicValues();

		public override bool Equals(object obj)
		{
			if (obj is null || obj.GetType() != GetType())
			{
				return false;
			}

			var other = (ValueObject) obj;

			using (var thisValues = GetAtomicValues().GetEnumerator())
			{
				using (var otherValues = other.GetAtomicValues().GetEnumerator())
				{
					return CompareProperties(thisValues, otherValues);
				}
			}
		}

		public override int GetHashCode()
		{
			return GetAtomicValues()
				.Select(x => x?.GetHashCode() ?? 0)
				.Aggregate((x, y) => x ^ y);
		}

		private static bool CompareProperties(IEnumerator<object> thisValues, IEnumerator<object> otherValues)
		{
			while (thisValues.MoveNext() && otherValues.MoveNext())
			{
				if (thisValues.Current is null ^ otherValues.Current is null)
				{
					return false;
				}

				if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
				{
					return false;
				}
			}

			return !thisValues.MoveNext() && !otherValues.MoveNext();
		}
	}
}