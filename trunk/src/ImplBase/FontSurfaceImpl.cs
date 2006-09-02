using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.ImplBase
{
    /// <summary>
    /// Implements a FontSurface
    /// </summary>
    public abstract class FontSurfaceImpl : IDisposable
    {
        private OriginAlignment mAlignment = OriginAlignment.TopLeft;
        private Color mColor = Color.White;
        private double mScaleWidth = 1.0;
        private double mScaleHeight = 1.0;

        /// <summary>
        /// Measures the width of the given string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public abstract int StringDisplayWidth(string text);
        /// <summary>
        /// Measures the height of the given string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public abstract int StringDisplayHeight(string text);
        /// <summary>
        /// Measures the size of the given string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public abstract Size StringDisplaySize(string text);

        /// <summary>
        /// Draws the specified string at the specified location.
        /// </summary>
        /// <param name="dest_x"></param>
        /// <param name="dest_y"></param>
        /// <param name="text"></param>
        public abstract void DrawText(int dest_x, int dest_y, string text);
        /// <summary>
        /// Draws the specified string at the specified location.
        /// </summary>
        /// <param name="dest_x"></param>
        /// <param name="dest_y"></param>
        /// <param name="text"></param>
        public abstract void DrawText(double dest_x, double dest_y, string text);
        /// <summary>
        /// Draws the specified string at the specified location.
        /// </summary>
        /// <param name="dest_pt"></param>
        /// <param name="text"></param>
        public abstract void DrawText(Point dest_pt, string text);
        /// <summary>
        /// Draws the specified string at the specified location.
        /// </summary>
        /// <param name="dest_pt"></param>
        /// <param name="text"></param>
        public abstract void DrawText(PointF dest_pt, string text);

        /// <summary>
        /// Sets how to interpret the point given to DrawText methods.
        /// </summary>
        public virtual OriginAlignment DisplayAlignment
        {
            get { return mAlignment; }
            set { mAlignment = value; }
        }
        /// <summary>
        /// Sets the color of the text to be drawn.
        /// </summary>
        public virtual Color Color
        {
            get { return mColor; }
            set { mColor = value; }
        }
        /// <summary>
        /// Sets the alpha value of the text to be drawn.
        /// </summary>
        public virtual double Alpha
        {
            get { return mColor.A / 255.0; }
            set
            {
                if (value < 0) value = 0;
                if (value > 1.0) value = 1.0;

                mColor = Color.FromArgb((int)(value * 255), mColor);
            }
        }
        /// <summary>
        /// Gets or sets the amount the width is scaled when the text is drawn.
        /// 1.0 is no scaling.
        /// </summary>
        public double ScaleWidth
        {
            get { return mScaleWidth; }
            set { mScaleWidth = value; }
        }
        /// <summary>
        /// Gets or sets the amount the height is scaled when the text is drawn.
        /// 1.0 is no scaling.
        /// </summary>
        public double ScaleHeight
        {
            get { return mScaleHeight; }
            set { mScaleHeight = value; }
        }
        /// <summary>
        /// Sets ScaleWidth and ScaleHeight.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public virtual void SetScale(double x, double y)
        {
            ScaleWidth = x;
            ScaleHeight = y;
        }
        /// <summary>
        /// Gets ScaleWidth and ScaleHeight.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public virtual void GetScale(out double x, out double y)
        {
            x = ScaleWidth;
            y = ScaleHeight;
        }

        /// <summary>
        /// Disposes of unmanaged resources.
        /// </summary>
        public abstract void Dispose();
    }

}
