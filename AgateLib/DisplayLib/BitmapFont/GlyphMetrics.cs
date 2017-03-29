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
using System.Runtime.Serialization;
using AgateLib.Mathematics.Geometry;
using YamlDotNet.Serialization;
using AgateLib.Quality;

namespace AgateLib.DisplayLib.BitmapFont
{
	/// <summary>
	/// GlyphMetrics defines the metrics for a particular glyph in a font, including
	/// the rectangle of the glyph on the source image, overhang, and kerning pairs.
	/// </summary>
	public sealed class GlyphMetrics
	{
		private Rectangle mSourceRect;

		private int mLeftOverhang;
		private int mRightOverhang;

		private Dictionary<char, int> mKerning = new Dictionary<char, int>();

		/// <summary>
		/// Constructs a GlyphMetrics object.
		/// </summary>
		public GlyphMetrics()
		{
		}

		/// <summary>
		/// Constructs a GlyphMetrics object with the specified source rectangle.
		/// </summary>
		/// <param name="srcRect"></param>
		public GlyphMetrics(Rectangle srcRect)
		{
			mSourceRect = srcRect;
		}

		/// <summary>
		/// Source rectangle on the image which supplies the glyph.
		/// </summary>
		public Rectangle SourceRect
		{
			get { return mSourceRect; }
			set { mSourceRect = value; }
		}

		/// <summary>
		/// Gets or sets the x position of the source rectangle.
		/// </summary>
		[YamlIgnore]
		public int X
		{
			get { return mSourceRect.X; }
			set { mSourceRect.X = value; }
		}

		/// <summary>
		/// Gets or sets the y position of the source rectangle.
		/// </summary>
		[YamlIgnore]
		public int Y
		{
			get { return mSourceRect.Y; }
			set { mSourceRect.Y = value; }
		}
		
		/// <summary>
		/// Gets or sets the width of the glyph.
		/// </summary>
		[YamlIgnore]
		public int Width
		{
			get { return mSourceRect.Width; }
			set { mSourceRect.Width = value; }
		}
		/// <summary>
		/// Gets or sets the height of the glyph.
		/// </summary>
		[YamlIgnore]
		public int Height
		{
			get { return mSourceRect.Height; }
			set { mSourceRect.Height = value; }
		}

		/// <summary>
		/// Gets or sets the size of the glyph.
		/// </summary>
		[YamlIgnore]
		public Size Size
		{
			get { return mSourceRect.Size; }
			set { mSourceRect.Size = value; }
		}

		/// <summary>
		/// Number of pixels this character is shifted to the left.
		/// </summary>
		public int LeftOverhang
		{
			get { return mLeftOverhang; }
			set { mLeftOverhang = value; }
		}
		/// <summary>
		/// Number of pixels the next character should be shifted to the left.
		/// </summary>
		public int RightOverhang
		{
			get { return mRightOverhang; }
			set { mRightOverhang = value; }
		}

		/// <summary>
		/// Gets the number of pixels the drawing position is advanced when this glyph is drawn.
		/// </summary>
		[YamlIgnore]
		public int LayoutWidth
		{
			get { return mSourceRect.Width - LeftOverhang - RightOverhang; }
		}

		/// <summary>
		/// A dictionary of characters which need kerning when paired with this glyph.
		/// </summary>
		public Dictionary<char, int> KerningPairs
		{
			get { return mKerning; }
			set
			{
				Require.ArgumentNotNull(value, nameof(KerningPairs));
				mKerning = value;
			}
		}

		/// <summary>
		/// Performs a deep copy.
		/// </summary>
		/// <returns></returns>
		public GlyphMetrics Clone()
		{
			GlyphMetrics result = new GlyphMetrics();

			result.SourceRect = SourceRect;
			result.LeftOverhang = LeftOverhang;
			result.RightOverhang = RightOverhang;

			foreach (KeyValuePair<char, int> kvp in KerningPairs)
			{
				result.KerningPairs.Add(kvp.Key, kvp.Value);
			}

			return result;
		}

	}
}
