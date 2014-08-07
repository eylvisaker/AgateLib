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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AgateLib.Geometry
{
	/// <summary>
	/// A structure with two properties, a width and height.
	/// </summary>
	public partial struct Size
	{
		int width, height;

		/// <summary>
		/// Constructs a Size.
		/// </summary>
		/// <param name="pt"></param>
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
			get { return width; }
			set { width = value; }
		}
		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		public int Height
		{
			get { return height; }
			set { height = value; }
		}

		/// <summary>
		/// Returns true if width and height are zero.
		/// </summary>
		
		public bool IsEmpty
		{
			get { return width == 0 && height == 0; }
		}

		/// <summary>
		/// Gets the aspect ratio (width / height) of this Size object.
		/// </summary>
		public double AspectRatio
		{
			get { return width / (double)height; }
		}

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

		#endregion

		#region --- Object Overrides ---

		/// <summary>
		/// Converts to a string.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format(System.Globalization.CultureInfo.CurrentCulture,
				"{0}Width={1},Height={2}{3}", "{", width, height, "}");
		}
		/// <summary>
		/// Gets a hash code.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return width.GetHashCode() + height.GetHashCode();
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
		public static Size FromString(string str)
		{
			if (str.StartsWith("{") && str.EndsWith("}"))
			{
				str = str.Substring(1, str.Length - 2);
			}

			string[] values = str.Split(',');
			Size retval = new Size();

			if (values.Length == 1 && str.Contains("x") && str.Contains("=") == false)
				values = str.Split('x');
			if (values.Length != 2)
				throw new FormatException("Could not parse size data from text.");

			if (str.Contains("="))
			{
				// parse named arguments
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i].ToLowerInvariant().Contains("width")
						&& values[i].Contains("="))
					{
						int equals = values[i].IndexOf("=", StringComparison.OrdinalIgnoreCase);

						retval.Width = int.Parse(values[i].Substring(equals + 1), System.Globalization.CultureInfo.CurrentCulture);
					}
					else if (values[i].ToLowerInvariant().Contains("height")
						&& values[i].Contains("="))
					{
						int equals = values[i].IndexOf('=');

						retval.Height = int.Parse(values[i].Substring(equals + 1));
					}
				}
			}
			else
			{
				retval.Width = int.Parse(values[0], System.Globalization.CultureInfo.InvariantCulture);
				retval.Height = int.Parse(values[1], System.Globalization.CultureInfo.InvariantCulture);
			}
			return retval;
		}

	}
}