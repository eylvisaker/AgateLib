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

namespace AgateLib.DisplayLib.BitmapFont
{
	/// <summary>
	/// Class which indicates how a bitmap font should be generated
	/// from an operating system font.
	/// </summary>
	public class BitmapFontOptions
	{
		/// <summary>
		/// Class which represents a range of characters 
		/// </summary>
		public class CharacterRange
		{
			private char mStartChar;
			private char mEndChar;

			/// <summary>
			/// Constructs a character range.
			/// </summary>
			/// <param name="startChar"></param>
			/// <param name="endChar"></param>
			public CharacterRange(char startChar, char endChar)
			{
				if (mEndChar < mStartChar)
					throw new ArgumentException("endChar must not be less than startChar.");

				mStartChar = startChar;
				mEndChar = endChar;
			}

			/// <summary>
			/// Last char in the range.
			/// </summary>
			public char EndChar
			{
				get { return mEndChar; }
				set
				{
					mEndChar = value;

					if (value < mStartChar)
						throw new ArgumentException("EndChar must be less than StartChar.");
				}
			}
			/// <summary>
			/// First char in the range.
			/// </summary>
			public char StartChar
			{
				get { return mStartChar; }
				set
				{
					mStartChar = value;

					if (mStartChar > mEndChar)
						mEndChar = mStartChar;
				}
			}

			/// <summary>
			/// Returns a string representation of the CharacterRange object.
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				return ((int)mStartChar).ToString() + " - " + ((int)mEndChar).ToString();
			}
		}

		private List<CharacterRange> mRanges = new List<CharacterRange>();

		/// <summary>
		/// Constructs a BitmapFontOptions object using a 10-point sans serif font.
		/// </summary>
		public BitmapFontOptions()
			: this("Sans Serif", 14.0f)
		{
		}
		/// <summary>
		/// Constructs a BitmapFontOptions object.
		/// </summary>
		/// <param name="fontFamily"></param>
		/// <param name="sizeInPoints"></param>
		public BitmapFontOptions(string fontFamily, float sizeInPoints)
		{
			FontFamily = fontFamily;
			SizeInPoints = sizeInPoints;

			// Latin characters used by English.
			mRanges.Add(new CharacterRange(' ', '~'));

			// Latin characters used by languages other than English.
			mRanges.Add(new CharacterRange((char)0xA1, (char)0xFF));
		}

		/// <summary>
		/// Constructs a BitmapFontOptions object.
		/// </summary>
		/// <param name="fontFamily"></param>
		/// <param name="sizeInPoints"></param>
		/// <param name="style"></param>
		public BitmapFontOptions(string fontFamily, float sizeInPoints, FontStyles style)
			: this(fontFamily, sizeInPoints)
		{
			FontStyle = style;
		}

		/// <summary>
		/// Adds a range of characters to be rendered.
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		public void AddRange(char start, char end)
		{
			try
			{
				for (int i = 0; i < mRanges.Count; i++)
				{
					if (mRanges[i].StartChar < end)
					{
						mRanges[i].StartChar = start;
						return;
					}
					else if (mRanges[i].EndChar > start)
					{
						mRanges[i].EndChar = end;
						return;
					}
				}

				mRanges.Add(new CharacterRange(start, end));
			}
			finally
			{
				ConsolidateRanges();
			}
		}

		private void ConsolidateRanges()
		{
			SortRanges();

			for (int i = 0; i < mRanges.Count - 1; i++)
			{
				while (mRanges[i].EndChar > mRanges[i + 1].StartChar)
				{
					mRanges[i].EndChar = mRanges[i + 1].EndChar;

					mRanges.RemoveAt(i + 1);
					i--;
				}
			}
		}
		private void SortRanges()
		{
			mRanges.Sort(
				delegate(CharacterRange x, CharacterRange y)
				{
					return x.StartChar.CompareTo(y.StartChar);
				});
		}

		/// <summary>
		/// Enumerates the character ranges to be rendered.
		/// </summary>
		public IEnumerable<CharacterRange> CharacterRanges
		{
			get { return mRanges; }
		}

		/// <summary>
		/// Gets the total number of characters to be rendered to the bitmap.
		/// </summary>
		public int TotalChars
		{
			get
			{
				int result = 0;

				for (int i = 0; i < mRanges.Count; i++)
					result += (int)(mRanges[i].EndChar - mRanges[i].StartChar + 1);

				return result;
			}
		}

		/// <summary>
		/// Indicates how to treat font edges.
		/// </summary>
		public BitmapFontEdgeOptions EdgeOptions { get; set; } = BitmapFontEdgeOptions.IntensityAlphaColor;
		/// <summary>
		/// Indicates whether to use System.Windows.Forms.TextRenderer instead of
		/// System.Drawing.Graphics.  TextRenderer on Windows will likely produce 
		/// nicer-looking characters than Graphics, but it is much slower rendering
		/// the characters.
		/// </summary>
		public TextRenderEngine TextRenderer { get; set; }

		/// <summary>
		/// Style of the font to be generated.
		/// </summary>
		public FontStyles FontStyle { get; set; }
		
		/// <summary>
		/// Size of the font in points.
		/// </summary>
		public float SizeInPoints { get; set; }
		
		/// <summary>
		/// Name of the font family.
		/// </summary>
		public string FontFamily { get; set; }
		
		/// <summary>
		/// Color of the border on the glyphs.  Colors
		/// other than black may show up correctly when drawing
		/// colored text.
		/// </summary>
		public Color BorderColor { get; set; } = Color.FromArgb(128, Color.Black);

		/// <summary>
		/// Indicates whether to create a border around the glyphs.
		/// </summary>
		public bool CreateBorder { get; set; }

		/// <summary>
		/// Indicates how much to increase the top margin of letters.  Can be negative.
		/// </summary>
		public int TopMarginAdjust { get; set; }

		/// <summary>
		/// Indicates how much to increase the bottom margin of letters.  Can be negative.
		/// </summary>
		public int BottomMarginAdjust { get; set; }

		/// <summary>
		/// Set to true to force the digits 0-9 to be generated at the same width.
		/// </summary>
		public bool MonospaceNumbers { get; set; } = true;

		/// <summary>
		/// Gets or sets the number of pixels the width of numbers is to be increased or decreased.
		/// This value is only used if MonospaceNumbers is enabled.
		/// </summary>
		public int NumberWidthAdjust { get; set; }
	}

	/// <summary>
	/// Enum values for indicating which rendering engine should be used to render text for a bitmap font.
	/// </summary>
	public enum TextRenderEngine
	{
		/// <summary>
		/// Use the TextRenderer class.
		/// </summary>
		TextRenderer,

		/// <summary>
		/// Use raw GDI methods.
		/// </summary>
		Gdi,

		/// <summary>
		/// Use System.Drawing.Graphics.
		/// </summary>
		Graphics,
	}
}
