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
using System.Runtime.Serialization;
using System.Text;

namespace AgateLib.Geometry
{
	/// <summary>
	/// SizeF structure.
	/// </summary>
	[DataContract]
	public struct SizeF  
	{
		[DataMember]
		float width, height;

		/// <summary>
		/// Constructs a SizeF structure.
		/// </summary>
		/// <param name="pt"></param>
		public SizeF(PointF pt)
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
			get { return width; }
			set { width = value; }
		}
		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		public float Height
		{
			get { return height; }
			set { height = value; }
		}

		/// <summary>
		/// Gets the aspect ratio (width / height) of this Size object.
		/// </summary>
		public double AspectRatio
		{
			get { return width / height; }
		}

		/// <summary>
		/// True if width and height are zero.
		/// </summary>
		
		public bool IsEmpty
		{
			get { return width == 0 && height == 0; }
		}
		/// <summary>
		/// Empty SizeF structure.
		/// </summary>
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
