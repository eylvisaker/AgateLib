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

//using AgateLib.Display.ImplementationBase;
//using AgateLib.Resources;
//using AgateLib.Display.Cache;
//using AgateLib.IO;
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AgateLib.Display.BitmapFont
{
    /// <summary>
    /// Provides a basic implementation for the use of non-system fonts provided
    /// as a bitmap.
    /// 
    /// To construct a bitmap font, call the appropriate static FontSurface method.
    /// </summary>
    public class BitmapFontTexture : IFontTexture
    {
        private Texture2D texture;
        private FontMetrics mFontMetrics;
        private int mCharHeight;

        #region --- Cache Class ---

        private class BitmapFontCache : IFontStateCache
        {
            public bool NeedsRefresh = true;
            public Rectangle[] SrcRects;
            public Rectangle[] DestRects;
            public int DisplayTextLength;

            public IFontStateCache Clone()
            {
                BitmapFontCache cache = new BitmapFontCache();

                cache.SrcRects = (Rectangle[])SrcRects.Clone();
                cache.DestRects = (Rectangle[])DestRects.Clone();

                return cache;
            }

            public void OnTextChanged(FontState fontState)
            {
                NeedsRefresh = true;
            }
            public void OnDisplayAlignmentChanged(FontState fontState)
            {
                NeedsRefresh = true;
            }
            public void OnLocationChanged(FontState fontState)
            {
                NeedsRefresh = true;
            }

            public void OnColorChanged(FontState fontState)
            {
            }

            public void OnScaleChanged(FontState fontState)
            {
            }

            public void OnSizeChanged(FontState fontState)
            {
            }

            public void OnStyleChanged(FontState fontState)
            {
            }
        }

        #endregion

        /// <summary>
        /// Constructs a BitmapFontImpl, taking the passed surface as the source for
        /// the characters.  The source rectangles for each character are passed in.
        /// </summary>
        /// <param name="texture">Texture which contains the image data for the font glyphs.</param>
        /// <param name="fontMetrics">FontMetrics structure which describes how characters
        /// are laid out.</param>
        /// <param name="name">The name of the font.</param>
        public BitmapFontTexture(Texture2D texture, FontMetrics fontMetrics, string name)
        {
            FontName = name;
            mFontMetrics = fontMetrics.Clone();
            float maxHeight = 0;

            foreach (var kvp in mFontMetrics)
            {
                if (kvp.Value.SourceRect.Height > maxHeight)
                    maxHeight = kvp.Value.SourceRect.Height;
            }

            mCharHeight = (int)Math.Ceiling(maxHeight);
            this.texture = texture;
        }

        public string FontName { get; set; }

        /// <summary>
        /// Gets the font metric information.
        /// </summary>
        /// <returns></returns>
        public FontMetrics FontMetrics => mFontMetrics;

        /// <summary>
        /// Gets the surface containing the glyphs.
        /// </summary>
        /// <returns></returns>
        public Texture2D Texture => texture;

        /// <summary>
        /// Measures the string based on how it would be drawn with the
        /// specified FontState object.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public Size MeasureString(FontState state, string text)
        {
            if (string.IsNullOrEmpty(text))
                return Size.Empty;

            int carriageReturnCount = 0;
            int i = 0;
            double highestLineWidth = 0;

            // measure width
            string[] lines = text.Split('\n');
            for (i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                double lineWidth = 0;

                for (int j = 0; j < line.Length; j++)
                {
                    if (mFontMetrics.ContainsKey(line[j]) == false)
                        continue;

                    lineWidth += mFontMetrics[line[j]].LayoutWidth;
                }

                if (lineWidth > highestLineWidth)
                    highestLineWidth = lineWidth;
            }

            // measure height
            i = 0;
            do
            {
                i = text.IndexOf('\n', i + 1);

                if (i == -1)
                    break;

                carriageReturnCount++;

            } while (i != -1);

            if (text[text.Length - 1] == '\n')
                carriageReturnCount--;

            return new Size((int)Math.Ceiling(highestLineWidth * state.ScaleWidth),
                (int)(mCharHeight * (carriageReturnCount + 1) * state.ScaleHeight));
        }

        /// <summary>
        /// Returns the height of characters in the font.
        /// </summary>
        public int FontHeight(FontState state)
        {
            return (int)(mCharHeight * state.ScaleHeight);
        }

        private void GetRects(Rectangle[] srcRects, Rectangle[] destRects, out int rectCount,
            string text, double ScaleHeight, double ScaleWidth)
        {
            double destX = 0;
            double destY = 0;
            int height = mCharHeight;

            rectCount = 0;

            for (int i = 0; i < text.Length; i++)
            {
                switch (text[i])
                {
                    case '\r':
                        // ignore '\r' characters that are followed by '\n', because
                        // the line break on Windows is these two characters in succession.
                        if (i + 1 < text.Length && text[i + 1] == '\n')
                        {
                            break;
                        }

                        // this '\r' is not followed by a '\n', so treat it like any other character.
                        goto default;

                    case '\n':
                        destX = 0;
                        destY += height * ScaleHeight;
                        break;

                    default:
                        if (mFontMetrics.ContainsKey(text[i]) == false)
                        {
                            break;
                        }
                        GlyphMetrics glyph = mFontMetrics[text[i]];

                        destX = Math.Max(0, destX - glyph.LeftOverhang * ScaleWidth);

                        srcRects[rectCount] = new Rectangle(
                            glyph.SourceRect.X, glyph.SourceRect.Y,
                            glyph.SourceRect.Width, glyph.SourceRect.Height);

                        destRects[rectCount] = new Rectangle((int)destX, (int)destY,
                            (int)(srcRects[rectCount].Width * ScaleWidth),
                            (int)(srcRects[rectCount].Height * ScaleHeight));

                        destX += destRects[rectCount].Width - glyph.RightOverhang * ScaleWidth;

                        // check for kerning
                        if (i < text.Length - 1)
                        {
                            if (glyph.KerningPairs.ContainsKey(text[i + 1]))
                            {
                                destX += glyph.KerningPairs[text[i + 1]] * ScaleWidth;
                            }
                        }

                        rectCount++;
                        break;
                }
            }
        }

        private static BitmapFontCache GetCache(FontState state)
        {
            BitmapFontCache cache = state.Cache as BitmapFontCache;

            if (cache == null)
            {
                cache = new BitmapFontCache();
                state.Cache = cache;
            }

            if (cache.SrcRects == null ||
                cache.SrcRects.Length < state.Text.Length)
            {
                cache.SrcRects = new Rectangle[state.Text.Length];
                cache.DestRects = new Rectangle[state.Text.Length];
            }

            return cache;
        }

        public void DrawText(FontState state, SpriteBatch spriteBatch)
        {
            if (string.IsNullOrEmpty(state.Text))
                return;

            BitmapFontCache cache = GetCache(state);

            RefreshCache(state, cache);

            for (int i = 0; i < cache.DisplayTextLength; i++)
            {
                spriteBatch.Draw(
                    texture,
                    cache.DestRects[i],
                    cache.SrcRects[i],
                    state.Color);
            }
        }

        private void RefreshCache(FontState state, BitmapFontCache cache)
        {
            if (cache.NeedsRefresh == false)
                return;

            // this variable counts the number of rectangles actually used to display text.
            // It may be less then text.Length because carriage return characters 
            // don't need any rects.
            GetRects(cache.SrcRects, cache.DestRects, out cache.DisplayTextLength,
                state.Text, state.ScaleHeight, state.ScaleWidth);

            Vector2 dest = state.Location;

            if (state.TextAlignment != OriginAlignment.TopLeft)
            {
                Point value = Origin.Calc(state.TextAlignment,
                    MeasureString(state, state.Text));

                dest.X -= value.X;
                dest.Y -= value.Y;
            }

            for (int i = 0; i < cache.DisplayTextLength; i++)
            {
                cache.DestRects[i].X += (int)dest.X;
                cache.DestRects[i].Y += (int)dest.Y;
            }

            cache.NeedsRefresh = false;
        }
    }
}