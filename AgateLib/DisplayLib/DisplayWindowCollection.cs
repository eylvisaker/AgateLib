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

		public IEnumerator<DisplayWindow> GetEnumerator()
		{
			return windows.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable) windows).GetEnumerator();
		}

		public void Add(DisplayWindow item)
		{
			windows.Add(item);
		}

		public void Clear()
		{
			windows.Clear();
		}

		public bool Contains(DisplayWindow item)
		{
			return windows.Contains(item);
		}

		public void CopyTo(DisplayWindow[] array, int arrayIndex)
		{
			windows.CopyTo(array, arrayIndex);
		}

		public bool Remove(DisplayWindow item)
		{
			return windows.Remove(item);
		}

		public int Count => windows.Count;

		public bool IsReadOnly => false;
	}
}
