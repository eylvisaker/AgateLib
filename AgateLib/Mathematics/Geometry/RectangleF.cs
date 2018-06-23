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
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using YamlDotNet.Serialization;

namespace AgateLib.Mathematics.Geometry
{
	/// <summary>
	/// Replacement for System.Drawing.RectangleF structure.
	/// </summary>
	public struct RectangleF 
	{
		Vector2 pt;
		SizeF sz;

		/// <summary>
		/// Constructs a RectangleF.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public RectangleF(float x, float y, float width, float height)
		{
			this.pt = new Vector2(x, y);
			this.sz = new SizeF(width, height);
		}
		/// <summary>
		/// Construts a RectangleF.
		/// </summary>
		/// <param name="pt"></param>
		/// <param name="sz"></param>
		public RectangleF(Microsoft.Xna.Framework.Vector2 pt, SizeF sz)
		{
			this.pt = (Vector2)pt;
			this.sz = sz;
		}

		public static explicit operator RectangleF(Rectangle r)
		{
			return new RectangleF(r.X, r.Y, r.Width, r.Height);
		}

		/// <summary>
		/// Static method which returns a RectangleF with specified left, top, right and bottom.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="top"></param>
		/// <param name="right"></param>
		/// <param name="bottom"></param>
		/// <returns></returns>
		public static RectangleF FromLTRB(float left, float top, float right, float bottom)
		{
			return new RectangleF(left, top, right - left, bottom - top);
		}

		/// <summary>
		/// X value
		/// </summary>
		public float X
		{
			get => pt.X;
			set => pt.X = value;
		}
		/// <summary>
		/// Y value
		/// </summary>
		public float Y
		{
			get => pt.Y;
			set => pt.Y = value;
		}

		/// <summary>
		/// Width
		/// </summary>
		public float Width
		{
			get => sz.Width;
			set => sz.Width = value;
		}

		/// <summary>
		/// Height
		/// </summary>
		public float Height
		{
			get => sz.Height;
			set => sz.Height = value;
		}

		/// <summary>
		/// Gets bottom.
		/// </summary>
		[YamlIgnore]
		public float Bottom => pt.Y + sz.Height;

		/// <summary>
		/// Gets left.
		/// </summary>
		[YamlIgnore]
		public float Left => pt.X;

		/// <summary>
		/// Gets top.
		/// </summary>
		[YamlIgnore]
		public float Top => pt.Y;

		/// <summary>
		/// Gets right.
		/// </summary>
		[YamlIgnore]
		public float Right => pt.X + sz.Width;

		/// <summary>
		/// Gets or sets top-left point.
		/// </summary>
		[YamlIgnore]
		public Vector2 Location
		{
			get => pt;
			set => pt = value;
		}

		/// <summary>
		/// Gets or sets size.
		/// </summary>
		[YamlIgnore]
		public SizeF Size
		{
			get => sz;
			set => sz = value;
		}

		/// <summary>
		/// Returns true if the RectangleF contains a point.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool Contains(float x, float y)
		{
			return Contains(new Vector2(x, y));
		}
		/// <summary>
		/// Returns true if the RectangleF contains a point.
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public bool Contains(Vector2 pt)
		{
			if (pt.X < Left)
				return false;
			if (pt.Y < Top)
				return false;
			if (pt.X > Right)
				return false;
			if (pt.Y > Bottom)
				return false;

			return true;
		}

		/// <summary>
		/// Returns true if the RectangleF entirely contains the specified RectangleF.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public bool Contains(RectangleF rect)
		{
			if (Contains(rect.Location) && Contains(Vector2.Add(rect.Location, (Vector2)rect.Size)))
				return true;
			else
				return false;
		}


		#region --- Operator Overloads ---

		/// <summary>
		/// Equality comparison test.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator ==(RectangleF a, RectangleF b)
		{
			return a.Equals(b);
		}
		/// <summary>
		/// Inequality comparison test.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator !=(RectangleF a, RectangleF b)
		{
			return !a.Equals(b);
		}

