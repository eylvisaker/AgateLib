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
