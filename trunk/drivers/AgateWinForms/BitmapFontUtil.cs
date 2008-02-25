using System;
using Drawing = System.Drawing;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib;
using ERY.AgateLib.Geometry;
using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib.WinForms
{
    public static class BitmapFontUtil
    {

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

    }
}
