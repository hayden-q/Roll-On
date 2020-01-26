using System;
using System.Collections.Generic;

namespace RollOn
{
	public static class CollectionExtensions
	{
		public static void PushRange<T>(this Stack<T> source, IEnumerable<T> elements)
		{
			if (elements is null)
			{
				return;
			}
			
			foreach (var element in elements)
			{
				source.Push(element);
			}
		}
		
		public static IEnumerable<int> AllIndexesOf(this string input, string searchString)
		{
			var minIndex = input?.IndexOf(searchString, StringComparison.Ordinal) ?? -1;

			while (minIndex != -1)
			{
				yield return minIndex;
				minIndex = input?.IndexOf(searchString, minIndex + searchString.Length, StringComparison.Ordinal) ?? -1;
			}
		}
	}
}