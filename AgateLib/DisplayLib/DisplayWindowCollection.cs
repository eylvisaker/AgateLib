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
