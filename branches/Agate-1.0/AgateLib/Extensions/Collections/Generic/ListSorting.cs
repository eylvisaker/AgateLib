﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Extensions.Collections.Generic
{
	public static class ListSorting
	{
		/// <summary>
		/// Provides a sort method for IList&lt;T&gt; objects which 
		/// is stable, unlike the List&lt;T&gt;.Sort() method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="comparison"></param>
		public static void InsertionSort<T>(this IList<T> list)
		{
			if (list == null) throw new ArgumentNullException("list");

			Comparison<T> comp;

			if (typeof(IComparable<T>).IsAssignableFrom(typeof(T)))
			{
				InsertionSort(list, (x, y) => ((IComparable<T>)x).CompareTo(y));
			}
			else if (typeof(IComparable).IsAssignableFrom(typeof(T)))
			{
				InsertionSort(list, (x, y) => ((IComparable)x).CompareTo(y));
			}
		}
		/// <summary>
		/// Provides a sort method for IList&lt;T&gt; objects which 
		/// is stable, unlike the List&lt;T&gt;.Sort() method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="comparison"></param>
		public static void InsertionSort<T>(this IList<T> list, Comparison<T> comparison)
		{
			if (list == null) throw new ArgumentNullException("list");
			if (comparison == null) throw new ArgumentNullException("comparison");

			int count = list.Count;
			for (int i = 1; i < count; i++)
			{
				T current = list[i];

				int j = i - 1;
				for (; j >= 0 && comparison(list[j], current) > 0; j--)
				{
					list[j + 1] = list[j];
				}
				list[j + 1] = current;
			}
		}

	}
}
