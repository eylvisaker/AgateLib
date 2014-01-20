using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Extensions.Collections.NonGeneric
{
	public static class NonGenericListSorting
	{
		/// <summary>
		/// Provides a sort method for IList&lt;T&gt; objects which 
		/// is stable, unlike the List&lt;T&gt;.Sort() method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="comparison"></param>
		public static void InsertionSort(this IList list)
		{
			if (list == null) throw new ArgumentNullException("list");

			int count = list.Count;
			for (int i = 1; i < count; i++)
			{
				object current = list[i];

				int j = i - 1;
				for (; j >= 0 && Comparer<object>.Default.Compare(list[j], current) > 0; j--)
				{
					list[j + 1] = list[j];
				}
				list[j + 1] = current;
			}
		}
	}
}
