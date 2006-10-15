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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;

using ERY.AgateLib.Geometry;
using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib
{

    /// <summary>
    /// Class which represents a Font to draw on the screen.
    /// </summary>
    public class FontSurface : IDisposable
    {
        internal FontSurfaceImpl impl;
        private StringTransformer mTransformer = StringTransformer.None;

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
        /// Private initializer to tell it what impl to use.
        /// </summary>
        /// <param name="implToUse"></param>
        private FontSurface(FontSurfaceImpl implToUse)
        {
            impl = implToUse;
        }

        /// <summary>
        /// This function loads a monospace bitmap font from the specified image file.
        /// Only the character size is given.  It is assumed that all ASCII characters 
        /// are present, in order from left to right, and top to bottom.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="characterSize"></param>
        /// <returns></returns>
        public static FontSurface BitmapMonospace(string filename, Size characterSize)
        {
            FontSurfaceImpl impl = new BitmapFontImpl(filename, characterSize);

            return new FontSurface(impl);
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
        /// Gets or sets how strings are transformed when they are drawn to the screen.
        /// This is useful for bitmap fonts which contain only all uppercase letters, for
        /// example.
        /// </summary>
        public StringTransformer StringTransformer
        {
            get { return mTransformer; }
            set
            {
                mTransformer = value;

                if (value == null)
                    mTransformer = StringTransformer.None;
            }
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
        public void DrawText(int destX, int destY, string text) 
        {
            impl.DrawText(destX, destY, mTransformer.Transform(text)); 
        }
        /// <summary>
        /// Draws the specified string at the specified location.
        /// </summary>
        /// <param name="destX"></param>
        /// <param name="destY"></param>
        /// <param name="text"></param>
        public void DrawText(double destX, double destY, string text) 
        {
            impl.DrawText(destX, destY, mTransformer.Transform(text)); 
        }
        /// <summary>
        /// Draws the specified string at the specified location.
        /// </summary>
        /// <param name="destPt"></param>
        /// <param name="text"></param>
        public void DrawText(Point destPt, string text) 
        {
            impl.DrawText(destPt.X, destPt.Y, mTransformer.Transform(text)); 
        }
        /// <summary>
        /// Draws the specified string at the specified location.
        /// </summary>
        /// <param name="destPt"></param>
        /// <param name="text"></param>
        public void DrawText(PointF destPt, string text) 
        { 
            impl.DrawText(destPt.X, destPt.Y, mTransformer.Transform(text)); 
        }
        /// <summary>
        /// Draws the specified string at the origin.
        /// </summary>
        /// <param name="text"></param>
        public void DrawText(string text) 
        {
            impl.DrawText(0, 0, mTransformer.Transform(text));
        }

    }
   
}
