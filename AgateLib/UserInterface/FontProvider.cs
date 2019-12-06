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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AgateLib.UserInterface
{
    public interface IFontProvider : IEnumerable<Font>
    {
        /// <summary>
        /// Gets the default font.
        /// </summary>
		Font Default { get; }

        /// <summary>
        /// Creates a font object for the specified font family.
        /// Returns the default font if the specified font family is not found.
        /// </summary>
        /// <param name="fontDescription">The description of a font. This can be a comma separate value like "Sans,14,bold" 
        /// to indicate the size and attributes. Any attributes are optional except the font family name. The
        /// family name must be first.</param>
        Font GetOrDefault(string fontDescription);

        /// <summary>
        /// Gets a font by font face name. Throws an exception if the font is not present.
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        Font this[string family] { get; }

        /// <summary>
        /// Returns true if the specified font is available.
        /// </summary>
        /// <param name="family">The name of a font family</param>
        /// <returns></returns>
        bool HasFont(string family);
    }

    /// <summary>
    /// Provides a prototypical implementation of IFontProvider.
    /// A good way to use this is to inherit from it and use the Add method in the constructor to add fonts.
    /// </summary>
    /// <example>
    /// [Singleton]
    /// public class GenerationsOfLoreFontProvider : FontProvider
    /// {
    ///     public GenerationsOfLoreFontProvider(IContentProvider content)
    ///     {
    ///         Add("MedievalSharp", Font.Load(content, "UserInterface/Fonts/MedievalSharp"));
    /// 
    ///         Add("Sans", Font.Load(content, "AgateLib/AgateSans"));
    ///         Add("Mono", Font.Load(content, "AgateLib/AgateMono"));
    /// 
    ///         Default.Size = 20;
    ///     }
    /// } 
    /// </example>
    public class FontProvider : IFontProvider
    {
        private static char[] descriptionSeparator = new char[] { ',', ' ' };

        private readonly Dictionary<string, Font> fonts
            = new Dictionary<string, Font>(StringComparer.OrdinalIgnoreCase);

        public Font this[string fontFace]
            => string.IsNullOrWhiteSpace(fontFace) ? Default : fonts[fontFace];

        public Font Default { get; set; }

        public bool HasFont(string family)
        {
            if (family == null)
                return false;

            return fonts.ContainsKey(family);
        }

        public void Add(string family, Font font)
        {
            fonts.Add(family, font);

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

        public Font GetOrDefault(string fontDescription)
        {
            if (string.IsNullOrWhiteSpace(fontDescription))
            {
                return new Font(Default);
            }

            string[] parts = fontDescription.Split(descriptionSeparator, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
                throw new ArgumentException(nameof(fontDescription));

            string family = parts[0];

            Font result = new Font(HasFont(family) ? this[family] : Default);

            foreach(string param in parts.Skip(1))
            {
                if (int.TryParse(param, NumberStyles.Integer, CultureInfo.InvariantCulture, out int size))
                    result.Size = size;
                else if (Enum.TryParse<FontStyles>(param, out FontStyles fontStyles))
                    result.Style = fontStyles;
            }

            return result;
        }
    }
}
