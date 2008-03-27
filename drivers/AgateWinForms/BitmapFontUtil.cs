using System;
using Drawing = System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ERY.AgateLib;
using ERY.AgateLib.Geometry;
using ERY.AgateLib.BitmapFont;

namespace ERY.AgateLib.WinForms
{
    public static class BitmapFontUtil
    {
        interface ICharacterRenderer
        {
            Drawing.Font Font { get; set; }

            int Padding { get; }
            Size MeasureText(Drawing.Graphics g, string text);
            void DrawText(Drawing.Graphics g, string text, Point location, Drawing.Color clr);
        }

        class TextRend : ICharacterRenderer
        {
            Drawing.Font font;

            public TextRend(Drawing.Font font)
            {
                Font = font;
            }
            public System.Drawing.Font Font
            {
                get { return font; }
                set { font = value; }
            }
            public int Padding
            {
                get { return 1; }
            }
            TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix;
            
            public Size MeasureText(System.Drawing.Graphics g, string text)
            {
                Drawing.Size size = TextRenderer.MeasureText(g, text,
                           font, new System.Drawing.Size(256, 256), flags);

                return new Size(size.Width, size.Height);
            }

            public void DrawText(System.Drawing.Graphics g, string text, Point location, Drawing.Color clr)
            {
                TextRenderer.DrawText(g, text, font,
                        new System.Drawing.Rectangle(location.X, location.Y, 256, 256), 
                        clr, flags);
            }

        }
        class GraphicsRend : ICharacterRenderer
        {
            Drawing.Font font;
            float padding;

            public GraphicsRend(Drawing.Font font)
            {
                Font = font;
            }
            public System.Drawing.Font Font
            {
                get { return font; }
                set { font = value; }
            }

            public int Padding
            {
                get { return (int)Math.Ceiling(padding); }
            }
            void CalculatePadding(Drawing.Graphics g)
            {
                // apparently .NET (or GDI+) does this stupid thing on Windows
                // where is reports extra padded space around the characters drawn.
                // Fortunately, this padding is equal the reported size of the
                // space character, which is not drawn when by itself.
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    SizeF padSize = Interop.Convert(g.MeasureString(" ", font));

                    padding = padSize.Width - 1;
                }
            }
            public Size MeasureText(Drawing.Graphics g, string text)
            {
                if (padding == 0)
                    CalculatePadding(g);

                Drawing.SizeF size = g.MeasureString(text, font);
                size.Width -= padding;

                // for space character on windows.
                if (text == " " && padding > 0)
                    size.Width = padding;

                return new Size((int)(size.Width), (int)(size.Height));
            }

            public void DrawText(Drawing.Graphics g, string text, Point location, Drawing.Color clr)
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                // we need to adjust the position by half the padding
                location.X -= (int)Math.Ceiling(padding / 2);

