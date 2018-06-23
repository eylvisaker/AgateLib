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
using AgateLib.Display.BitmapFont;
using AgateLib.Quality;

namespace AgateLib.Display
{
	/// <summary>
	/// Constructs a font from a set of font surfaces.
	/// </summary>
	public class FontBuilder
	{
		private Font font;

		public FontBuilder(string name)
		{
			font = new Font(name);
		}

		/// <summary>
		/// Adds a font surface to the font.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="fontTexture"></param>
		/// <returns></returns>
		public FontBuilder AddFontTexture(FontSettings settings, IFontTexture fontTexture)
		{
			Require.That<InvalidOperationException>(font != null,
				"FontBuilder objects cannot be reused.");

			font.AddFontTexture(settings, fontTexture);

			return this;
		}

		/// <summary>
		/// Builds the resulting Font object.
		/// </summary>
		/// <returns></returns>
		public Font Build()
		{
			var result = font;

			font = null;

			return result;
		}
	}
}