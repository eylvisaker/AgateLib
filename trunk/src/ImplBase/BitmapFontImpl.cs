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

namespace ERY.AgateLib.ImplBase
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

        /// <summary>
        /// Stores source rectangles for all characters.
        /// </summary>
        Dictionary<char, RectangleF> mSrcRects = new Dictionary<char, RectangleF>();

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
        public BitmapFontImpl(Surface surface, IDictionary<char, RectangleF> srcRects)
        {
            float maxHeight = 0;

            foreach (KeyValuePair<char, RectangleF> kvp in srcRects)
            {
                mSrcRects.Add(kvp.Key, kvp.Value);

                if (kvp.Value.Height > maxHeight)
                    maxHeight = kvp.Value.Height;
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
                RectangleF src = new RectangleF(x, y, characterSize.Width, characterSize.Height);

                mSrcRects[val] = src;

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

            Save(imageFilename, doc, doc);

            doc.Save(xmlFileName);
        }
        private void Save(string imageFilename, XmlNode node, XmlDocument doc)
        {
            mSurface.SaveTo(imageFilename);

            XmlNode root = doc.CreateElement("Font");

            foreach (char glyph in mSrcRects.Keys)
            {
                XmlNode current = doc.CreateElement("Glyph");

                XmlAttribute ch = doc.CreateAttribute("Char");
                ch.Value = glyph.ToString();

                XmlAttribute rect = doc.CreateAttribute("Source");
                rect.Value = mSrcRects[glyph].ToString();

                current.Attributes.Append(ch);
                current.Attributes.Append(rect);

                root.AppendChild(current);
            }

            node.AppendChild(root);
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
        public static FontSurfaceImpl FromOSFont(string fontFamily, float sizeInPoints, FontStyle style)
        {
            System.Drawing.FontStyle drawingStyle = System.Drawing.FontStyle.Regular;

            if ((style & FontStyle.Bold) > 0) drawingStyle |= System.Drawing.FontStyle.Bold;
            if ((style & FontStyle.Italic) > 0) drawingStyle |= System.Drawing.FontStyle.Italic;
            if ((style & FontStyle.Strikeout) > 0) drawingStyle |= System.Drawing.FontStyle.Strikeout;
            if ((style & FontStyle.Underline) > 0) drawingStyle |= System.Drawing.FontStyle.Underline;

            Drawing.Font font = new Drawing.Font(fontFamily, sizeInPoints, drawingStyle);

            Drawing.Bitmap bmp = new System.Drawing.Bitmap(512, 512);
            Drawing.Graphics g = Drawing.Graphics.FromImage(bmp);

            // first calculate the size of image we need
            Dictionary<char, RectangleF> glyphs = new Dictionary<char, RectangleF>();

            char startChar = ' ';
            char endChar = (char)256;

            // amount of space between characters on the texture.
            int bitmapPadding = 2;

            float x = 0, y = 0;
            float height = 0;
            float padding = 0;

            // apparently .NET does this stupid thing on Windows
            // where is reports extra padded space around the characters drawn.
            // Fortunately, this padding is equal the reported size of the
            // space character, which is not drawn when by itself.
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SizeF padSize = new SizeF(g.MeasureString(" ", font));

                padding = padSize.Width - 1;
            }
            for (char i = startChar; i < endChar; i++)
            {
                SizeF size = new SizeF(g.MeasureString(i.ToString(), font));
                size.Width -= padding;

                // for space character on windows.
                if (i == ' ' && padding > 0.0)
                    size.Width = padding;

                glyphs[i] = new RectangleF(0, 0, size.Width, size.Height);

                x += (float)Math.Ceiling(glyphs[i].Width) + bitmapPadding;

                if (glyphs[i].Height > height)
                    height = glyphs[i].Height;

                if (x > 512)
                {
                    x = glyphs[i].Width;
                    y += (float)Math.Ceiling(height + 1);
                    height = 0;
                }
            }

            int maxHeight = (int)y;
            if (maxHeight > 512)
            {
                g.Dispose();
                bmp.Dispose();

                bmp = new System.Drawing.Bitmap(512, 1024);
                g = Drawing.Graphics.FromImage(bmp);
            }

            Drawing.Brush brush = Drawing.Brushes.White;

            x = 0;
            y = 0;
            height = 0;

            for (char i = startChar; i < endChar; i++)
            {
                if (x + glyphs[i].Width > 512)
                {
                    x = 0;
                    y += (float)Math.Ceiling(height + 1);
                    height = 0;
                }

                g.DrawString(i.ToString(), font, brush, new System.Drawing.PointF(x, y));
                glyphs[i] = new RectangleF(
                    new PointF(x + padding / 2f, y),
                    glyphs[i].Size);

                x += (float)Math.Ceiling(glyphs[i].Width) + bitmapPadding;

                if (glyphs[i].Height > height)
                    height = glyphs[i].Height;

            }

            g.Dispose();

            PostProcessFont(bmp);

            //bmp.Save("testfont.png", Drawing.Imaging.ImageFormat.Png);

            string tempFile = System.IO.Path.GetTempFileName() + ".png";
            tempFile = tempFile.Replace("\\", "/");
            bmp.Save(tempFile, Drawing.Imaging.ImageFormat.Png);

            bmp.Dispose();

            Surface surf = new Surface(tempFile);
            return new BitmapFontImpl(surf, glyphs);

        }

        private static void PostProcessFont(System.Drawing.Bitmap bmp)
        {
            Drawing.Imaging.BitmapData data = bmp.LockBits(
                new Drawing.Rectangle(Drawing.Point.Empty, bmp.Size),
                Drawing.Imaging.ImageLockMode.ReadWrite, Drawing.Imaging.PixelFormat.Format32bppArgb);

            PixelFormat bitmapFormat = PixelFormat.BGRA8888;

            PixelBuffer buffer = new PixelBuffer(bitmapFormat, new Size(bmp.Size), data.Scan0,
                bitmapFormat, data.Stride);

            // now convert pixels to gray scale.
            for (int j = 0; j < buffer.Height; j++)
            {
                for (int i = 0; i < buffer.Width; i++)
                {
                    Color clr = buffer.GetPixel(i, j);
                    if (clr.ToArgb() == 0)
                        continue;

                    int alpha = clr.A;
                    int value_i = (int)(0.30 * clr.R + 0.59 * clr.G + 0.11 * clr.B);
                    byte value = (byte)value_i;

                    System.Diagnostics.Debug.Assert(alpha == 0 || alpha == 255);
                    System.Diagnostics.Debug.Assert(0 <= value_i && value_i <= 255);

                    clr = Color.FromArgb(value, Color.White);
                    //clr.R = clr.B = clr.G = value;
                    //clr.A = clr.R = clr.B = clr.G = value;

                    buffer.SetPixel(i, j, clr);
                }
            }


            System.Runtime.InteropServices.Marshal.Copy(
                buffer.Data, 0, data.Scan0, buffer.Data.Length);

            bmp.UnlockBits(data);
        }

        private void CalcAverageCharWidth()
        {
            IEnumerable<RectangleF> rects = mSrcRects.Values;
            float total = 0;
            int count = 0;

            foreach (RectangleF r in rects)
            {
                total += r.Width;
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
                    lineWidth += mSrcRects[line[j]].Width;
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

        private void GetRects(string text, out RectangleF[] srcRects, out RectangleF[] destRects)
        {
            srcRects = new RectangleF[text.Length];
            destRects = new RectangleF[text.Length];

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
                        srcRects[i] = mSrcRects[text[i]];
                        destRects[i] = new RectangleF((float)destX, (float)destY,
                            (float)(srcRects[i].Width * ScaleWidth),
                            (float)(srcRects[i].Height * ScaleHeight));

                        destX += mSrcRects[text[i]].Width * ScaleWidth;
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
            RectangleF[] srcRects;
            RectangleF[] destRects;

            if (string.IsNullOrEmpty(text))
                return;

            GetRects(text, out srcRects, out destRects);

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
}