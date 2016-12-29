//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AgateLib.Extensions.Collections.Generic
{
	/// <summary>
	/// Extensions for Lists that provide stable sort methods.
	/// </summary>
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

			Comparison<T> comp = null;

			if (typeof(T).GetTypeInfo().ImplementedInterfaces.Contains(typeof(IComparable<T>)))
			{
				comp = (x, y) => ((IComparable<T>)x).CompareTo(y);
			}
			else if (typeof(T).GetTypeInfo().ImplementedInterfaces.Contains(typeof(IComparable)))
			{
				comp = (x, y) => ((IComparable)x).CompareTo(y);
			}

			if (comp == null)
				throw new InvalidOperationException("No comparison method available for " + typeof(T).FullName);

			InsertionSort(list, comp);
		}
		/// <summary>
		/// Provides a sort method for IList&lt;T&gt; objects which 
		/// is stable, unlike the List&lt;T&gt;.Sort() method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="comparison"></param>
		public static void InsertionSort<T>(this IList<T> list, IComparer<T> comparer)
		{
			if (list == null) throw new ArgumentNullException("list");

			InsertionSort(list, (x, y) => comparer.Compare(x, y));
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
