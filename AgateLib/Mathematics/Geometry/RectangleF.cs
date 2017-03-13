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
using System.Runtime.Serialization;

namespace AgateLib.Mathematics.Geometry
{
	/// <summary>
	/// Replacement for System.Drawing.RectangleF structure.
	/// </summary>
	[DataContract]
	public struct RectangleF 
	{
		[DataMember]
		PointF pt;
		[DataMember]
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
			this.pt = new PointF(x, y);
			this.sz = new SizeF(width, height);
		}
		/// <summary>
		/// Construts a RectangleF.
		/// </summary>
		/// <param name="pt"></param>
		/// <param name="sz"></param>
		public RectangleF(Vector2 pt, SizeF sz)
		{
			this.pt = (PointF)pt;
			this.sz = sz;
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
			get { return pt.X; }
			set { pt.X = value; }
		}
		/// <summary>
		/// Y value
		/// </summary>
		public float Y
		{
			get { return pt.Y; }
			set { pt.Y = value; }
		}

		/// <summary>
		/// Width
		/// </summary>
		public float Width
		{
			get { return sz.Width; }
			set { sz.Width = value; }
		}

		/// <summary>
		/// Height
		/// </summary>
		public float Height
		{
			get { return sz.Height; }
			set { sz.Height = value; }
		}

		/// <summary>
		/// Gets bottom.
		/// </summary>
		public float Bottom
		{
			get { return pt.Y + sz.Height; }
		}

		/// <summary>
		/// Gets left.
		/// </summary>
		public float Left
		{
			get { return pt.X; }
		}

		/// <summary>
		/// Gets top.
		/// </summary>
		public float Top
		{
			get { return pt.Y; }
		}

		/// <summary>
		/// Gets right.
		/// </summary>
		public float Right
		{
			get { return pt.X + sz.Width; }
		}

		/// <summary>
		/// Gets or sets top-left point.
		/// </summary>
		public PointF Location
		{
			get { return pt; }
			set { pt = value; }
		}

		/// <summary>
		/// Gets or sets size.
		/// </summary>
		public SizeF Size
		{
			get { return sz; }
			set { sz = value; }
		}
		/// <summary>
		/// Returns true if the RectangleF contains a point.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool Contains(float x, float y)
		{
			return Contains(new PointF(x, y));
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
			if (Contains(rect.Location) && Contains(PointF.Add(rect.Location, rect.Size)))
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
		/// True if this is (0,0,0,0).
		/// </summary>
		
		public bool IsEmpty
		{
			get { return pt.IsEmpty && sz.IsZero; }
		}


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
