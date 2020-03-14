using System;
using System.Collections;
using System.Collections.Generic;

namespace RollOn
{
	public interface IStateEnumerator<out T> : IEnumerator<T>
	{
		bool HasNext { get; }
	}

	public class StateEnumerator<T> : IStateEnumerator<T>
	{
		private readonly IEnumerator<T> _enumerator;

		public StateEnumerator(IEnumerator<T> enumerator)
		{
			_enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));
		}

		public bool HasNext { get; private set; }
		public T Current => _enumerator.Current;

		public bool MoveNext()
		{
			HasNext = _enumerator.MoveNext();
			return HasNext;
		}

		public void Reset()
		{
			_enumerator.Reset();
			HasNext = false;
		}

		object IEnumerator.Current => Current;

		public void Dispose() => _enumerator.Dispose();
	}
}