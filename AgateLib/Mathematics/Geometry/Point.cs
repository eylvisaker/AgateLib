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
using System.Diagnostics;
using System.Runtime.Serialization;

namespace AgateLib.Mathematics.Geometry
{
	/// <summary>
	/// Replacement for System.Drawing.Point structure.
	/// </summary>
	[DataContract]
	public struct Point
	{
		[DataMember]
		int x, y;

		#region --- Construction ---

		/// <summary>
		/// Constructs a point.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		[DebuggerStepThrough]
		public Point(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		/// <summary>
		/// Constructs a point.
		/// </summary>
		/// <param name="pt"></param>
		[DebuggerStepThrough]
		public Point(Point pt)
		{
			this.x = pt.x;
			this.y = pt.y;
		}

		/// <summary>
		/// Constructs a point.
		/// </summary>
		/// <param name="size"></param>
		[DebuggerStepThrough]
		public Point(Size size)
		{
			this.x = size.Width;
			this.y = size.Height;
		}

		/// <summary>
		/// Parses the string.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static Point FromString(string str)
		{
			if (str.StartsWith("{") && str.EndsWith("}"))
				str = str.Substring(1, str.Length - 2);

			string[] values = str.Split(',');
			Point result = new Point();

			if (values.Length > 2)
				throw new FormatException("Could not parse point data from text.");

			result.X = ParseEntry(values[0], "X");
			result.Y = ParseEntry(values[1], "Y");

			return result;
		}

		private static int ParseEntry(string str, string name)
		{
			var r = new System.Text.RegularExpressions.Regex(name + " *=");
			var matches = r.Matches(str);

			switch (matches.Count)
			{
				case 0:
					return int.Parse(str);
				case 1:
					return int.Parse(str.Substring(matches[0].Index + matches[0].Length));
				default:
					throw new FormatException("Could not parse " + name + " value.");
			}
		}

		#endregion
		#region --- Public Properties ---

		/// <summary>
		/// Gets or sets the X value.
		/// </summary>
		public int X
		{
			[DebuggerStepThrough]
			get { return x; }
			[DebuggerStepThrough]
			set { x = value; }
		}
		/// <summary>
		/// Gets or sets the Y value.
		/// </summary>
		public int Y
		{
			[DebuggerStepThrough]
			get { return y; }
			[DebuggerStepThrough]
			set { y = value; }
		}

		/// <summary>
		/// Returns true if X and Y are zero.
		/// </summary>
		public bool IsEmpty
		{
			[DebuggerStepThrough]
			get { return x == 0 && y == 0; }
		}

		#endregion

		#region --- Operator Overloads ---

		/// <summary>
		/// Equality comparison test.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator ==(Point a, Point b)
		{
			return a.Equals(b);
		}
		/// <summary>
		/// Inequality comparison test.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator !=(Point a, Point b)
		{
			return !a.Equals(b);
		}

		#endregion
		#region --- Type Converters ---

		/// <summary>
		/// Explicitly converts a point to a pointf structure.
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public static explicit operator PointF(Point a)
		{
			return new PointF((float)a.X, (float)a.Y);
		}

		/// <summary>
		/// Converts to a Vector2 object.
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public static implicit operator Vector2(Point a)
		{
			return new Vector2(a.X, a.Y);
		}

		/// <summary>
		/// Converts to a Vector2f object.
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public static implicit operator Vector2f(Point a)
		{
			return new Vector2f(a.X, a.Y);
		}

		#endregion

		#region --- Object Overrides ---

		/// <summary>
		/// Gets a hash code
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return x.GetHashCode() + y.GetHashCode();
		}
		/// <summary>
		/// Creates a string representing this object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format(System.Globalization.CultureInfo.CurrentCulture,
				"(X={0},Y={1})", x, y);
		}
		/// <summary>
		/// Equality test.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is Point)
				return Equals((Point)obj);
			else
				return base.Equals(obj);
		}
		/// <summary>
		/// Equality test.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public bool Equals(Point obj)
		{
			if (x == obj.x && y == obj.y)
				return true;
			else
				return false;
		}

		#endregion

		#region --- Static Methods and Fields ---

		/// <summary>
		/// Empty point.
		/// </summary>
		[Obsolete("Use Point.Zero instead.")]
		public static readonly Point Empty = new Point(0, 0);

		/// <summary>
		/// Point with X and Y equal to zero.
		/// </summary>
		public static readonly Point Zero = new Point(0, 0);

		/// <summary>
		/// Adds the specified size object to the specified point object
		/// and returns the new point.
		/// </summary>
		/// <param name="pt"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public static Point Add(Point pt, Size size)
		{
			return new Point(pt.x + size.Width, pt.y + size.Height);
		}
		/// <summary>
		/// Rounds the PointF object up.
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public static Point Ceiling(Vector2f pt)
		{
			return new Point((int)Math.Ceiling(pt.X), (int)Math.Ceiling(pt.Y));
		}

		/// <summary>
		/// Rounds the PointF object to the nearest integer.
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public static Point Round(Vector2f pt)
		{
			return new Point((int)Math.Round(pt.X), (int)Math.Round(pt.Y));
		}

		#endregion
	}
}
