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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Structure which encapsulates font size and style.
	/// </summary>
	public struct FontSettings : IEquatable<FontSettings>
	{
		int mSize;
		FontStyles mStyle;

		/// <summary>
		/// Size of the font in points.
		/// </summary>
		public int Size { get { return mSize; } set { mSize = value; } }
		/// <summary>
		/// Style(s) of the font. Combinations like bold&italic are possible.
		/// </summary>
		public FontStyles Style { get { return mStyle; } set { mStyle = value; } }

		/// <summary>
		/// Returns true if the two FontSettings objects are equal.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(FontSettings other)
		{
			if (Size != other.Size)
				return false;
			if (Style != other.Style)
				return false;

			return true;
		}

		/// <summary>
		/// Constructs a FontSettings object.
		/// </summary>
		/// <param name="size"></param>
		/// <param name="style"></param>
		public FontSettings(int size, FontStyles style)
		{
			mSize = size;
			mStyle = style;
		}

		/// <summary>
		/// Returns the full name of a font with these settings.
		/// </summary>
		/// <param name="family"></param>
		/// <returns></returns>
		public string FontName(string family)
		{
			return family + "-" + ToString();
		}

		/// <summary>
		/// Converts to a string representation.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if (Style != FontStyles.None)
			{
				return string.Format("{0}-{1}", Size, Style);
			}
			else
				return Size.ToString();
		}
	}
}
