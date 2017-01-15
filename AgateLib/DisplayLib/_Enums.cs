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


	/// <summary>
	/// Enum indicating how images are laid out when drawing inline with text.
	/// </summary>
	public enum TextImageLayout
	{
		/// <summary>
		/// The top of the image is aligned with the top of the text.
		/// </summary>
		InlineTop,
		/// <summary>
		/// The center of the image is aligned with the center of the text.
		/// </summary>
		InlineCenter,
		/// <summary>
		/// The bottom of the image is aligned with the bottom of the text.
		/// </summary>
		InlineBottom,
	}

	/// <summary>
	/// Enum which describes what position the window should be created at on screen.
	/// </summary>
	public enum WindowPosition
	{
		/// <summary>
		/// Lets AgateLib choose where to position the window.  
		/// </summary>
		DefaultAgate,

		/// <summary>
		/// Let the runtime decide where the window is placed.
		/// </summary>
		DefaultOS,

		/// <summary>
		/// Center the window horizontally on screen, but vertically above center.
		/// This often looks better because the vertical center of the monitor is usually 
		/// positioned below eye-level.
		/// </summary>
		AboveCenter,

		/// <summary>
		/// Center the window on the screen.
		/// </summary>
		CenterScreen,
	}

}
