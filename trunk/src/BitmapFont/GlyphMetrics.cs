using System;
using System.Collections.Generic;
using System.Text;

using ERY.AgateLib.Geometry;

namespace ERY.AgateLib.BitmapFont
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

        private Dictionary<char, int> mKerning;

        /// <summary>
        /// Source rectangle on the image which supplies the glyph.
        /// </summary>
        public Rectangle SourceRect
        {
            get { return mSourceRect; }
            set { mSourceRect = value; }
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
        /// A dictionary of characters which need kerning when paired with this glyph.
        /// </summary>
        public Dictionary<char, int> KerningPairs
        {
            get { return mKerning; }
        }
    }
}
