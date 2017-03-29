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
using Draw = System.Drawing;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Platform.WinForms
{
	/// <summary>
	/// Class for System.Drawing interoperation.  This converts members of
	/// AgateLib.Geometry into System.Drawing types, and vice versa.  
	/// </summary>
	public static class FormsInterop
	{
		/// <summary>
		/// Converts surface data to a bitmap for saving.
		/// </summary>
		/// <param name="surf"></param>
		/// <returns></returns>
		public static System.Drawing.Bitmap ToBitmap(this Surface surf)
		{
			// TODO: try to dump this save to a temp file and load bitmap round-about method.
			string filename = System.IO.Path.GetTempFileName();

			surf.SaveTo(filename);

			System.Drawing.Bitmap result;

			using (var stream = System.IO.File.OpenRead(filename))
			{
				result = new System.Drawing.Bitmap(stream);
			}

			System.IO.File.Delete(filename);

			return result;
		}
		/// <summary>
		/// Converts color structures.
		/// </summary>
		/// <param name="clr"></param>
		/// <returns></returns>
		public static Draw.Color ToDrawing(this Color clr)
		{
			return Draw.Color.FromArgb(clr.ToArgb());
		}
		/// <summary>
		/// Converts color structures.
		/// </summary>
		/// <param name="clr"></param>
		/// <returns></returns>
		public static Color ToGeometry(this Draw.Color clr)
		{
			return Color.FromArgb(clr.ToArgb());
		}

		/// <summary>
		/// Converts rectangle structures.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static Draw.Rectangle ToDrawing(this Rectangle rect)
		{
			return new Draw.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>
		/// Converts rectangle structures.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static Rectangle ToGeometry(this Draw.Rectangle rect)
		{
			return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
		}


		/// <summary>
		/// Converts rectangle structures.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static Draw.RectangleF ToDrawing(this RectangleF rect)
		{
			return new Draw.RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>
		/// Converts rectangle structures.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static RectangleF ToGeometry(this Draw.RectangleF rect)
		{
			return new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
		}


		/// <summary>
		/// Converts point structures.
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public static Point ToGeometry(this Draw.Point pt)
		{
			return new Point(pt.X, pt.Y);
		}
		/// <summary>
		/// Converts point structures.
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public static Draw.Point ToDrawing(this Point pt)
		{
			return new Draw.Point(pt.X, pt.Y);
		}

		/// <summary>
		/// Converts point structures.
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public static PointF ToGeometry(this Draw.PointF pt)
		{
			return new PointF(pt.X, pt.Y);
		}
		/// <summary>
		/// Converts point structures.
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public static Draw.PointF ToDrawing(this PointF pt)
		{
			return new Draw.PointF(pt.X, pt.Y);
		}

		/// <summary>
		/// Converts size structures.
		/// </summary>
		/// <param name="sz"></param>
		/// <returns></returns>
		public static Size ToGeometry(this Draw.Size sz)
		{
			return new Size(sz.Width, sz.Height);
		}
		/// <summary>
		/// Converts size structures.
		/// </summary>
		/// <param name="sz"></param>
		/// <returns></returns>
		public static Draw.Size ToDrawing(this Size sz)
		{
			return new Draw.Size(sz.Width, sz.Height);
		}

		/// <summary>
		/// Converts size structures.
		/// </summary>
		/// <param name="sz"></param>
		/// <returns></returns>
		public static SizeF ToGeometry(this Draw.SizeF sz)
		{
			return new SizeF(sz.Width, sz.Height);
		}
		/// <summary>
		/// Converts size structures.
		/// </summary>
		/// <param name="sz"></param>
		/// <returns></returns>
		public static Draw.SizeF ToDrawing(this SizeF sz)
		{
			return new Draw.SizeF(sz.Width, sz.Height);
		}

	}
}
