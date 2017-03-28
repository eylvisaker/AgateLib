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
	/// A structure with two properties, a width and height.
	/// </summary>
	[DataContract]
	public struct Size
	{
		[DataMember]
		int width, height;

		/// <summary>
		/// Constructs a Size.
		/// </summary>
		/// <param name="pt"></param>
		[DebuggerStepThrough]
		public Size(Point pt)
		{
			width = pt.X;
			height = pt.Y;
		}

		/// <summary>
		/// Constructs a Size.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		[DebuggerStepThrough]
		public Size(int width, int height)
		{
			this.width = width;
			this.height = height;
		}

		/// <summary>
		/// Gets or sets the width.
		/// </summary>
		public int Width
		{
			[DebuggerStepThrough]
			get { return width; }
			[DebuggerStepThrough]
			set { width = value; }
		}

		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		public int Height
		{
			[DebuggerStepThrough]
			get { return height; }
			[DebuggerStepThrough]
			set { height = value; }
		}

		/// <summary>
		/// Returns true if width and height are zero.
		/// </summary>
		public bool IsEmpty
		{
			[DebuggerStepThrough]
			get { return width == 0 && height == 0; }
		}

		/// <summary>
		/// Gets the aspect ratio (width / height) of this Size object.
		/// </summary>
		public double AspectRatio => width / (double)height;

		#region --- Operator Overloads ---

		/// <summary>
		/// Equality comparison test.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator ==(Size a, Size b)
		{
			return a.Equals(b);
		}
		/// <summary>
		/// Inequality comparison test.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator !=(Size a, Size b)
		{
			return !a.Equals(b);
		}

		/// <summary>
		/// Converts a Size to a Vector2.
		/// </summary>
		/// <param name="size"></param>
		public static explicit operator Vector2(Size size)
		{
			return new Vector2(size.Width, size.Height);
		}

		/// <summary>
		/// Converts a Size to a Vector2f.
		/// </summary>
		/// <param name="size"></param>
		public static explicit operator Vector2f(Size size)
		{
			return new Vector2f(size.Width, size.Height);
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
			if (obj is Size)
				return Equals((Size)obj);
			else
				return base.Equals(obj);
		}
		/// <summary>
		/// Equality test.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public bool Equals(Size obj)
		{
			if (width == obj.width && height == obj.height)
				return true;
			else
				return false;
		}

		#endregion

		/// <summary>
		/// Empty Size.
		/// </summary>
		public static readonly Size Empty = new Size(0, 0);

		/// <summary>
		/// Rounds the SizeF structure up.
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public static Size Ceiling(SizeF a)
		{
			return new Size((int)Math.Ceiling(a.Width), (int)Math.Ceiling(a.Height));
		}

		/// <summary>
		/// Explicit conversion to SizeF structure.
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public static explicit operator SizeF(Size size)
		{
			return new SizeF(size.width, size.height);
		}

		/// <summary>
		/// Parses a string into a size object.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static Size FromString(string text)
		{
			if (text.StartsWith("{") && text.EndsWith("}"))
			{
				text = text.Substring(1, text.Length - 2);
			}

			string[] values = text.Split(',');
			Size result = new Size();

			if (values.Length == 1 && text.Contains("x") && text.Contains("=") == false)
				values = text.Split('x');
			if (values.Length != 2)
				throw new FormatException("Could not parse size data from text.");

			if (text.Contains("="))
			{
				// parse named arguments
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i].ToLowerInvariant().Contains("width")
						&& values[i].Contains("="))
					{
						int equals = values[i].IndexOf("=", StringComparison.OrdinalIgnoreCase);

						result.Width = int.Parse(values[i].Substring(equals + 1), System.Globalization.CultureInfo.CurrentCulture);
					}
					else if (values[i].ToLowerInvariant().Contains("height")
						&& values[i].Contains("="))
					{
						int equals = values[i].IndexOf('=');

						result.Height = int.Parse(values[i].Substring(equals + 1));
					}
				}
			}
			else
			{
				result.Width = int.Parse(values[0], System.Globalization.CultureInfo.InvariantCulture);
				result.Height = int.Parse(values[1], System.Globalization.CultureInfo.InvariantCulture);
			}

			return result;
		}

	}
}