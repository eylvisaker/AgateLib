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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;
using Draw = System.Drawing;
using AgateLib.Geometry;

namespace AgateLib.WinForms
{
	/// <summary>
	/// Class for System.Drawing interoperation.  This converts members of
	/// AgateLib.Geometry into System.Drawing types, and vice versa.  
	/// </summary>
	public static class Interop
	{
		/// <summary>
		/// Converts color structures.
		/// </summary>
		/// <param name="clr"></param>
		/// <returns></returns>
		public static Draw.Color Convert(Color clr)
		{
			return Draw.Color.FromArgb(clr.ToArgb());
		}
		/// <summary>
		/// Converts color structures.
		/// </summary>
		/// <param name="clr"></param>
		/// <returns></returns>
		public static Color Convert(Draw.Color clr)
		{
			return Color.FromArgb(clr.ToArgb());
		}

		/// <summary>
		/// Converts rectangle structures.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static Draw.Rectangle Convert(Rectangle rect)
		{
			return new Draw.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>
		/// Converts rectangle structures.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static Rectangle Convert(Draw.Rectangle rect)
		{
			return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
		}


		/// <summary>
		/// Converts rectangle structures.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static Draw.RectangleF Convert(RectangleF rect)
		{
			return new Draw.RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>
		/// Converts rectangle structures.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static RectangleF Convert(Draw.RectangleF rect)
		{
			return new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
		}


		/// <summary>
		/// Converts point structures.
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public static Point Convert(Draw.Point pt)
		{
			return new Point(pt.X, pt.Y);
		}
		/// <summary>
		/// Converts point structures.
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public static Draw.Point Convert(Point pt)
		{
			return new Draw.Point(pt.X, pt.Y);
		}

		/// <summary>
		/// Converts point structures.
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public static PointF Convert(Draw.PointF pt)
		{
			return new PointF(pt.X, pt.Y);
		}
		/// <summary>
		/// Converts point structures.
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public static Draw.PointF Convert(PointF pt)
		{
			return new Draw.PointF(pt.X, pt.Y);
		}

		/// <summary>
		/// Converts size structures.
		/// </summary>
		/// <param name="sz"></param>
		/// <returns></returns>
		public static Size Convert(Draw.Size sz)
		{
			return new Size(sz.Width, sz.Height);
		}
		/// <summary>
		/// Converts size structures.
		/// </summary>
		/// <param name="sz"></param>
		/// <returns></returns>
		public static Draw.Size Convert(Size sz)
		{
			return new Draw.Size(sz.Width, sz.Height);
		}

		/// <summary>
		/// Converts size structures.
		/// </summary>
		/// <param name="sz"></param>
		/// <returns></returns>
		public static SizeF Convert(Draw.SizeF sz)
		{
			return new SizeF(sz.Width, sz.Height);
		}
		/// <summary>
		/// Converts size structures.
		/// </summary>
		/// <param name="sz"></param>
		/// <returns></returns>
		public static Draw.SizeF Convert(SizeF sz)
		{
			return new Draw.SizeF(sz.Width, sz.Height);
		}

	}
}
