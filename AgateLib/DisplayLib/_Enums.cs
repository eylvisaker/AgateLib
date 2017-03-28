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

	/// <summary>
	/// Types of lines that Display.Primitives.DrawLines can render.
	/// </summary>
	public enum LineType
	{
		/// <summary>
		/// Indicates that the points trace the perimeter of a polygon.
		/// The first and last points will be connected.
		/// </summary>
		Polygon,

		/// <summary>
		/// Indicates that the points trace a path. Each point will be 
		/// connected to the next, but the first and last points will not be
		/// connected.
		/// </summary>
		Path,

		/// <summary>
		/// Indicates each pair of points in the list should be drawn independently.
		/// No lines are connected unless the points are duplicated.
		/// </summary>
		LineSegments,
	}
}
