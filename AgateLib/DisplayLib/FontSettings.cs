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