                using (Drawing.Brush brush = new Drawing.SolidBrush(clr))
                {
                    g.DrawString(text, font, brush, new Drawing.Point(location.X, location.Y));
                }
            }
        }

        /// <summary>
        /// Creates a bitmap font by loading an OS font, and drawing it to 
        /// a bitmap to use as a Surface object.  You should only use this method
        /// if writing a driver.
        /// </summary>
        /// <param name="fontFamily"></param>
        /// <param name="sizeInPoints"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static BitmapFontImpl ConstructFromOSFont(BitmapFontOptions options)
        {
            System.Drawing.FontStyle drawingStyle = System.Drawing.FontStyle.Regular;

            if ((options.FontStyle & FontStyle.Bold) > 0) drawingStyle |= System.Drawing.FontStyle.Bold;
            if ((options.FontStyle & FontStyle.Italic) > 0) drawingStyle |= System.Drawing.FontStyle.Italic;
            if ((options.FontStyle & FontStyle.Strikeout) > 0) drawingStyle |= System.Drawing.FontStyle.Strikeout;
            if ((options.FontStyle & FontStyle.Underline) > 0) drawingStyle |= System.Drawing.FontStyle.Underline;

            Drawing.Font font = new Drawing.Font(options.FontFamily, options.SizeInPoints, drawingStyle);
            Drawing.Bitmap bmp;
            FontMetrics glyphs;

            ICharacterRenderer rend = options.UseTextRenderer ? 
                (ICharacterRenderer)new TextRend(font) : 
                (ICharacterRenderer)new GraphicsRend(font);
            
            MakeBitmap(options, rend, out bmp, out glyphs);

            //bmp.Save("testfont.png", Drawing.Imaging.ImageFormat.Png);

            string tempFile = System.IO.Path.GetTempFileName() + ".png";
            tempFile = tempFile.Replace("\\", "/");

            bmp.Save(tempFile, Drawing.Imaging.ImageFormat.Png);
            bmp.Dispose();

            Surface surf = new Surface(tempFile);
            System.IO.File.Delete(tempFile);

            return new BitmapFontImpl(surf, glyphs);

        }

        private static void MakeBitmap(BitmapFontOptions options, ICharacterRenderer rend,
            out Drawing.Bitmap bmp, out FontMetrics glyphs)
        {
            Size bitmapSize = new Size(256, 64);
            
            bmp = new System.Drawing.Bitmap(bitmapSize.Width, bitmapSize.Height);
            Drawing.Graphics g = Drawing.Graphics.FromImage(bmp);
            Drawing.Font font = rend.Font;

            glyphs = new FontMetrics();

            const int bitmapPadding = 2;

            int x = rend.Padding, y = 2;
            int height = 0;
            char lastChar = ' ';

            // first measure the required height of the image.
            foreach (BitmapFontOptions.CharacterRange range in options.CharacterRanges)
            {
                for (char i = range.StartChar; i <= range.EndChar; i++)
                {
                    
                    Size sourceSize = rend.MeasureText(g, i.ToString());
                    if (options.CreateBorder)
                    {
                        sourceSize.Width += 2;
                        sourceSize.Height += 2;
                    }

                    int thisWidth = sourceSize.Width + bitmapPadding;
                     
                    x += thisWidth;

                    if (height < sourceSize.Height)
                        height = sourceSize.Height;

                    if (x > bitmapSize.Width)
                    {
                        x = 1 + thisWidth;
                        y += height + bitmapPadding + 1;
                        height = 0;
                    }

                    glyphs[i] = new GlyphMetrics(new Rectangle(0, 0, sourceSize.Width, sourceSize.Height));

                    lastChar = i;
                }
            }

            y += glyphs[lastChar].Height;

            if (y > bitmapSize.Height)
            {
                while (y > bitmapSize.Height)
                    bitmapSize.Height *= 2;

                g.Dispose();
                bmp.Dispose();

                bmp = new System.Drawing.Bitmap(bitmapSize.Width, bitmapSize.Height);
                g = Drawing.Graphics.FromImage(bmp);
            }

            Drawing.Bitmap borderBmp = new System.Drawing.Bitmap(bmp.Width, bmp.Height);
            Drawing.Graphics borderG = Drawing.Graphics.FromImage(borderBmp);

            x = rend.Padding;
            y = 2;
            height = 0;
            Drawing.Color borderColor = System.Drawing.Color.FromArgb(
                options.BorderColor.A,  options.BorderColor.R, options.BorderColor.G, options.BorderColor.B);
            
            foreach (BitmapFontOptions.CharacterRange range in options.CharacterRanges)
            {
                for (char i = range.StartChar; i <= range.EndChar; i++)
                {
                    if (x + glyphs[i].Width > bitmapSize.Width)
                    {
                        x = rend.Padding;
                        y += height + bitmapPadding + 1;
                        height = 0;
                    }

                    if (options.CreateBorder)
                    {
                        rend.DrawText(borderG, i.ToString(), new Point(x, y + 1), borderColor);
                        rend.DrawText(borderG, i.ToString(), new Point(x + 2, y + 1), borderColor);
                        rend.DrawText(borderG, i.ToString(), new Point(x + 1, y), borderColor);
                        rend.DrawText(borderG, i.ToString(), new Point(x + 1, y + 2), borderColor);

                        rend.DrawText(g, i.ToString(), new Point(x+1, y+1), System.Drawing.Color.White);

                        if (font.SizeInPoints >= 14.0)
                            glyphs[i].LeftOverhang = 1;

                        glyphs[i].RightOverhang = 1;
                    }
                    else
                    {
                        rend.DrawText(g, i.ToString(), new Point(x, y), System.Drawing.Color.White);
                    }

                    glyphs[i].SourceRect = new Rectangle(
                        new Point(x, y),
                        glyphs[i].Size);

                    x += glyphs[i].Width + bitmapPadding;

                    if (height < glyphs[i].Height)
                        height = glyphs[i].Height;

                }
            }

            g.Dispose();

            // do post processing of chars.
            PostProcessFont(options, bmp);

            // place the chars on the border image
            borderG.DrawImage(bmp, new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height));

            bmp.Dispose();
            borderG.Dispose();

            bmp = borderBmp;

        }

        private static void PostProcessFont(BitmapFontOptions options, System.Drawing.Bitmap bmp)
        {
            if (options.EdgeOptions == BitmapFontEdgeOptions.None)
                return;

            Drawing.Imaging.BitmapData data = bmp.LockBits(
                new Drawing.Rectangle(Drawing.Point.Empty, bmp.Size),
                Drawing.Imaging.ImageLockMode.ReadWrite, Drawing.Imaging.PixelFormat.Format32bppArgb);

            PixelFormat bitmapFormat = PixelFormat.BGRA8888;

            PixelBuffer buffer = new PixelBuffer(bitmapFormat, Interop.Convert(bmp.Size), 
                data.Scan0, bitmapFormat, data.Stride);

            // now convert pixels to gray scale.
            for (int j = 0; j < buffer.Height; j++)
            {
                for (int i = 0; i < buffer.Width; i++)
                {
                    Color clr = buffer.GetPixel(i, j);
                    if (clr.ToArgb() == 0)
                        continue;

                    int alpha = clr.A;
                    int intensity = (int)(0.30 * clr.R + 0.59 * clr.G + 0.11 * clr.B);
                    byte value = (byte)intensity;

                    System.Diagnostics.Debug.Assert(alpha == 0 || alpha == 255);
                    System.Diagnostics.Debug.Assert(0 <= intensity && intensity <= 255);

                    switch (options.EdgeOptions)
                    {
                        case BitmapFontEdgeOptions.IntensityAlphaWhite:
                            clr = Color.FromArgb(value, Color.White);
                            break;

                        case BitmapFontEdgeOptions.IntensityAlphaColor:
                            clr = Color.FromArgb(value, clr);
                            break;

                    }

                    buffer.SetPixel(i, j, clr);
                }
            }


            System.Runtime.InteropServices.Marshal.Copy(
                buffer.Data, 0, data.Scan0, buffer.Data.Length);

            bmp.UnlockBits(data);
        }

    }
}
