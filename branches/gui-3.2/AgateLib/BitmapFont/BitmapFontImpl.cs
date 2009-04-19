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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.ImplementationBase;
using AgateLib.Resources;

namespace AgateLib.BitmapFont
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
			FontName = System.IO.Path.GetFileNameWithoutExtension(filename);
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
		/// <param name="fontMetrics">FontMetrics structure which describes how characters
		/// are laid out.</param>
		public BitmapFontImpl(Surface surface, FontMetrics fontMetrics, string name)
		{
			FontName = name;
			mFontMetrics = (FontMetrics)((ICloneable)fontMetrics).Clone();
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
		/// <summary>
		/// Gets the font metric information.
		/// </summary>
		/// <returns></returns>
		public FontMetrics FontMetrics
		{
			get { return mFontMetrics; }
		}
		/// <summary>
		/// Gets the surface containing the glyphs.
		/// </summary>
		/// <returns></returns>
		public Surface Surface
		{
			get { return mSurface; }
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
				string line = lines[i];
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

		/// <summary>
		/// Returns the height of characters in the font.
		/// </summary>
		public override int FontHeight
		{
			get { return mCharHeight; }
		}

		private void GetRects(string text, RectangleF[] srcRects, RectangleF[] destRects, out int rectCount)
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
						destY += height * this.ScaleHeight;
						break;

					default:
						if (mFontMetrics.ContainsKey(text[i]) == false)
						{
							break;
						}
						GlyphMetrics glyph = mFontMetrics[text[i]];

						destX = Math.Max(0, destX - glyph.LeftOverhang * ScaleWidth);

						srcRects[rectCount] = new RectangleF(
							glyph.SourceRect.X, glyph.SourceRect.Y,
							glyph.SourceRect.Width, glyph.SourceRect.Height);

						destRects[rectCount] = new RectangleF((float)destX, (float)destY,
							(float)(srcRects[rectCount].Width * ScaleWidth),
							(float)(srcRects[rectCount].Height * ScaleHeight));

						destX += destRects[rectCount].Width - glyph.RightOverhang * ScaleWidth;

						rectCount++;
						break;
				}
			}
		}

		RectangleF[] cacheSrcRects;
		RectangleF[] cacheDestRects;

		/// <summary>
		/// Draws the text to the screen.
		/// </summary>
		/// <param name="destX"></param>
		/// <param name="destY"></param>
		/// <param name="text"></param>
		public override void DrawText(int destX, int destY, string text)
		{
			if (string.IsNullOrEmpty(text))
				return;

			if (cacheSrcRects == null || text.Length > cacheSrcRects.Length)
			{
				cacheSrcRects = new RectangleF[text.Length];
				cacheDestRects = new RectangleF[text.Length];
			}

			RectangleF[] srcRects = cacheSrcRects;
			RectangleF[] destRects = cacheDestRects;

			DrawTextImpl(destX, destY, text, srcRects, destRects);
		}

		private void DrawTextImpl(int destX, int destY, string text,
			RectangleF[] srcRects, RectangleF[] destRects)
		{
			// this variable counts the number of rectangles actually used to display text.
			// It may be less then text.Length because carriage return characters 
			// don't need any rects.
			int displayTextLength;
			GetRects(text, srcRects, destRects, out displayTextLength);

			if (DisplayAlignment != OriginAlignment.TopLeft)
			{
				Point value = Origin.Calc(DisplayAlignment, StringDisplaySize(text));

				destX -= value.X;
				destY -= value.Y;
			}

			for (int i = 0; i < displayTextLength; i++)
			{
				destRects[i].X += destX;
				destRects[i].Y += destY;
			}

			mSurface.DrawRects(srcRects, destRects, 0, displayTextLength);
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