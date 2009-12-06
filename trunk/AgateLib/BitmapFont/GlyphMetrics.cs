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

using AgateLib.Geometry;

namespace AgateLib.BitmapFont
{
	/// <summary>
	/// GlyphMetrics defines the metrics for a particular glyph in a font, including
	/// the rectangle of the glyph on the source image, overhang, and kerning pairs.
	/// </summary>
	public sealed class GlyphMetrics : ICloneable
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
		/// Gets or sets the width of the glyph.
		/// </summary>
		public int Width
		{
			get { return mSourceRect.Width; }
			set { mSourceRect.Width = value; }
		}
		/// <summary>
		/// Gets or sets the height of the glyph.
		/// </summary>
		public int Height
		{
			get { return mSourceRect.Height; }
			set { mSourceRect.Height = value; }
		}

		/// <summary>
		/// Gets or sets the size of the glyph.
		/// </summary>
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
		}

		/// <summary>
		/// Performs a deep copy.
		/// </summary>
		/// <returns></returns>
		public GlyphMetrics Clone()
		{
			GlyphMetrics retval = new GlyphMetrics();

			retval.SourceRect = SourceRect;
			retval.LeftOverhang = LeftOverhang;
			retval.RightOverhang = RightOverhang;

			foreach (KeyValuePair<char, int> kvp in KerningPairs)
			{
				retval.KerningPairs.Add(kvp.Key, kvp.Value);
			}

			return retval;
		}

		#region ICloneable Members

		object ICloneable.Clone()
		{
			return Clone();
		}

		#endregion
	}
}
