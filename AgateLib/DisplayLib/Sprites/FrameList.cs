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
using System.Collections.Generic;
using System.Text;

namespace AgateLib.DisplayLib.Sprites
{
	/// <summary>
	/// Class which contains a list of sprite frames.
	/// </summary>
	/// <typeparam name="T">Type which should implement the ISpriteFrame interface.</typeparam>
	public class FrameList<T> : IList<T>, IFrameList where T : ISpriteFrame
	{
		List<T> mFrames = new List<T>();

		#region IList<T> Members

		/// <summary>
		/// Returns the index of the specified frame.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(T item)
		{
			return mFrames.IndexOf(item);
		}
		/// <summary>
		/// Inserts a sprite frame into the list.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, T item)
		{
			mFrames.Insert(index, item);
		}
		/// <summary>
		/// Removes a sprite frame by index.
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			mFrames.RemoveAt(index);
		}
		/// <summary>
		/// Gets or sets the sprite frame at the specified index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public T this[int index]
		{
			get
			{
				return mFrames[index];
			}
			set
			{
				mFrames[index] = value;
			}
		}

		#endregion
		#region ICollection<T> Members

		/// <summary>
		/// Adds a sprite frame to the list.
		/// </summary>
		/// <param name="item"></param>
		public void Add(T item)
		{
			mFrames.Add(item);
		}
		/// <summary>
		/// Clears the list of sprite frames.
		/// </summary>
		public void Clear()
		{
			mFrames.Clear();
		}
		/// <summary>
		/// Returns true if the specified frame is in the list.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(T item)
		{
			return mFrames.Contains(item);
		}
		/// <summary>
		/// Copies the list to an array.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		public void CopyTo(T[] array, int arrayIndex)
		{
			mFrames.CopyTo(array, arrayIndex);
		}
		/// <summary>
		/// Returns the number of frames in the list
		/// </summary>
		public int Count
		{
			get { return mFrames.Count; }
		}
		/// <summary>
		/// Always false.
		/// </summary>
		public bool IsReadOnly
		{
			get { return false; }
		}
		/// <summary>
		/// Removes the specified item, if it is in the list.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Remove(T item)
		{
			return mFrames.Remove(item);
		}

		#endregion

		#region IEnumerable<T> Members

		/// <summary>
		/// Enumerates through the frames.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<T> GetEnumerator()
		{
			return mFrames.GetEnumerator();
		}

		#endregion
		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region IFrameList Members


		ISpriteFrame IFrameList.this[int index]
		{
			get { return mFrames[index]; }
		}

		int IFrameList.IndexOf(ISpriteFrame item)
		{
			if (item is T)
			{
				return mFrames.IndexOf((T)item);
			}
			else
				return -1;
		}

		bool IFrameList.Contains(ISpriteFrame item)
		{
			if (item is T)
				return mFrames.Contains((T)item);
			else
				return false;
		}

		void IFrameList.CopyTo(ISpriteFrame[] array, int arrayIndex)
		{
			Array.Copy(mFrames.ToArray(), 0, array, arrayIndex, mFrames.Count);
		}

		#endregion
	}
}
