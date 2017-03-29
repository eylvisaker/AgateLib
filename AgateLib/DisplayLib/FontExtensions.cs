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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.Mathematics;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Extensions for the IFont interface.
	/// </summary>
	public static class FontExtensions
	{
		/// <summary>
		/// Draws text at the specified point.
		/// </summary>
		/// <param name="font"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="text"></param>
		public static void DrawText(this IFont font, double x, double y, string text)
		{
			font.DrawText(new Vector2(x, y), text);
		}

		/// <summary>
		/// Draws text at the specified point.
		/// </summary>
		/// <param name="font"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="text"></param>
		public static void DrawText(this IFont font, int x, int y, string text)
		{
			font.DrawText(new Vector2(x, y), text);
		}

		/// <summary>
		/// Draws text at the specified point.
		/// </summary>
		/// <param name="font"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="text"></param>
		/// <param name="parameters"></param>
		public static void DrawText(this IFont font, int x, int y, string text, params object[] parameters)
		{
			font.DrawText(new Vector2(x, y), text);
		}
	
		/// <summary>
		/// Performs text layout, with an optional maximum width value that will automatically
		/// wrap text at break points.
		/// </summary>
		/// <param name="font"></param>
		/// <param name="text"></param>
		/// <param name="maxWidth"></param>
		/// <returns></returns>
		public static ContentLayout LayoutText(this IFont font, string text, int? maxWidth)
		{
			var result = new ContentLayout(font, maxWidth);

			result.Append(text);

			return result;
		}
	}
}
