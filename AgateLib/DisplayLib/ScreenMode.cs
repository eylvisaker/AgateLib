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

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Class which contains information about an available screen mode.
	/// </summary>
	public sealed class ScreenMode
	{
		private int mWidth;
		private int mHeight;
		private int mBpp;

		/// <summary>
		/// Constructs a ScreenMode object.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="bpp"></param>
		public ScreenMode(int width, int height, int bpp)
		{
			mWidth = width;
			mHeight = height;
			mBpp = bpp;
		}

		/// <summary>
		/// Width of the screen mode in pixels.
		/// </summary>
		public int Width
		{
			get { return mWidth; }
		}
		/// <summary>
		/// Height of the screen mode in pixels.
		/// </summary>
		public int Height
		{
			get { return mHeight; }
		}
		/// <summary>
		/// Bits per pixel of the screen mode.
		/// </summary>
		public int Bpp
		{
			get { return mBpp; }
		}

		/// <summary>
		/// Searches through the available screen resolutions and selects the one
		/// which is the closest match for the parameters passed in.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="bpp"></param>
		/// <returns>null if no screen mode could be found, otherwise the appropriate
		/// ScreenMode structure.</returns>
		public static ScreenMode SelectBestMode(int width, int height, int bpp)
		{
			ScreenMode[] modes = Display.EnumScreenModes();

			ScreenMode selected = null;
			int diff = 0;

			if (modes == null || modes.Length == 0)
				return null;

			foreach (ScreenMode mode in modes)
			{
				if (mode.Width < width)
					continue;

				if (mode.Height < height)
					continue;

				if (mode.Bpp != bpp)
					continue;

				int thisDiff = Math.Abs(width - mode.Width) + Math.Abs(height - mode.Height);
				thisDiff *= Math.Abs(mode.Bpp - bpp) / 2;


				// first mode by default, or any mode which is a better match.
				if (selected == null || thisDiff < diff)
				{
					selected = mode;
					diff = thisDiff;
				}

			}

			return selected;
		}

		/// <summary>
		/// Reports useful debugging information.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format(
				"ScreenMode: {0} x {1} @ {2}", mWidth, mHeight, mBpp);
		}
	}
}
