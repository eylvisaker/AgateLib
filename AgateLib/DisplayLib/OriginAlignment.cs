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

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// OriginAlignment enum.  Used to specify how
	/// points should be interpreted.
	/// </summary>
	public enum OriginAlignment
	{
		/// <summary>
		/// Point indicates top-left.
		/// </summary>
		TopLeft = 0x11,
		/// <summary>
		/// Point indicates top-center.
		/// </summary>
		TopCenter = 0x12,
		/// <summary>
		/// Point indicates top-right.
		/// </summary>
		TopRight = 0x13,

		/// <summary>
		/// Point indicates center-left.
		/// </summary>
		CenterLeft = 0x21,
		/// <summary>
		/// Point indicates center.
		/// </summary>
		Center = 0x22,
		/// <summary>
		/// Point indicates center-right.
		/// </summary>
		CenterRight = 0x23,

		/// <summary>
		/// Point indicates bottom-left.
		/// </summary>
		BottomLeft = 0x31,
		/// <summary>
		/// Point indicates bottom-center.
		/// </summary>
		BottomCenter = 0x32,
		/// <summary>
		/// Point indicates bottom-right.
		/// </summary>
		BottomRight = 0x33,

		/// <summary>
		/// Specified indicates that the value in question is specified through
		/// some other means.
		/// </summary>
		Specified = 0xFF,
	}

}
