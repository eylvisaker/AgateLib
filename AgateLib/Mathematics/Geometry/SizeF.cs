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
using Microsoft.Xna.Framework;
using YamlDotNet.Serialization;

namespace AgateLib.Mathematics.Geometry
{
	/// <summary>
	/// SizeF structure.
	/// </summary>
	public struct SizeF  
	{
		float width, height;

		/// <summary>
		/// Constructs a SizeF structure.
		/// </summary>
		/// <param name="pt"></param>
		public SizeF(Vector2 pt)
		{
			width = pt.X;
			height = pt.Y;
		}

		/// <summary>
		/// Constructs a SizeF structure.
		/// </summary>
		/// <param name="height"></param>
		/// <param name="width"></param>
		public SizeF(float width, float height)
		{
			this.width = width;
			this.height = height;
		}

		/// <summary>
		/// Gets or sets the width.
		/// </summary>
		public float Width
		{
			get => width;
			set => width = value;
		}
		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		public float Height
		{
			get => height;
			set => height = value;
		}

		/// <summary>
		/// Gets the aspect ratio (width / height) of this Size object.
		/// </summary>
		[YamlIgnore]
		public double AspectRatio => width / height;

		/// <summary>
		/// True if width and height are zero.
		/// </summary>
		[YamlIgnore]
		public bool IsZero => width == 0 && height == 0;

		[Obsolete("Use IsZero instead.")]
		[YamlIgnore]
		public bool IsEmpty => IsZero;

		/// <summary>
		/// Empty SizeF structure.
		/// </summary>
		public static readonly SizeF Zero = new SizeF(0, 0);

		/// <summary>
		/// Empty SizeF structure.
		/// </summary>
		[Obsolete("Use Zero instead.")]
		public static readonly SizeF Empty = new SizeF(0, 0);

		/// <summary>
		/// Conversion to Size casts height and width to integers.
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public static explicit operator Size(SizeF size)
		{
			return new Size((int)size.width, (int)size.height);
		}

		#region --- Operator Overloads ---

		/// <summary>
		/// Equality comparison test.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator ==(SizeF a, SizeF b)
		{
			return a.Equals(b);
		}
		/// <summary>
		/// Inequality comparison test.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator !=(SizeF a, SizeF b)
		{
			return !a.Equals(b);
		}
		/// <summary>
		/// Converts a Size to a Vector2f.
		/// </summary>
		/// <param name="size"></param>
		public static explicit operator Vector2(SizeF size)
		{
			return new Vector2(size.Width, size.Height);
		}


		#endregion

		#region --- Object Overrides ---

		/// <summary>
		/// Converts to a string.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format(System.Globalization.CultureInfo.CurrentCulture,
				"(Width={0},Height={1})", width, height);
		}
		/// <summary>
		/// Gets a hash code.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return width.GetHashCode() ^ height.GetHashCode();
		}
		/// <summary>
		/// Equality test.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is SizeF)
				return Equals((SizeF)obj);
			else
				return base.Equals(obj);
		}
		/// <summary>
		/// Equality test.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public bool Equals(SizeF obj)
		{
			if (width == obj.width && height == obj.height)
				return true;
			else
				return false;
		}

		#endregion

	}
}
