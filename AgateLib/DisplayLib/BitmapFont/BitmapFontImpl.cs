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
using System.Collections.Generic;
using System.Text;
using System.Xml;

using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Resources;
using AgateLib.DisplayLib.Cache;
using AgateLib.IO;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib.BitmapFont
{
	/// <summary>
	/// Provides a basic implementation for the use of non-system fonts provided
	/// as a bitmap.
	/// 
	/// To construct a bitmap font, call the appropriate static FontSurface method.
	/// </summary>
	public class BitmapFontImpl : FontSurfaceImpl
	{
		ISurface mSurface;
		FontMetrics mFontMetrics;

		int mCharHeight;
		double mAverageCharWidth;

		#region --- Cache Class ---

		class BitmapFontCache : FontStateCache
		{
			public bool NeedsRefresh = true;
			public RectangleF[] SrcRects;
			public RectangleF[] DestRects;
			public int DisplayTextLength;

			protected internal override FontStateCache Clone()
			{
				BitmapFontCache cache = new BitmapFontCache();

				cache.SrcRects = (RectangleF[])SrcRects.Clone();
				cache.DestRects = (RectangleF[])DestRects.Clone();

				return cache;
			}

			protected internal override void OnTextChanged(FontState fontState)
			{
				NeedsRefresh = true;
			}
			protected internal override void OnDisplayAlignmentChanged(FontState fontState)
			{
				NeedsRefresh = true;
			}
			protected internal override void OnLocationChanged(FontState fontState)
			{
				NeedsRefresh = true;
			}
		}

		#endregion

		/// <summary>
		/// Constructs a BitmapFontImpl, assuming the characters in the given file
		/// are all the same size, and are in their ASCII order.
		/// </summary>
		/// <param name="filename">Path to the file which contains the image data for the font glyphs.</param>
		/// <param name="characterSize">Size of each character in the image.</param>
		public BitmapFontImpl(string filename, Size characterSize)
		{
			FontName = filename;
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
		/// <param name="name">The name of the font.</param>
		public BitmapFontImpl(ISurface surface, FontMetrics fontMetrics, string name)
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
		public FontMetrics FontMetrics => mFontMetrics;

		/// <summary>
		/// Gets the surface containing the glyphs.
		/// </summary>
		/// <returns></returns>
		public ISurface Surface => mSurface;

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
		/// Measures the string based on how it would be drawn with the
		/// specified FontState object.
		/// </summary>
		/// <param name="state"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		public override Size MeasureString(FontState state, string text)
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
		public override int FontHeight(FontState state)
		{
			return (int)(mCharHeight * state.ScaleHeight);
		}

		private void GetRects(RectangleF[] srcRects, RectangleF[] destRects, out int rectCount,
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

						srcRects[rectCount] = new RectangleF(
							glyph.SourceRect.X, glyph.SourceRect.Y,
							glyph.SourceRect.Width, glyph.SourceRect.Height);

						destRects[rectCount] = new RectangleF((float)destX, (float)destY,
							(float)(srcRects[rectCount].Width * ScaleWidth),
							(float)(srcRects[rectCount].Height * ScaleHeight));

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
				cache.SrcRects = new RectangleF[state.Text.Length];
				cache.DestRects = new RectangleF[state.Text.Length];
			}

			return cache;
		}

		/// <summary>
		/// Draws the text to the screen.
		/// </summary>
		/// <param name="state"></param>
		public override void DrawText(FontState state)
		{
			BitmapFontCache cache = GetCache(state);

			RefreshCache(state, cache);

			mSurface.InterpolationHint = state.InterpolationHint;
			mSurface.Color = state.Color;
			mSurface.DrawRects(cache.SrcRects, cache.DestRects, 0, cache.DisplayTextLength);
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

			Vector2f dest = state.Location;

			if (state.TextAlignment != OriginAlignment.TopLeft)
			{
				Point value = Origin.Calc(state.TextAlignment,
					MeasureString(state, state.Text));

				dest.X -= value.X;
				dest.Y -= value.Y;
			}

			for (int i = 0; i < cache.DisplayTextLength; i++)
			{
				cache.DestRects[i].X += dest.X;
				cache.DestRects[i].Y += dest.Y;
			}

			cache.NeedsRefresh = false;
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