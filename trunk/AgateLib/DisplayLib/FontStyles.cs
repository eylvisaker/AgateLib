using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Enumeration which allows selection of font styles when creating
	/// a font from the OS.  This enum has the FlagsAttribute, so its members
	/// can be combined in a bitwise fashion.
	/// </summary>
	[Flags]
	public enum FontStyles
	{
		/// <summary>
		/// No style is applied.
		/// </summary>
		None = 0,
		/// <summary>
		/// Make the font bold.
		/// </summary>
		Bold = 1,
		/// <summary>
		/// Use italics.
		/// </summary>
		Italic = 2,
		/// <summary>
		/// Strikeout through the font glyphs.
		/// </summary>
		Strikeout = 4,
		/// <summary>
		/// Underline beneath the glyphs.
		/// </summary>
		Underline = 8,
	}

}
