using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib
{
    /// <summary>
    /// Replacement for System.Drawing.Size object.
    /// </summary>
    public struct Size
    {
        int width, height;

        /// <summary>
        /// Constructs a Size.
        /// </summary>
        /// <param name="pt"></param>
        public Size(Point pt)
        {
            width = pt.X;
            height = pt.Y;
        }
        /// <summary>
        /// Constructs a Size.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Size(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
        /// <summary>
        /// Constructs a Size.
        /// </summary>
        /// <param name="size"></param>
        public Size(System.Drawing.Size size)
        {
            width = size.Width;
            height = size.Height;
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// Returns true if width and height are zero.
        /// </summary>
        public bool IsEmpty
        {
            get { return width == 0 && height == 0; }
        }

        /// <summary>
        /// Converts to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}Width={1},Height={2}{3}", "{", width, height, "}");
        }
        /// <summary>
        /// Empty Size.
        /// </summary>
        public static readonly Size Empty = new Size(0, 0);

        /// <summary>
        /// Explicit conversion for System.Drawing interop.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static explicit operator System.Drawing.Size(Size size)
        {
            return new System.Drawing.Size(size.width, size.height);
        }
    }
}