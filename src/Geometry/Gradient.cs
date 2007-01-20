using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ERY.AgateLib.Geometry
{
    /// <summary>
    /// A four color gradient, with a different color value for each corner.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct Gradient
    {
        private Color mTopLeft;
        private Color mTopRight;
        private Color mBottomLeft;
        private Color mBottomRight;

        /// <summary>
        /// Initializes a gradient with the same color in all four corners.
        /// </summary>
        /// <param name="color"></param>
        public Gradient(Color color)
        {
            mTopLeft = mTopRight = mBottomLeft = mBottomRight = color;
        }
        /// <summary>
        /// Initializes a gradient.
        /// </summary>
        /// <param name="tl">The top left color.</param>
        /// <param name="tr">The top right color.</param>
        /// <param name="bl">The bottom left color.</param>
        /// <param name="br">The bottom right color.</param>
        public Gradient(Color tl, Color tr, Color bl, Color br)
        {
            mTopLeft = tl;
            mTopRight = tr;
            mBottomLeft = bl;
            mBottomRight = br;
        }
        /// <summary>
        /// Gets or sets the color for the top left.
        /// </summary>
        public Color TopLeft
        {
            get { return mTopLeft; }
            set { mTopLeft = value; }
        }
        /// <summary>
        /// Gets or sets the color for the top right.
        /// </summary>
        public Color TopRight
        {
            get { return mTopRight; }
            set { mTopRight = value; }
        }
        /// <summary>
        /// Gets or sets the color for the bottom left.
        /// </summary>
        public Color BottomLeft
        {
            get { return mBottomLeft; }
            set { mBottomLeft = value; }
        }
        /// <summary>
        /// Gets or sets the color for the bottom right.
        /// </summary>
        public Color BottomRight
        {
            get { return mBottomRight; }
            set { mBottomRight = value; }
        }

        /// <summary>
        /// Calculates the color which is the average of all four colors.
        /// </summary>
        public Color AverageColor
        {
            get
            {
                return Color.FromArgb(
                    (TopLeft.A + TopRight.A + BottomLeft.A + BottomRight.A) / 4,
                    (TopLeft.R + TopRight.R + BottomLeft.R + BottomRight.R) / 4,
                    (TopLeft.G + TopRight.G + BottomLeft.G + BottomRight.G) / 4,
                    (TopLeft.B + TopRight.B + BottomLeft.B + BottomRight.B) / 4);
            }
        }

        /// <summary>
        /// Interpolates the color into the interior of the gradient.
        /// Parameters should be in the range 0.0 to 1.0; they are clamped
        /// if outside this range.
        /// </summary>
        /// <param name="x">Distance from the left side of the gradient.  0.0 is 
        /// on the left side, 1.0 is on the right.</param>
        /// <param name="y">Distance from the top of the gradient.  0.0 is 
        /// on the top, 1.0 is on the bottom.</param>
        /// <returns></returns>
        public Color Interpolate(double x, double y)
        {
            Color left;
            Color right;

            if (y <= 0.0)
            {
                left = TopLeft;
                right = TopRight;
            }
            else if (y >= 1.0)
            {
                left = BottomLeft;
                right = BottomRight;
            }
            else
            {
                left = Color.FromArgb(
                    (int)(TopLeft.A * (1 - y) + BottomLeft.A * y),
                    (int)(TopLeft.R * (1 - y) + BottomLeft.R * y),
                    (int)(TopLeft.G * (1 - y) + BottomLeft.G * y),
                    (int)(TopLeft.B * (1 - y) + BottomLeft.B * y));

                right = Color.FromArgb(
                    (int)(TopRight.A * (1 - y) + BottomRight.A * y),
                    (int)(TopRight.R * (1 - y) + BottomRight.R * y),
                    (int)(TopRight.G * (1 - y) + BottomRight.G * y),
                    (int)(TopRight.B * (1 - y) + BottomRight.B * y));
            }

            if (x <= 0.0)
                return left;
            else if (x >= 1.0)
                return right;
            else
            {
                return Color.FromArgb(
                    (int)(left.A * (1 - x) + right.A * x),
                    (int)(left.R * (1 - x) + right.R * x),
                    (int)(left.G * (1 - x) + right.G * x),
                    (int)(left.B * (1 - x) + right.B * x));
            }
        }
    }
}
