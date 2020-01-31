using System;
using System.Threading;

namespace RollOn
{
	public class ThreadSafeRandom : IRandom
	{
		private readonly ThreadLocal<Random> _random;

		public ThreadSafeRandom()
		{
			_random = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));
		}

		public int Next(int start, int endExclusive)
		{
			return _random.Value.Next(start, endExclusive);
		}
	}
}