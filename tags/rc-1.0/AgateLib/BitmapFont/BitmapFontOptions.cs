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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2011.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.BitmapFont
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

		private string mFamily;
		private float mSize;
		private FontStyle mStyle;
		private bool mUseTextRenderer = true;
		private bool mCreateBorder;
		private bool mMonospaceNumbers = true;
		private Color mBorderColor = Color.FromArgb(128, Color.Black);
		private BitmapFontEdgeOptions mEdgeOptions;
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
			mFamily = fontFamily;
			mSize = sizeInPoints;
			mEdgeOptions = BitmapFontEdgeOptions.IntensityAlphaColor;

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
		public BitmapFontOptions(string fontFamily, float sizeInPoints, FontStyle style)
			: this(fontFamily, sizeInPoints)
		{
			mStyle = style;
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
				int retval = 0;

				for (int i = 0; i < mRanges.Count; i++)
					retval += (int)(mRanges[i].EndChar - mRanges[i].StartChar + 1);

				return retval;
			}
		}

		/// <summary>
		/// Indicates how to treat font edges.
		/// </summary>
		public BitmapFontEdgeOptions EdgeOptions
		{
			get { return mEdgeOptions; }
			set { mEdgeOptions = value; }
		}
		/// <summary>
		/// Indicates whether to use System.Windows.Forms.TextRenderer instead of
		/// System.Drawing.Graphics.  TextRenderer on Windows will likely produce 
		/// nicer-looking characters than Graphics, but it is much slower rendering
		/// the characters.
		/// </summary>
		public bool UseTextRenderer
		{
			get { return mUseTextRenderer; }
			set { mUseTextRenderer = value; }
		}

		/// <summary>
		/// Style of the font to be generated.
		/// </summary>
		public FontStyle FontStyle
		{
			get { return mStyle; }
			set { mStyle = value; }
		}
		/// <summary>
		/// Size of the font in points.
		/// </summary>
		public float SizeInPoints
		{
			get { return mSize; }
			set { mSize = value; }
		}
		/// <summary>
		/// Name of the font family.
		/// </summary>
		public string FontFamily
		{
			get { return mFamily; }
			set { mFamily = value; }
		}
		/// <summary>
		/// Color of the border on the glyphs.  Colors
		/// other than black may show up correctly when drawing
		/// colored text.
		/// </summary>
		public Color BorderColor
		{
			get { return mBorderColor; }
			set { mBorderColor = value; }
		}
		/// <summary>
		/// Indicates whether to create a border around the glyphs.
		/// </summary>
		public bool CreateBorder
		{
			get { return mCreateBorder; }
			set { mCreateBorder = value; }
		}

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
		public bool MonospaceNumbers
		{
			get { return mMonospaceNumbers; }
			set { mMonospaceNumbers = value; }
		}

		/// <summary>
		/// Gets or sets the number of pixels the width of numbers is to be increased or decreased.
		/// This value is only used if MonospaceNumbers is enabled.
		/// </summary>
		public int NumberWidthAdjust { get; set; }
	}

}
