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

using AgateLib.Display;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace AgateLib.UserInterface
{
	public interface IFontProvider : IEnumerable<Font>
	{
		Font Default { get; }

		/// <summary>
		/// Gets a font by font face name. Throws an exception if the font is not present.
		/// </summary>
		/// <param name="fontFace"></param>
		/// <returns></returns>
		Font this[string fontFace] { get; }

		/// <summary>
		/// Returns true if the specified font is available.
		/// </summary>
		/// <param name="fontFace"></param>
		/// <returns></returns>
		bool HasFont(string fontFace);
	}

	[Singleton]
	public class FontProvider : IFontProvider
	{
		private readonly Dictionary<string, Font> fonts 
			= new Dictionary<string, Font>(StringComparer.OrdinalIgnoreCase);
		
		public Font this[string fontFace] 
			=> string.IsNullOrWhiteSpace(fontFace) ? Default : fonts[fontFace];

		public Font Default { get; set; }

		public bool HasFont(string fontFace)
		{
			return fonts.ContainsKey(fontFace);
		}

		public void Add(string fontFace, Font font)
		{
			fonts.Add(fontFace, font);

			if (Default == null)
				Default = font;
		}

		public IEnumerator<Font> GetEnumerator()
		{
			return fonts.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