		/// <summary>
		/// Conversion to Rectangle structure.
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		public static explicit operator Rectangle(RectangleF r)
		{
			return new Rectangle((int)r.X, (int)r.Y, (int)r.Width, (int)r.Height);
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
		/// <param name="destination"></param>
		public void WriteToPolygon(Polygon destination)
		{
			destination.Points.Count = 4;

			destination[0] = new Microsoft.Xna.Framework.Vector2(Left, Top);
			destination[1] = new Microsoft.Xna.Framework.Vector2(Right, Top);
			destination[2] = new Microsoft.Xna.Framework.Vector2(Right, Bottom);
			destination[3] = new Microsoft.Xna.Framework.Vector2(Left, Bottom);
		}

		#endregion
		#region --- Object Overrides ---

		/// <summary>
		/// Gets a hash code.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return pt.GetHashCode() ^ sz.GetHashCode();
		}

		/// <summary>
		/// Creates a string representing this RectangleF.
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
			if (obj is RectangleF)
				return Equals((RectangleF)obj);
			else
				return base.Equals(obj);
		}
		/// <summary>
		/// Equality test.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public bool Equals(RectangleF obj)
		{
			if (pt == obj.pt && sz == obj.sz)
				return true;
			else
				return false;
		}

		#endregion

		/// <summary>
		/// Returns a rectangle contracted by the specified amount.  The center of the rectangle remains in the same place,
		/// so this adds amount to X and Y, and subtracts amount*2 from Width and Height.
		/// </summary>
		/// <param name="amount"></param>
		public RectangleF Contract(float amount)
		{
			return Expand(-amount);
		}

		/// <summary>
		/// Returns a rectangle contracted by the specified amount.  The center of the rectangle remains in the same place,
		/// so this adds amount to X and Y, and subtracts amount*2 from Width and Height.
		/// </summary>
		/// <param name="amount"></param>
		public RectangleF Contract(SizeF amount)
		{
			return Expand(new SizeF(-amount.Width, -amount.Height));
		}

		/// <summary>
		///  Returns a rectangle contracted or expanded by the specified amount.  
		///  The center of the rectangle remains in the same place,
		/// so this subtracts amount to X and Y, and adds amount*2 from Width and Height.
		/// </summary>
		/// <param name="amount">The amount to expand the rectangle by. If this value is negative,
		/// the rectangle is contracted.</param>
		public RectangleF Expand(float amount)
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
		public RectangleF Expand(SizeF amount)
		{
			var result = this;

			result.X -= amount.Width;
			result.Y -= amount.Height;
			result.Width += amount.Width * 2;
			result.Height += amount.Height * 2;

			return result;
		}

		/// <summary>
		/// Returns true if this intersects another RectangleF.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public bool IntersectsWith(RectangleF rect)
		{
			if (this.Left > rect.Right) return false;
			if (rect.Left > this.Right) return false;
			if (this.Top > rect.Bottom) return false;
			if (rect.Top > this.Bottom) return false;

			return true;
		}

		/// <summary>
		/// Returns true if all members of the rectangle are zero.
		/// </summary>
		[YamlIgnore]
		public bool IsEmpty => pt.X == 0 && pt.Y == 0 && sz.IsZero;

		/// <summary>
		/// Empty RectangleF
		/// </summary>
		public static readonly RectangleF Empty = new RectangleF(0, 0, 0, 0);

		/// <summary>
		/// Static method returning the intersection of two RectangleFs.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static RectangleF Intersection(RectangleF a, RectangleF b)
		{
			if (a.IntersectsWith(b))
			{
				return RectangleF.FromLTRB(
					Math.Max(a.Left, b.Left),
					Math.Max(a.Top, b.Top),
					Math.Min(a.Right, b.Right),
					Math.Min(a.Bottom, b.Bottom));
			}
			else
				return Empty;
		}

		[Obsolete("Use RectangleF.Intersection instead.", true)]
		public static RectangleF Intersect(RectangleF a, RectangleF b)
		{
			return Intersection(a, b);
		}
	}
}
