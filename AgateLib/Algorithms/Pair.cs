using System;

namespace AgateLib.Algorithms
{
	/// <summary>
	/// This structure is used instead of the generic .NET Tuple&lt;&gt; classes because
	/// the Tuple classes are immutable, and this is needed internally by the iterating algorithms.
	/// </summary>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	[Obsolete("Is this still used? Or should it be replaced?", false)]
	public struct Pair<T1, T2>
	{
		public T1 First;
		public T2 Second;

		public Pair(T1 f, T2 s)
		{
			First = f;
			Second = s;
		}
	}
}