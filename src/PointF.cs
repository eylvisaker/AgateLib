using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib
{
    /// <summary>
    /// Replacement for System.Drawing.PointF structure.
    /// </summary>
    public struct PointF
    {
        float x, y;

        #region --- Construction ---

        /// <summary>
        /// Constructs a PointF object.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public PointF(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        /// <summary>
        /// Constructs a PointF object.
        /// </summary>
        /// <param name="pt"></param>
        public PointF(PointF pt)
        {
            this.x = pt.x;
            this.y = pt.y;
        }
        /// <summary>
        /// Constructs a PointF object.
        /// </summary>
        /// <param name="size"></param>
        public PointF(SizeF size)
        {
            this.x = size.Width;
            this.y = size.Height;
        }
        /// <summary>
        /// Constructs a PointF object.
        /// </summary>
        /// <param name="pt"></param>
        public PointF(System.Drawing.PointF pt)
        {
            this.x = pt.X;
            this.y = pt.Y;
        }

        #endregion
        #region --- Public Properties ---

        /// <summary>
        /// Gets or sets the X value.
        /// </summary>
        public float X
        {
            get { return x; }
            set { x = value; }
        }
        /// <summary>
        /// Gets or sets the Y value.
        /// </summary>
        public float Y
        {
            get { return y; }
            set { y = value; }
        }
        /// <summary>
        /// Returns true if X and Y are zero.
        /// </summary>
        public bool IsEmpty
        {
            get { return x == 0 && y == 0; }
        }

        #endregion

        #region --- Static Methods and Fields ---

        /// <summary>
        /// Empty PointF.
        /// </summary>
        public static readonly PointF Empty = new PointF(0, 0);

        /// <summary>
        /// Adds the specified SizeF structure to the specified PointF structure
        /// and returns the result as a new PointF structure.
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static PointF Add(PointF pt, SizeF size)
        {
            return new PointF(pt.x + size.Width, pt.y + size.Height);
        }

        #endregion

    }
}
