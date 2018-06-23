//
//    Copyright (c) 2006-2018 Erik Ylvisaker
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
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Styling;

namespace AgateLib.UserInterface.Layout
{
	/// <summary>
	/// Static class which provides some basic math utilities for doing widget layouts.
	/// </summary>
	public static class LayoutMath
	{
		/// <summary>
		/// Gets the maximum content size for an item, given the max dimensions of its parent.
		/// </summary>
		/// <param name="itemBox"></param>
		/// <param name="parentMaxWidth"></param>
		/// <param name="parentMaxHeight"></param>
		/// <returns></returns>
		public static Size ItemContentMaxSize(LayoutBox itemBox, Size parentMaxSize)
		{
			int itemMaxWidth = parentMaxSize.Width;
			int itemMaxHeight = parentMaxSize.Height;

			itemMaxWidth -= itemBox.Width;
			itemMaxHeight -= itemBox.Height;

			return new Size(itemMaxWidth, itemMaxHeight);
		}
	}
}
