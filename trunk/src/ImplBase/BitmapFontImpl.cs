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
        /// <param name="srcRects">An object implementing the IDictionary&lt;char, Rectangle&gr;
        /// interface, containing the source rectangles on the surface for each font glyph.</param>
        public BitmapFontImpl(Surface surface, IDictionary<char, RectangleF> srcRects)
        {
            mSurface = surface;

            foreach (KeyValuePair<char, RectangleF> kvp in srcRects)
            {
                mSrcRects.Add(kvp.Key, kvp.Value);
            }
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
        /// Creates a bitmap font by loading an OS font, and drawing it to 
        /// a bitmap to use as a Surface object.
        /// </summary>
        /// <param name="fontFamily"></param>
        /// <param name="sizeInPoints"></param>
        /// <returns></returns>
        public static FontSurfaceImpl FromOSFont(string fontFamily, float sizeInPoints)
        {
            Drawing.Font font = new Drawing.Font(fontFamily, sizeInPoints, System.Drawing.FontStyle.Bold);
            
            Drawing.Bitmap bmp = new System.Drawing.Bitmap(512, 512);
            Drawing.Graphics g = Drawing.Graphics.FromImage(bmp);

            // first calculate the size of image we need
            Dictionary<char, RectangleF> glyphs = new Dictionary<char, RectangleF>();

            char startChar = ' ';
            char endChar = (char)256;

            float x = 0, y = 0;
            float height = 0;

            for (char i = startChar; i < endChar; i++)
            {
                SizeF size = new SizeF(g.MeasureString(i.ToString(), font));

                glyphs[i] = new RectangleF(0, 0, size.Width, size.Height);

                x += (float)Math.Ceiling(glyphs[i].Width);

                if (glyphs[i].Height > height)
                    height = glyphs[i].Height;

                if (x > 512)
                {
                    x = glyphs[i].Width;
                    y += (float) Math.Ceiling(height);
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

            for (char i = startChar; i < endChar ; i++)
            {
                if (x + glyphs[i].Width > 512)
                {
                    x = 0;
                    y += (float)Math.Ceiling(height);
                    height = 0;
                }

                g.DrawString(i.ToString(), font, brush, new System.Drawing.PointF(x, y));
                glyphs[i]= new RectangleF(new PointF(x, y), glyphs[i].Size);

                x += (float)Math.Ceiling(glyphs[i].Width);

                if (glyphs[i].Height > height)
                    height = glyphs[i].Height;
                
            }

            g.Dispose();

            string tempFile = System.IO.Path.GetTempFileName() + ".png";
            bmp.Save("yourmom.png", Drawing.Imaging.ImageFormat.Png);
            bmp.Save(tempFile, Drawing.Imaging.ImageFormat.Png);

            bmp.Dispose();

            Surface surf = new Surface(tempFile);
            return new BitmapFontImpl(surf, glyphs);

            //throw new NotImplementedException();
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

            return (int)Math.Ceiling(highestLineWidth);
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

            return mCharHeight * (CRcount+1);
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

        private void GetRects(string text, out Rectangle[] srcRects, out Rectangle[] destRects)
        {
            srcRects = new Rectangle[text.Length];
            destRects = new Rectangle[text.Length];

            int destX = 0;
            int destY = 0;
            int height = mCharHeight;

            for (int i = 0; i < text.Length; i++)
            {
                switch (text[i])
                {
                    case '\n':
                        destX = 0;
                        destY += height;
                        break;

                    default:
                        srcRects[i] = Rectangle.Ceiling(mSrcRects[text[i]]);
                        destRects[i] =
                            new Rectangle(destX, destY,
                            (int)(srcRects[i].Width * ScaleWidth + 0.5),
                            (int)(srcRects[i].Height * ScaleHeight + 0.5));

                        destX += destRects[i].Width;
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
            Rectangle[] srcRects;
            Rectangle[] destRects;

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