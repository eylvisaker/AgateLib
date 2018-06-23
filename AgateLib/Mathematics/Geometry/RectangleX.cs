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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Quality;
using Microsoft.Xna.Framework;

namespace AgateLib.Mathematics.Geometry
{

	public static class RectangleX
	{
		/// <summary>
		/// Equality test. Returns true if X, Y, Width and Height are equal.
		/// </summary>
		/// <returns></returns>
		public static bool Equals(Rectangle a, Rectangle b)
		{
			if (a.X != b.X) return false;
			if (a.Y != b.Y) return false;
			if (a.Width != b.Width) return false;
			if (a.Height != b.Height) return false;

			return true;
		}


		/// <summary>
		/// Converts the rectangle to a polygon object.
		/// </summary>
		/// <returns></returns>
		public static Polygon ToPolygon(this Rectangle r)
		{
			Polygon result = new Polygon();

			WriteToPolygon(r, result);

			return result;
		}

		/// <summary>
		/// Converts the rectangle to a RectangleF structure.
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		public static RectangleF ToRectangleF(this Rectangle r)
		{
			return new RectangleF(r.X, r.Y, r.Width, r.Height);
		}

		/// <summary>
		/// Writes the rectangle vertices to the polygon.
		/// </summary>
		/// <param name="result"></param>
		public static void WriteToPolygon(this Rectangle r, Polygon result)
		{
			result.Points.Count = 4;

			result[0] = new Vector2(r.Left, r.Top);
			result[1] = new Vector2(r.Left, r.Bottom);
			result[2] = new Vector2(r.Right, r.Bottom);
			result[3] = new Vector2(r.Right, r.Top);
		}

		public static Rectangle FromLTRB(int left, int top, int right, int bottom)
		{
			return new Rectangle(left, top, right - left, bottom - top);
		}

		/// <summary>
		/// Returns a rectangle contracted by the specified amount.  The center of the rectangle remains in the same place,
		/// so this adds amount to X and Y, and subtracts amount*2 from Width and Height.
		/// </summary>
		/// <param name="amount"></param>
		public static Rectangle Contract(this Rectangle rect, int amount)
		{
			return Expand(rect, -amount);
		}

		/// <summary>
		/// Returns a rectangle contracted by the specified amount.  The center of the rectangle remains in the same place,
		/// so this adds amount to X and Y, and subtracts amount*2 from Width and Height.
		/// </summary>
		/// <param name="amount"></param>
		public static Rectangle Contract(this Rectangle rect, Size amount)
		{
			return Expand(rect, new Size(-amount.Width, -amount.Height));
		}

		/// <summary>
		///  Returns a rectangle contracted or expanded by the specified amount.  
		///  The center of the rectangle remains in the same place,
		/// so this subtracts amount to X and Y, and adds amount*2 from Width and Height.
		/// </summary>
		/// <param name="amount">The amount to expand the rectangle by. If this value is negative,
		/// the rectangle is contracted.</param>
		public static Rectangle Expand(this Rectangle rect, int amount)
		{
			return Expand(rect, new Size(amount, amount));
		}

		public static Rectangle Parse(string arg)
		{
			var numbers = arg.Split(',');

			return new Rectangle(
				int.Parse(numbers[0]),
				int.Parse(numbers[1]),
				int.Parse(numbers[2]),
				int.Parse(numbers[3]));
		}

		/// <summary>
		/// Returns a rectangle contracted or expanded by the specified amount.  
		/// This subtracts amount.Width from X and adds amount.Width * 2 to the rectangle width.
		/// </summary>
		/// <param name="amount">The amount to expand the rectangle by. If this value is negative,
		/// the rectangle is contracted.</param>
		public static Rectangle Expand(this Rectangle rect, Size amount)
		{
			var result = rect;

			result.X -= amount.Width;
			result.Y -= amount.Height;
			result.Width += amount.Width * 2;
			result.Height += amount.Height * 2;

			return result;
		}

		/// <summary>
		/// Returns a rectangle structure with all the values (location, size) from the 
		/// floating point rectangle structure rounded to the nearest integer.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static Rectangle Round(RectangleF rect)
		{
			return new Rectangle(
				(int)Math.Round(rect.X),
				(int)Math.Round(rect.Y),
				(int)Math.Round(rect.Width),
				(int)Math.Round(rect.Height));
		}

		/// <summary>
		/// Creates a new rectangle which contains all the area of the two passed in rectangles.
		/// </summary>
		/// <param name="rects">The collection of rectangles to union together.</param>
		/// <returns></returns>
		public static Rectangle Union(IEnumerable<Rectangle> rects)
		{
			return Union(rects.ToArray());
		}

		/// <summary>
		/// Creates a new rectangle which contains all the area of the two passed in rectangles.
		/// </summary>
		/// <param name="rects">The collection of rectangles to union together.</param>
		/// <returns></returns>
		public static Rectangle Union(params Rectangle[] rects)
		{
			Require.That<ArgumentException>(rects.Length > 0, "At least one rectangle must be passed to Rectangle.Union");

			return FromLTRB(
				rects.Min(r => r.Left),
				rects.Min(r => r.Top),
				rects.Max(r => r.Right),
				rects.Max(r => r.Bottom));
		}

		/// <summary>
		/// Returns true if this intersects another rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		[Obsolete]
		public static bool IntersectsWith(this Rectangle arct, Rectangle rect)
		{
			if (arct.Left >= rect.Right) return false;
			if (rect.Left >= arct.Right) return false;
			if (arct.Top >= rect.Bottom) return false;
			if (rect.Top >= arct.Bottom) return false;

			return true;
		}

		/// <summary>
		/// Gets the center point of the rectangle.
		/// </summary>
		public static Vector2 CenterPoint(this Rectangle rect) 
			=> new Vector2(rect.Location.X + rect.Size.X * 0.5f, rect.Location.Y + rect.Size.Y * 0.5f);

	}
}
