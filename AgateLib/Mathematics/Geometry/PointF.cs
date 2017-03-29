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
using System.Runtime.Serialization;

namespace AgateLib.Mathematics.Geometry
{
	/// <summary>
	/// Replacement for System.Drawing.PointF structure.
	/// </summary>
	[DataContract]
	public struct PointF 
	{
		[DataMember]
		float x, y;

		#region --- Construction ---

		/// <summary>
		/// Constructs a PointF object.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public PointF(float x, float y)
		{
			this.x = x;
			this.y = y;
		}
		/// <summary>
		/// Constructs a PointF object.
		/// </summary>
		/// <param name="pt"></param>
		public PointF(PointF pt)
		{
			this.x = pt.x;
			this.y = pt.y;
		}
		/// <summary>
		/// Constructs a PointF object.
		/// </summary>
		/// <param name="size"></param>
		public PointF(SizeF size)
		{
			this.x = size.Width;
			this.y = size.Height;
		}

		#endregion
		#region --- Public Properties ---

		/// <summary>
		/// Gets or sets the X value.
		/// </summary>
		public float X
		{
			get { return x; }
			set { x = value; }
		}
		/// <summary>
		/// Gets or sets the Y value.
		/// </summary>
		public float Y
		{
			get { return y; }
			set { y = value; }
		}

		/// <summary>
		/// Returns true if X and Y are zero.
		/// </summary>
		public bool IsZero
		{
			get { return x == 0 && y == 0; }
		}

		[Obsolete("Use IsZero instead.")]
		public bool IsEmpty => IsZero;


		#endregion

		#region --- Operator Overloads ---

		/// <summary>
		/// Equality comparison test.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator ==(PointF a, PointF b)
		{
			return a.Equals(b);
		}
		/// <summary>
		/// Inequality comparison test.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator !=(PointF a, PointF b)
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
		public static explicit operator Point(PointF a)
		{
			return new Point((int)a.X, (int)a.Y);
		}

		/// <summary>
		/// Converts to a Vector2 object.
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public static implicit operator Vector2(PointF a)
		{
			return new Vector2(a.x, a.y);
		}
		/// <summary>
		/// Converts to a Vector2 object.
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public static implicit operator Vector2f(PointF a)
		{
			return new Vector2f(a.x, a.y);
		}

		#endregion
		
		#region --- Object Overrides ---

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
		/// Gets a hash code.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return x.GetHashCode() ^ y.GetHashCode();
		}
		/// <summary>
		/// Equality test.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is PointF)
				return Equals((PointF)obj);
			else
				return base.Equals(obj);
		}
		/// <summary>
		/// Equality test.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public bool Equals(PointF obj)
		{
			if (x == obj.x && y == obj.y)
				return true;
			else
				return false;
		}

		#endregion

		#region --- Static Methods and Fields ---

		/// <summary>
		/// Empty PointF.
		/// </summary>
		public static readonly PointF Empty = new PointF(0, 0);

		/// <summary>
		/// Adds the specified SizeF structure to the specified PointF structure
		/// and returns the result as a new PointF structure.
		/// </summary>
		/// <param name="pt"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public static PointF Add(PointF pt, SizeF size)
		{
			return new PointF(pt.x + size.Width, pt.y + size.Height);
		}

		#endregion
	}
}
