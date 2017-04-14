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
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using AgateLib.Quality;

namespace AgateLib.Mathematics.Geometry
{
	/// <summary>
	/// Implements a Rectangle structure, containing position and size.
	/// </summary>
	[DataContract]
	public struct Rectangle
	{
		[DataMember]
		Point pt;
		[DataMember]
		Size sz;

		/// <summary>
		/// Constructs a rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
        [DebuggerStepThrough]
        public Rectangle(int x, int y, int width, int height)
		{
			this.pt = new Point(x, y);
			this.sz = new Size(width, height);
		}
		/// <summary>
		/// Constructs a rectangle.
		/// </summary>
		/// <param name="pt"></param>
		/// <param name="sz"></param>
        [DebuggerStepThrough]
        public Rectangle(Point pt, Size sz)
		{
			this.pt = pt;
			this.sz = sz;
		}

		public static Rectangle FromString(string text)
		{
			if (text.StartsWith("{") && text.EndsWith("}"))
			{
				text = text.Substring(1, text.Length - 2);
			}

			string[] values = text.Split(',');
			Rectangle result = new Rectangle();

			if (values.Length != 4)
				throw new FormatException("Could not parse rectangle data from text.");

			if (text.Contains("="))
			{
				// parse named arguments
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i].ToLowerInvariant().Contains("width")
						&& values[i].Contains("="))
					{
						result.Width = ParseNumeric(values[i]);
					}
					else if (values[i].ToLowerInvariant().Contains("height")
						&& values[i].Contains("="))
					{
						result.Height = ParseNumeric(values[i]);
					}
					else if (values[i].ToLowerInvariant().Contains("x")
						&& values[i].Contains("="))
					{
						result.X = ParseNumeric(values[i]);
					}
					else if (values[i].ToLowerInvariant().Contains("y")
						&& values[i].Contains("="))
					{
						result.Y = ParseNumeric(values[i]);
					}
				}
			}
			else
			{
				result.X = int.Parse(values[0], System.Globalization.CultureInfo.InvariantCulture);
				result.Y = int.Parse(values[1], System.Globalization.CultureInfo.InvariantCulture);
				result.Width = int.Parse(values[2], System.Globalization.CultureInfo.InvariantCulture);
				result.Height = int.Parse(values[3], System.Globalization.CultureInfo.InvariantCulture);
			}
			return result;
		}
		private static int ParseNumeric(string text)
		{
			int equals = text.IndexOf("=", StringComparison.OrdinalIgnoreCase);
			int value = int.Parse(text.Substring(equals + 1), System.Globalization.CultureInfo.CurrentCulture);
			return value;
		}

		/// <summary>
		/// Static method which returns a rectangle with specified left, top, right and bottom.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="top"></param>
		/// <param name="right"></param>
		/// <param name="bottom"></param>
		/// <returns></returns>
        [DebuggerStepThrough]
        public static Rectangle FromLTRB(int left, int top, int right, int bottom)
		{
			return new Rectangle(left, top, right - left, bottom - top);
		}
		/// <summary>
		/// X value
		/// </summary>
        public int X
		{
            [DebuggerStepThrough]
            get { return pt.X; }
            [DebuggerStepThrough]
            set { pt.X = value; }
		}
		/// <summary>
		/// Y value
		/// </summary>
        public int Y
		{
            [DebuggerStepThrough]
            get { return pt.Y; }
            [DebuggerStepThrough]
            set { pt.Y = value; }
		}
		/// <summary>
		/// Width
		/// </summary>
        public int Width
		{
            [DebuggerStepThrough]
            get { return sz.Width; }
            [DebuggerStepThrough]
            set { sz.Width = value; }
		}
		/// <summary>
		/// Height
		/// </summary>
        public int Height
		{
            [DebuggerStepThrough]
            get { return sz.Height; }
            [DebuggerStepThrough]
            set { sz.Height = value; }
		}
		/// <summary>
		/// Gets bottom.
		/// </summary>
        public int Bottom
		{
            [DebuggerStepThrough]
            get { return pt.Y + sz.Height; }
		}
		/// <summary>
		/// Gets left.
		/// </summary>
		public int Left
		{
            [DebuggerStepThrough]
            get { return pt.X; }
		}
		/// <summary>
		/// Gets top.
		/// </summary>
		public int Top
		{
            [DebuggerStepThrough]
            get { return pt.Y; }
		}
		/// <summary>
		/// Gets right.
		/// </summary>
		public int Right
		{
            [DebuggerStepThrough]
            get { return pt.X + sz.Width; }
		}
		/// <summary>
		/// Gets or sets top-left point.
		/// </summary>
		public Point Location
		{
            [DebuggerStepThrough]
            get { return pt; }
            [DebuggerStepThrough]
            set { pt = value; }
		}
		/// <summary>
		/// Gets or sets size.
		/// </summary>
		public Size Size
		{
            [DebuggerStepThrough]
            get { return sz; }
            [DebuggerStepThrough]
            set { sz = value; }
		}
		/// <summary>
		/// Returns true if the rectangle contains the specified point.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool Contains(int x, int y)
		{
			return Contains(new Point(x, y));
		}
		/// <summary>
		/// Returns true if the rectangle contains the specified point.
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public bool Contains(Point pt)
		{
			if (pt.X < Left)
				return false;
			if (pt.Y < Top)
				return false;
			if (pt.X >= Right)
				return false;
			if (pt.Y >= Bottom)
				return false;

			return true;
		}

		/// <summary>
		/// Returns true if the rectangle entirely contains the specified rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public bool Contains(Rectangle rect)
		{
			if (Contains(rect.Location) && Contains(Point.Add(rect.Location, rect.Size)))
				return true;
			else
				return false;
		}

		/// <summary>
		/// Returns a rectangle contracted by the specified amount.  The center of the rectangle remains in the same place,
		/// so this adds amount to X and Y, and subtracts amount*2 from Width and Height.
		/// </summary>
		/// <param name="amount"></param>
		public Rectangle Contract(int amount)
		{
			return Expand(-amount);
		}

		/// <summary>
		/// Returns a rectangle contracted by the specified amount.  The center of the rectangle remains in the same place,
		/// so this adds amount to X and Y, and subtracts amount*2 from Width and Height.
		/// </summary>
		/// <param name="amount"></param>
		public Rectangle Contract(Size amount)
		{
			return Expand(new Size(-amount.Width, -amount.Height));
		}

		/// <summary>
		///  Returns a rectangle contracted or expanded by the specified amount.  
		///  The center of the rectangle remains in the same place,
		/// so this subtracts amount to X and Y, and adds amount*2 from Width and Height.
		/// </summary>
		/// <param name="amount">The amount to expand the rectangle by. If this value is negative,
		/// the rectangle is contracted.</param>
		public Rectangle Expand(int amount)
		{
			var result = this;

			result.X -= amount;
			result.Y -= amount;
			result.Width += amount * 2;
			result.Height += amount * 2;

			return result;
		}

		/// <summary>
		/// Returns a rectangle contracted or expanded by the specified amount.  
		/// This subtracts amount.Width from X and adds amount.Width * 2 to the rectangle width.
		/// </summary>
		/// <param name="amount">The amount to expand the rectangle by. If this value is negative,
		/// the rectangle is contracted.</param>
		public Rectangle Expand(Size amount)
		{
			var result = this;

			result.X -= amount.Width;
			result.Y -= amount.Height;
			result.Width += amount.Width * 2;
			result.Height += amount.Height * 2;

			return result;
		}

		/// <summary>
		/// Returns true if this intersects another rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public bool IntersectsWith(Rectangle rect)
		{
			if (this.Left >= rect.Right) return false;
			if (rect.Left >= this.Right) return false;
			if (this.Top >= rect.Bottom) return false;
			if (rect.Top >= this.Bottom) return false;

			return true;
		}
		/// <summary>
		/// True if this is (0,0,0,0).
		/// </summary>
		
		public bool IsEmpty
		{
			get { return pt.IsEmpty && sz.IsEmpty; }
		}

		/// <summary>
		/// Gets the center point of the rectangle.
		/// </summary>
		public Vector2 CenterPoint => new Vector2(Location.X + Size.Width * 0.5, Location.Y + Size.Height * 0.5);

		#region --- Operator Overloads ---

		/// <summary>
		/// Equality comparison test.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator ==(Rectangle a, Rectangle b)
		{
			return a.Equals(b);
		}
		/// <summary>
		/// Inequality comparison test.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator !=(Rectangle a, Rectangle b)
		{
			return !a.Equals(b);
		}

		#endregion

		#region --- Type Conversions ---

		/// <summary>
		/// Explicitly converts a Rectangle to a RectangleF structure.
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public static explicit operator RectangleF(Rectangle a)
		{
			return new RectangleF(a.X, a.Y, a.Width, a.Height);
		}

		/// <summary>
		/// Converts the rectangle to a polygon object.
		/// </summary>
		/// <returns></returns>
		public Polygon ToPolygon()
		{
			Polygon result = new Polygon();

			WriteToPolygon(result);

			return result;
		}

		/// <summary>
		/// Writes the rectangle vertices to the polygon.
		/// </summary>
		/// <param name="result"></param>
		public void WriteToPolygon(Polygon result)
		{
			result.Points.Count = 4;

			result[0] = new Vector2(Left, Top);
			result[1] = new Vector2(Left, Bottom);
			result[2] = new Vector2(Right, Bottom);
			result[3] = new Vector2(Right, Top);
		}

		#endregion
		#region --- Object Overrides ---

		/// <summary>
		/// Gets a hash code.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return (Left + Top + Bottom + Right);
		}

		/// <summary>
		/// Creates a string representing this rectangle.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format(System.Globalization.CultureInfo.CurrentCulture,
				"(X={0},Y={1},Width={2},Height={3})", X, Y, Width, Height);
		}
		/// <summary>
		/// Equality test.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is Rectangle)
				return Equals((Rectangle)obj);
			else
				return base.Equals(obj);
		}
		/// <summary>
		/// Equality test.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public bool Equals(Rectangle obj)
		{
			if (pt == obj.pt && sz == obj.sz)
				return true;
			else
				return false;
		}

		#endregion

		/// <summary>
		/// Empty rectangle
		/// </summary>
		public static readonly Rectangle Empty = new Rectangle(0, 0, 0, 0);

		/// <summary>
		/// Static method returning the intersection of two rectangles.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Rectangle Intersect(Rectangle a, Rectangle b)
		{
			if (a.IntersectsWith(b))
			{
				return Rectangle.FromLTRB(
					Math.Max(a.Left, b.Left),
					Math.Max(a.Top, b.Top),
					Math.Min(a.Right, b.Right),
					Math.Min(a.Bottom, b.Bottom));
			}
			else
				return Empty;
		}
		/// <summary>
		/// Rounds the coordinates in the rectangle up.
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public static Rectangle Ceiling(RectangleF a)
		{
			return new Rectangle(Point.Ceiling(a.Location), Size.Ceiling(a.Size));
		}

		/// <summary>
		/// Creates a new rectangle which contains all the area of the two passed in rectangles.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Rectangle Union(Rectangle a, Rectangle b)
		{
			Rectangle result = a;

			if (b.Top < a.Top)
			{
				result.Y = b.Y;
			}
			if (b.Left < a.Left)
			{
				result.X = b.X;
			}

			if (a.Right > result.Right) result.Width = a.Right - result.Left;
			if (b.Right > result.Right) result.Width = b.Right - result.Left;

			if (a.Bottom > result.Bottom) result.Height = a.Bottom - result.Top;
			if (b.Bottom > result.Bottom) result.Height = b.Bottom - result.Top;

			return result;
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
			Require.True<ArgumentException>(rects.Length > 0, "At least one rectangle must be passed to Rectangle.Union");

			return FromLTRB(
				rects.Min(r => r.Left),
				rects.Min(r => r.Top),
				rects.Max(r => r.Right),
				rects.Max(r => r.Bottom));
		}

		/// <summary>
		/// Parses a string for a rectangle.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static Rectangle Parse(string text)
		{
			Rectangle result;

			if (TryParse(text, out result) == false)
				throw new FormatException("Could not parse rectangle data from text.");

			return result;
		}
		/// <summary>
		/// Parses a string for a rectangle.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool TryParse(string text, out Rectangle value)
		{
			int x, y, width, height;
			value = new Rectangle();

			if (FindValue(text, "X=", out x) == false) return false;
			if (FindValue(text, "Y=", out y) == false) return false;
			if (FindValue(text, "Width=", out width) == false) return false;
			if (FindValue(text, "Height=", out height) == false) return false;

			value = new Rectangle(x, y, width, height);
			return true;
		}
		static bool FindValue(string text, string name, out int value)
		{
			int index = text.IndexOf(name, StringComparison.OrdinalIgnoreCase);
			int comma = text.IndexOf(",", index, StringComparison.OrdinalIgnoreCase);
			if (comma == -1)
				comma = text.IndexOf("}", index, StringComparison.OrdinalIgnoreCase);

			int start = index + name.Length;

			if (int.TryParse(text.Substring(start, comma - start), out value) == false)
				return false;
			else
				return true;
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
		/// Returns a rectangle which has the two specified points as corners.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Rectangle FromPoints(Point a, Point b)
		{
			return new Rectangle(
				Math.Min(a.X, b.X),
				Math.Min(a.Y, b.Y),
				Math.Abs(a.X - b.X),
				Math.Abs(a.Y - b.Y));
		}

		/// <summary>
		/// Expands the rectangle structure by the specified 
		/// size structure. Actually, the size is increased by 
		/// double the amount because the rectangle is expanded
		/// in all directions.
		/// </summary>
		/// <param name="size"></param>
		public void Inflate(Size size)
		{
			X -= size.Width;
			Y -= size.Height;

			Width += size.Width * 2;
			Height += size.Height * 2;
		}
		/// <summary>
		/// Expands the rectangle structure by the specified 
		/// size. Actually, the size is increased by 
		/// double the amount specified because the rectangle is expanded
		/// in all directions.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void Inflate(int width, int height)
		{
			Inflate(new Size(width, height));
		}
	}
}