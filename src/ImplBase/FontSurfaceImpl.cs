//     ``The contents of this file are subject to the Mozilla Public License
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
        /// <param name="destX"></param>
        /// <param name="destY"></param>
        /// <param name="text"></param>
        public abstract void DrawText(int destX, int destY, string text);
        /// <summary>
        /// Draws the specified string at the specified location.
        /// </summary>
        /// <param name="destX"></param>
        /// <param name="destY"></param>
        /// <param name="text"></param>
        public abstract void DrawText(double destX, double destY, string text);
        /// <summary>
        /// Draws the specified string at the specified location.
        /// </summary>
        /// <param name="destPt"></param>
        /// <param name="text"></param>
        public abstract void DrawText(Point destPt, string text);
        /// <summary>
        /// Draws the specified string at the specified location.
        /// </summary>
        /// <param name="destPt"></param>
        /// <param name="text"></param>
        public abstract void DrawText(PointF destPt, string text);

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
