//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AgateLib.Collections.Generic
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

		/// <summary>
		/// Randomizes the list order
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="random"></param>
		public static void Randomize<T>(this IList<T> list, Random random)
		{
			for (int i = 0; i < list.Count; i++)
			{
				var index = random.Next(list.Count - i);

				var item = list[index];
				list.RemoveAt(index);

				list.Add(item);
			}
		}
	}
}
