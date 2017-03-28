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
using System.Text;

namespace AgateLib.DisplayLib.Sprites
{
	/// <summary>
	/// Iterface implemented by a list of sprite frames.
	/// This provides a read-only view into the frames in a sprite.
	/// </summary>
	public interface IFrameList : IEnumerable
	{
		/// <summary>
		/// Returns the number of frames in the list.
		/// </summary>
		int Count { get; }
		/// <summary>
		/// Gets a frame by index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		ISpriteFrame this[int index] { get; }

		/// <summary>
		/// Searches for a particular frame.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		int IndexOf(ISpriteFrame item);

		/// <summary>
		/// Returns a bool indicating whether the specified frame is
		/// contained in the list.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		bool Contains(ISpriteFrame item);

		/// <summary>
		/// Copies the list of frame to a target array.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		void CopyTo(ISpriteFrame[] array, int arrayIndex);
	}
}
