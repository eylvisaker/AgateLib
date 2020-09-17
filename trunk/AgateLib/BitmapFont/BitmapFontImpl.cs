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
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;
using AgateLib.Resources;
using AgateLib.DisplayLib.Cache;

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
		/// <param name="name">The name of the font.</param>
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

			int CRcount = 0;
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

					lineWidth += mFontMetrics[line[j]].Width;
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

				CRcount++;

			} while (i != -1);

			if (text[text.Length - 1] == '\n')
				CRcount--;

			return new Size((int)Math.Ceiling(highestLineWidth * state.ScaleWidth),
				(int)(mCharHeight * (CRcount + 1) * state.ScaleHeight));
		}

		/// <summary>
		/// Returns the height of characters in the font.
		/// </summary>
		public override int FontHeight
		{
			get { return mCharHeight; }
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
				state.TransformedText, state.ScaleHeight, state.ScaleWidth);

			PointF dest = state.Location;

			if (state.DisplayAlignment != OriginAlignment.TopLeft)
			{
				Point value = Origin.Calc(state.DisplayAlignment,
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