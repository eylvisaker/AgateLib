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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using Drawing = System.Drawing;
using System.Text;
using System.Xml;

using ERY.AgateLib.Geometry;
using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib.BitmapFont
{
    /// <summary>
    /// Provides a basic implementation for the use of non-system fonts provided
    /// as a bitmap.
    /// 
    /// To construct a bitmap font, call the appropriate static FontSurface method.
    /// </summary>
    public class BitmapFontImpl : FontSurfaceImpl
    {
        Surface mSurface;

        FontMetrics mFontMetrics;

        int mCharHeight;
        double mAverageCharWidth;

        /// <summary>
        /// Constructs a BitmapFontImpl, assuming the characters in the given file
        /// are all the same size, and are in their ASCII order.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="characterSize"></param>
        public BitmapFontImpl(string filename, Size characterSize)
        {
            mFontMetrics = new FontMetrics();

            mSurface = new Surface(filename);
            mCharHeight = characterSize.Height;

            ExtractMonoSpaceAsciiFont(characterSize);
        }

        /// <summary>
        /// Constructs a BitmapFontImpl, taking the passed surface as the source for
        /// the characters.  The source rectangles for each character are passed in.
        /// </summary>
        /// <param name="surface">Surface which contains the image data for the font glyphs.</param>
        /// <param name="srcRects">An object implementing the IDictionary&lt;char, Rectangle&gt;
        /// interface, containing the source rectangles on the surface for each font glyph.</param>
        public BitmapFontImpl(Surface surface, FontMetrics fontMetrics)
        {
            mFontMetrics = fontMetrics.Clone();
            float maxHeight = 0;

            foreach (KeyValuePair<char, GlyphMetrics> kvp in mFontMetrics)
            {
                if (kvp.Value.SourceRect.Height > maxHeight)
                    maxHeight = kvp.Value.SourceRect.Height;
            }

            mCharHeight = (int)Math.Ceiling(maxHeight);
            mSurface = surface;
        }

        /// <summary>
        /// Disposes of the object.
        /// </summary>
        public override void Dispose()
        {
            mSurface.Dispose();
        }

        private void ExtractMonoSpaceAsciiFont(Size characterSize)
        {
            int x = 0;
            int y = 0;
            char val = '\0';

            while (y + characterSize.Height <= mSurface.SurfaceHeight)
            {
                Rectangle src = new Rectangle(x, y, characterSize.Width, characterSize.Height);

                mFontMetrics[val] = new GlyphMetrics(src);

                val++;
                x += characterSize.Width;

                if (x + characterSize.Width > mSurface.SurfaceWidth)
                {
                    y += characterSize.Height;
                    x = 0;
                }
            }

            CalcAverageCharWidth();
        }

        /// <summary>
        /// Saves the bitmap font to two files, an image file which contains the
        /// binary image data, and an XML file which contains all the glyph definitions.
        /// </summary>
        /// <param name="imageFilename"></param>
        /// <param name="xmlFileName"></param>
        public void Save(string imageFilename, string xmlFileName)
        {
            XmlDocument doc = new XmlDocument();

            SaveImage(imageFilename);
            mFontMetrics.Save(doc, doc);

            doc.Save(xmlFileName);
        }
        void SaveImage(string imageFilename)
        {
            mSurface.SaveTo(imageFilename);
        }

        /// <summary>
        /// Creates a bitmap font by loading an OS font, and drawing it to 
        /// a bitmap to use as a Surface object.  You should only use this method
        /// if writing a driver.
        /// </summary>
        /// <seealso cref="FontSurface.BitmapFont(string, float, FontStyle)"/>
        /// <param name="fontFamily"></param>
        /// <param name="sizeInPoints"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        [Obsolete("Use AgateWinForms.dll::BitmapFontUtil.FromOSFont instead.", true)]
        public static FontSurfaceImpl FromOSFont(string fontFamily, float sizeInPoints, FontStyle style)
        {
            throw new InvalidOperationException("BitmapFontImpl.FromOSFont is no longer valid.  The implementation has been moved to AgateWinForms.dll.");
        }

        private void CalcAverageCharWidth()
        {
            double total = 0;
            int count = 0;

            foreach (GlyphMetrics glyph in mFontMetrics.Values)
            {
                total += glyph.SourceRect.Width;
                count++;
            }

            mAverageCharWidth = total / (double)count;
        }

        /// <summary>
        /// Overrides the base Color method to catch color changes to set them on the surface.
        /// </summary>
        public override Color Color
        {
            get
            {
                return base.Color;
            }
            set
            {
                base.Color = value;

                mSurface.Color = value;
            }
        }
        /// <summary>
        /// Measures the width of the text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public override int StringDisplayWidth(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            double highestLineWidth = 0;

            string[] lines = text.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].TrimEnd();
                double lineWidth = 0;

                for (int j = 0; j < line.Length; j++)
                {
                    lineWidth += mFontMetrics[line[j]].Width;
                }

                if (lineWidth > highestLineWidth)
                    highestLineWidth = lineWidth;

            }

            return (int)Math.Ceiling(highestLineWidth * ScaleWidth);
        }
        /// <summary>
        /// Measures the height of the text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public override int StringDisplayHeight(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            int CRcount = 0;
            int i = 0;

            do
            {
                i = text.IndexOf('\n', i + 1);

                if (i == -1)
                    break;

                CRcount++;

            } while (i != -1);

            if (text[text.Length - 1] == '\n')
                CRcount--;

            return (int)(mCharHeight * (CRcount + 1) * ScaleHeight);
        }
        /// <summary>
        /// Measures the size of the text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public override Size StringDisplaySize(string text)
        {
            return new Size(StringDisplayWidth(text), StringDisplayHeight(text));
        }

        private void GetRects(string text, RectangleF[] srcRects, RectangleF[] destRects)
        {
            double destX = 0;
            double destY = 0;
            int height = mCharHeight;

            for (int i = 0; i < text.Length; i++)
            {
                switch (text[i])
                {
                    case '\r':
                        // ignore '\r' characters that are followed by '\n', because
                        // the line break on Windows is these two characters in succession.
                        if (i + 1 < text.Length && text[i + 1] == '\n')
                            break;

                        // this '\r' is not followed by a '\n', so treat it like any other character.
                        goto default;

                    case '\n':
                        destX = 0;
                        destY += height * this.ScaleHeight;
                        break;

                    default:
                        GlyphMetrics glyph = mFontMetrics[text[i]];

                        destX = Math.Max(0, destX - glyph.LeftOverhang * ScaleWidth);

                        srcRects[i] = new RectangleF(
                            glyph.SourceRect.X, glyph.SourceRect.Y,
                            glyph.SourceRect.Width, glyph.SourceRect.Height);

                        destRects[i] = new RectangleF((float)destX, (float)destY,
                            (float)(srcRects[i].Width * ScaleWidth),
                            (float)(srcRects[i].Height * ScaleHeight));

                        destX += destRects[i].Width - glyph.RightOverhang * ScaleWidth;
                        break;
                }
            }
        }

        /// <summary>
        /// Draws the text to the screen.
        /// </summary>
        /// <param name="destX"></param>
        /// <param name="destY"></param>
        /// <param name="text"></param>
        public override void DrawText(int destX, int destY, string text)
        {
            RectangleF[] srcRects = new RectangleF[text.Length];
            RectangleF[] destRects = new RectangleF[text.Length];

            if (string.IsNullOrEmpty(text))
                return;

            GetRects(text, srcRects, destRects);

            if (DisplayAlignment != OriginAlignment.TopLeft)
            {
                Point value = Origin.Calc(DisplayAlignment, StringDisplaySize(text));

                destX -= value.X;
                destY -= value.Y;
            }

            for (int i = 0; i < destRects.Length; i++)
            {
                destRects[i].X += destX;
                destRects[i].Y += destY;
            }

            mSurface.DrawRects(srcRects, destRects);
        }

        /// <summary>
        /// Draws the text to the screen.
        /// </summary>
        /// <param name="destX"></param>
        /// <param name="destY"></param>
        /// <param name="text"></param>
        public override void DrawText(double destX, double destY, string text)
        {
            DrawText((int)destX, (int)destY, text);
        }

        /// <summary>
        /// Draws the text to the screen.
        /// </summary>
        /// <param name="destPt"></param>
        /// <param name="text"></param>
        public override void DrawText(Point destPt, string text)
        {
            DrawText(destPt.X, destPt.Y, text);
        }

        /// <summary>
        /// Draws the text to the screen.
        /// </summary>
        /// <param name="destPt"></param>
        /// <param name="text"></param>
        public override void DrawText(PointF destPt, string text)
        {
            DrawText(destPt.X, destPt.Y, text);
        }
        
    }

    /// <summary>
    /// Enum which indicates how pixels along glyph edges are processed.
    /// </summary>
    public enum BitmapFontEdgeOptions
    {
        /// <summary>
        /// Calculates the intensity of the pixel, and sets the pixel to white
        /// with an alpha value equal to its intensity.
        /// </summary>
        IntensityAlphaWhite,

        /// <summary>
        /// Preserves the color of the pixel, and sets the alpha to the intensity of
        /// the pixel.
        /// </summary>
        IntensityAlphaColor,

        /// <summary>
        /// Performs no processing on edges and leaves them as is.
        /// Note that this will result in edges which are not at all transparent.
        /// </summary>
        None,
    }


}