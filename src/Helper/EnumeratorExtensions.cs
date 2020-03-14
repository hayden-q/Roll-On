using System.Collections.Generic;

namespace RollOn
{
	public static class EnumeratorExtensions
	{
		public static IStateEnumerator<T> ToStateEnumerator<T>(this IEnumerator<T> enumerator) => new StateEnumerator<T>(enumerator);
	}
}