using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib
{
    /// <summary>
    /// Class which represents a Font to draw on the screen.
    /// </summary>
    public class FontSurface : IDisposable
    {
        internal FontSurfaceImpl impl;

        /// <summary>
        /// Creates a FontSurface object from the given fontFamily.
        /// </summary>
        /// <param name="fontFamily"></param>
        /// <param name="sizeInPoints"></param>
        public FontSurface(string fontFamily, float sizeInPoints)
        {
            impl = Display.Impl.CreateFont(fontFamily, sizeInPoints);

            Display.DisposeDisplay += new Display.DisposeDisplayHandler(Dispose);
        }
        /// <summary>
        /// Destroys this object.
        /// </summary>
        ~FontSurface()
        {
            Dispose(false);
        }
        /// <summary>
        /// Disposes of this object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        private void Dispose(bool disposing)
        {
            if (impl != null)
                impl.Dispose();

            impl = null;

            if (disposing)
                GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Sets the interpretation of the draw point used.
        /// </summary>
        public OriginAlignment DisplayAlignment
        {
            get { return impl.DisplayAlignment; }
            set { impl.DisplayAlignment = value; }
        }
        /// <summary>
        /// Sets the color of the font.
        /// </summary>
        public Color Color
        {
            get { return impl.Color; }
            set { impl.Color = value; }
        }
        /// <summary>
        /// Sets the transparency of the font.
        /// 0.0 is fully transparent
        /// 1.0 is completely opaque.
        /// </summary>
        public double Alpha
        {
            get { return impl.Alpha; }
            set { impl.Alpha = value; }
        }

        /// <summary>
        /// Sets the scale of the font.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetScale(double x, double y)
        {
            impl.SetScale(x, y);
        }
        /// <summary>
        /// Gets the scale of the font.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void GetScale(out double x, out double y)
        {
            impl.GetScale(out x, out y);
        }
        /// <summary>
        /// Gets or sets the amount the width is scaled when the text is drawn.
        /// 1.0 is no scaling.
        /// </summary>
        public double ScaleWidth
        {
            get { return impl.ScaleWidth; }
            set { impl.ScaleWidth = value; }
        }
        /// <summary>
        /// Gets or sets the amount the height is scaled when the text is drawn.
        /// 1.0 is no scaling.
        /// </summary>
        public double ScaleHeight
        {
            get { return impl.ScaleHeight; }
            set { impl.ScaleHeight = value; }
        }

        /// <summary>
        /// Measures the display width of the specified string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public int StringDisplayWidth(string text) { return impl.StringDisplayWidth(text); }
        /// <summary>
        /// Measures the display height of the specified string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public int StringDisplayHeight(string text) { return impl.StringDisplayHeight(text); }
        /// <summary>
        /// Measures the display size of the specified string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public Size StringDisplaySize(string text) { return impl.StringDisplaySize(text); }

        /// <summary>
        /// Draws the specified string at the specified location.
        /// </summary>
        /// <param name="destX"></param>
        /// <param name="destY"></param>
        /// <param name="text"></param>
        public void DrawText(int destX, int destY, string text) { impl.DrawText(destX, destY, text); }
        /// <summary>
        /// Draws the specified string at the specified location.
        /// </summary>
        /// <param name="destX"></param>
        /// <param name="destY"></param>
        /// <param name="text"></param>
        public void DrawText(double destX, double destY, string text) { impl.DrawText(destX, destY, text); }
        /// <summary>
        /// Draws the specified string at the specified location.
        /// </summary>
        /// <param name="destPt"></param>
        /// <param name="text"></param>
        public void DrawText(Point destPt, string text) { impl.DrawText(destPt.X, destPt.Y, text); }
        /// <summary>
        /// Draws the specified string at the specified location.
        /// </summary>
        /// <param name="destPt"></param>
        /// <param name="text"></param>
        public void DrawText(PointF destPt, string text) { impl.DrawText(destPt.X, destPt.Y, text); }
        /// <summary>
        /// Draws the specified string at the origin.
        /// </summary>
        /// <param name="text"></param>
        public void DrawText(string text) { impl.DrawText(0, 0, text); }



    }
   
}
