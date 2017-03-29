//
//    Copyright (c) 2006-2017 Erik Ylvisaker
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
using Drawing = System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.Resources;

namespace AgateLib.Platform.WinForms.Fonts
{
	/// <summary>
	/// Utility class for constructing a bitmap font image.
	/// </summary>
	public static partial class BitmapFontUtil
	{
		
		/// <summary>
		/// Creates a bitmap font by loading an OS font, and drawing it to 
		/// a bitmap to use as a Surface object.  You should only use this method
		/// if writing a driver.
		/// </summary>
		/// <param name="options"></param>
		/// <returns></returns>
		public static BitmapFontImpl ConstructFromOSFont(BitmapFontOptions options)
		{
			System.Drawing.FontStyle drawingStyle = System.Drawing.FontStyle.Regular;

			if ((options.FontStyle & FontStyles.Bold) > 0) drawingStyle |= System.Drawing.FontStyle.Bold;
			if ((options.FontStyle & FontStyles.Italic) > 0) drawingStyle |= System.Drawing.FontStyle.Italic;
			if ((options.FontStyle & FontStyles.Strikeout) > 0) drawingStyle |= System.Drawing.FontStyle.Strikeout;
			if ((options.FontStyle & FontStyles.Underline) > 0) drawingStyle |= System.Drawing.FontStyle.Underline;

			Drawing.Font font = new Drawing.Font(options.FontFamily, options.SizeInPoints, drawingStyle);
			Drawing.Bitmap bmp;
			FontMetrics glyphs;

			ICharacterRenderer rend;

			switch (options.TextRenderer)
			{
				case TextRenderEngine.Gdi:
					rend = new GdiWindows(font);
					break;

				case TextRenderEngine.TextRenderer:
					rend = new TextRend(font);
					break;

				default:
				case TextRenderEngine.Graphics:
					rend = new GraphicsRend(font);
					break;
			}

			MakeBitmap(options, rend, out bmp, out glyphs);

			string tempFile = System.IO.Path.GetTempFileName() + ".png";

			bmp.Save(tempFile, Drawing.Imaging.ImageFormat.Png);
			bmp.Dispose();

			Surface surf = new Surface(tempFile);
			System.IO.File.Delete(tempFile);

			string name = string.Format("{0} {1}{2}", options.FontFamily, options.SizeInPoints, 
				(options.FontStyle != FontStyles.None) ? " " + options.FontStyle.ToString():string.Empty );

			return new BitmapFontImpl(surf, glyphs, name);
		}

		private static void MakeBitmap(BitmapFontOptions options, ICharacterRenderer rend,
			out Drawing.Bitmap bmp, out FontMetrics glyphs)
		{
			Size bitmapSize = new Size(256, 64);

			bmp = new System.Drawing.Bitmap(bitmapSize.Width, bitmapSize.Height);
			// Set DPI for bmp so that font is actually rendered at the right size.
			bmp.SetResolution(96, 96);

			Drawing.Graphics g = Drawing.Graphics.FromImage(bmp);
			Drawing.Font font = rend.Font;

			g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			
			glyphs = new FontMetrics();

			const int bitmapPadding = 2;

			int x = rend.Padding, y = 2;
			int height = 0;
			char lastChar = ' ';
			int digitWidth = 0;

			// first measure the required height of the image.
			foreach (BitmapFontOptions.CharacterRange range in options.CharacterRanges)
			{
				for (char i = range.StartChar; i <= range.EndChar; i++)
				{
					Size sourceSize = rend.MeasureChar(g, i);

					// skip glyphs which are not in the font.
					if (sourceSize.Width == 0)
						continue;

					if (options.CreateBorder)
					{
						sourceSize.Width += 2;
						sourceSize.Height += 2;
					}
					sourceSize.Height += options.BottomMarginAdjust + options.TopMarginAdjust;

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

					if ('0' <= i && i <= '9')
						digitWidth = Math.Max(digitWidth, sourceSize.Width);

					lastChar = i;
				}
			}

			y += glyphs[lastChar].Height;

			if (y > bitmapSize.Height)
			{
				bitmapSize.Height = y;

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
			Drawing.Color borderColor = options.BorderColor.ToDrawing();

			foreach (BitmapFontOptions.CharacterRange range in options.CharacterRanges)
			{
				for (char i = range.StartChar; i <= range.EndChar; i++)
				{
					if (glyphs.ContainsKey(i) == false)
						continue;

					if (x + glyphs[i].Width > bitmapSize.Width)
					{
						x = rend.Padding;
						y += height + bitmapPadding + 1;
						height = 0;
					}

					int drawX = x;
					int drawY = y + options.TopMarginAdjust;

					if (options.CreateBorder)
					{
						rend.DrawChar(borderG, i, new Point(drawX , drawY + 1), borderColor);
						rend.DrawChar(borderG, i, new Point(drawX + 2, drawY + 1), borderColor);
						rend.DrawChar(borderG, i, new Point(drawX + 1, drawY), borderColor);
						rend.DrawChar(borderG, i, new Point(drawX + 1, drawY + 2), borderColor);

						rend.DrawChar(g, i, new Point(drawX + 1, drawY + 1), System.Drawing.Color.White);

						if (font.SizeInPoints >= 14.0)
						{
							glyphs[i].LeftOverhang += 1;
							glyphs[i].RightOverhang += 1;
						}
						else
							glyphs[i].RightOverhang += 1;

					}
					else
					{
						rend.DrawChar(g, i, new Point(drawX, drawY), System.Drawing.Color.White);
					}

					glyphs[i].SourceRect = new Rectangle(
						new Point(x, y),
						glyphs[i].Size);

					x += glyphs[i].Width + bitmapPadding;

					if (height < glyphs[i].Height)
						height = glyphs[i].Height;

				}
			}

			rend.ModifyMetrics(glyphs, g);

			if (options.MonospaceNumbers)
			{
				digitWidth += options.NumberWidthAdjust;

				for (char i = '0'; i <= '9'; i++)
				{
					int delta = digitWidth - glyphs[i].Width;

					int leftShift = -delta / 2;
					int rightShift = -delta - leftShift;

					glyphs[i].LeftOverhang = leftShift;
					glyphs[i].RightOverhang = rightShift;
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

			const PixelFormat bitmapFormat = PixelFormat.BGRA8888;

			PixelBuffer buffer = new PixelBuffer(bitmapFormat, bmp.Size.ToGeometry());
				
			buffer.SetData(data.Scan0, bitmapFormat, data.Stride);

			// now convert pixels to gray scale.
			for (int j = 0; j < buffer.Height; j++)
			{
				for (int i = 0; i < buffer.Width; i++)
				{
					Color clr = buffer.GetPixel(i, j);
					if (clr.ToArgb() == 0)
						continue;

					int alpha = clr.A;
					int intensity = (int)Math.Round(0.30 * clr.R + 0.59 * clr.G + 0.11 * clr.B);
					byte value = (byte)intensity;

					System.Diagnostics.Debug.Assert(0 <= intensity && intensity <= 255);
					if (intensity < 0) intensity = 0;
					if (intensity > 255) intensity = 255;


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
