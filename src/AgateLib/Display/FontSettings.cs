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

namespace AgateLib.Display
{
    /// <summary>
    /// Structure which encapsulates font size and style.
    /// </summary>
    public struct FontSettings : IEquatable<FontSettings>
    {
        /// <summary>
        /// Constructs a FontSettings object.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="style"></param>
        public FontSettings(int size, FontStyles style)
        {
            Size = size;
            Style = style;
        }

        /// <summary>
        /// Size of the font in points.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Style(s) of the font. Combinations like bold &amp; italic are possible.
        /// </summary>
        public FontStyles Style { get; set; }

        /// <summary>
        /// Returns true if the two FontSettings objects are equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(FontSettings other)
        {
            if (Size != other.Size)
            {
                return false;
            }

            if (Style != other.Style)
            {
                return false;
            }

            return true;
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
                return $"{Size}-{Style}";
            }
            else
            {
                return Size.ToString();
            }
        }
    }
}
