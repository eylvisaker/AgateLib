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

using AgateLib.Mathematics.Geometry;
using AgateLib.Quality;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AgateLib.Display.BitmapFont
{
    public interface IFontCore
    {
        string Name { get; }

        IReadOnlyDictionary<FontSettings, IFontTexture> FontItems { get; }

        int FontHeight(FontState state);

        void DrawText(FontState state, SpriteBatch spriteBatch, Vector2 dest, string text);

        Size MeasureString(FontState state, string text);

        [Obsolete("This should not be part of the interface.")]
        void AddFontSurface(FontSettings settings, IFontTexture fontSurface);

        FontSettings GetClosestFontSettings(FontSettings settings);

        IFontTexture FontSurface(FontState fontState);
    }

    internal class FontCore : IFontCore
    {
        private Dictionary<FontSettings, IFontTexture> fontTextures = new Dictionary<FontSettings, IFontTexture>();

        public FontCore(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public IReadOnlyDictionary<FontSettings, IFontTexture> FontItems => fontTextures;

        public void AddFontSurface(FontSettings settings, IFontTexture fontSurface)
        {
            Require.ArgumentNotNull(fontSurface, nameof(fontSurface));

            fontTextures[settings] = fontSurface;
        }

        public IFontTexture GetFontSurface(int size, FontStyles fontStyles)
        {
            return GetFontSurface(new FontSettings(size, fontStyles));
        }
        public IFontTexture GetFontSurface(FontSettings settings)
        {
            return fontTextures[settings];
        }

        public int FontHeight(FontState state)
        {
            var surface = FontSurface(state);
            return surface.FontHeight(state);
        }

        private int MaxSize(FontStyles style)
        {
            var keys = fontTextures.Keys.Where(x => x.Style == style);
            if (keys.Any())
                return keys.Max(x => x.Size);
            else
                return -1;
        }

        #region --- Finding correctly sized font ---

        public IFontTexture FontSurface(FontState state)
        {
            var settings = GetClosestFontSettings(state.Settings);
            var result = fontTextures[settings];

            var ratio = state.Settings.Size / (double)settings.Size;

            state.ScaleHeight = ratio;
            state.ScaleWidth = ratio;

            return result;
        }

        internal FontSettings GetClosestFontSettings(int size, FontStyles style)
        {
            return GetClosestFontSettings(new FontSettings(size, style));
        }
        public FontSettings GetClosestFontSettings(FontSettings settings)
        {
            if (fontTextures.ContainsKey(settings))
                return settings;

            int maxSize = MaxSize(settings.Style);

            // this happens if we have no font surfaces of this style.
            if (maxSize <= 0)
            {
                FontStyles newStyle;

                // OK remove styles until we find an actual font.
                if (TryRemoveStyle(settings.Style, FontStyles.Strikeout, out newStyle))
                    return GetClosestFontSettings(settings.Size, newStyle);
                if (TryRemoveStyle(settings.Style, FontStyles.Italic, out newStyle))
                    return GetClosestFontSettings(settings.Size, newStyle);
                if (TryRemoveStyle(settings.Style, FontStyles.Underline, out newStyle))
                    return GetClosestFontSettings(settings.Size, newStyle);
                if (TryRemoveStyle(settings.Style, FontStyles.Bold, out newStyle))
                    return GetClosestFontSettings(settings.Size, newStyle);
                else
                {
                    Debug.Assert(fontTextures.Count == 0);
                    throw new InvalidOperationException("There are no font styles defined.");
                }
            }

            if (settings.Size > maxSize)
                return GetClosestFontSettings(maxSize, settings.Style);

            for (int i = settings.Size; i <= maxSize; i++)
            {
                settings.Size = i;

                if (fontTextures.ContainsKey(settings))
                    return settings;
            }

            throw new InvalidOperationException("Could not find a valid font.");
        }


        #endregion

        private bool TryRemoveStyle(FontStyles value, FontStyles remove, out FontStyles result)
        {
            if ((value & remove) == remove)
            {
                result = ~(~value | remove);
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }

        public void DrawText(FontState state, SpriteBatch spriteBatch, Vector2 dest, string text)
        {
            state.Location = dest;
            state.Text = text;

            FontSurface(state).DrawText(state, spriteBatch);
        }

        public Size MeasureString(FontState state, string text)
        {
            return FontSurface(state).MeasureString(state, text);
        }
    }
}