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
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Disposable collection class which contains DisplayWindow objects.
	/// </summary>
	public class DisplayWindowCollection : ICollection<DisplayWindow>, IDisposable
	{
		private List<DisplayWindow> windows = new List<DisplayWindow>();

		/// <summary>
		/// Destroys all the DisplayWindow objects contained in the collection.
		/// </summary>
		public void Dispose()
		{
			foreach (var window in windows)
				window.Dispose();
		}

		/// <summary>
		/// Enumerates the display windows.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<DisplayWindow> GetEnumerator()
		{
			return windows.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable) windows).GetEnumerator();
		}

		/// <summary>
		/// Adds a display window.
		/// </summary>
		/// <param name="item"></param>
		public void Add(DisplayWindow item)
		{
			windows.Add(item);
		}

		/// <summary>
		/// Clears the collection.
		/// </summary>
		public void Clear()
		{
			windows.Clear();
		}

		/// <summary>
		/// Returns true if the collection contains the item.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(DisplayWindow item)
		{
			return windows.Contains(item);
		}

		/// <summary>
		/// Copies the collection to an array.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		public void CopyTo(DisplayWindow[] array, int arrayIndex)
		{
			windows.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Removes an item from the collection.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Remove(DisplayWindow item)
		{
			return windows.Remove(item);
		}

		/// <summary>
		/// Returns the number of items in the collection.
		/// </summary>
		public int Count => windows.Count;

		/// <summary>
		/// Returns false.
		/// </summary>
		public bool IsReadOnly => false;
	}
}
