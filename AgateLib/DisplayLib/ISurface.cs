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
using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib
{
    /// <summary>
    /// Public Surface interface.
    /// </summary>
    public interface ISurface : IDisposable, IDrawable
    {
        /// <summary>
        /// Gets or sets the alpha value for the surface.
        /// 0.0 is completely transparent and 1.0 is completely opaque.
        /// </summary>
        double Alpha { get; set; }
        /// <summary>
        /// Gets or sets the color the surface is multiplied by when drawn.
        /// Setting this value overwrites the Alpha value.
        /// </summary>
        Color Color { get; set; }
        /// <summary>
        /// Gets or sets a color gradient used for this surface.
        /// </summary>
        Gradient ColorGradient { get; set; }
        /// <summary>
        /// Gets or sets how coordinate arguments to Draw overloads are interpreted.
        /// </summary>
        OriginAlignment DisplayAlignment { get; set; }
        /// <summary>
        /// Gets or sets the display height of the surface, in pixels.
        /// </summary>
        int DisplayHeight { get; set; }
        /// <summary>
        /// Gets or sets the display width of the surface, in pixels.
        /// </summary>
        int DisplayWidth { get; set; }
        /// <summary>
        /// Draws the surface at the specified point.
        /// </summary>
        /// <param name="destPt"></param>
        void Draw(PointF destPt);
        /// <summary>
        /// Draws the surface in the specified rectangle.
        /// </summary>
        /// <param name="destRect"></param>
        void Draw(Rectangle destRect);
        /// <summary>
        /// Draws the surface at the specified point.
        /// </summary>
        /// <param name="destX"></param>
        /// <param name="destY"></param>
        void Draw(int destX, int destY);
        /// <summary>
        /// Draws the surface at the specified point.
        /// </summary>
        /// <param name="destX"></param>
        /// <param name="destY"></param>
        void Draw(float destX, float destY);
        /// <summary>
        /// Draws the surface at the specified point.
        /// </summary>
        /// <param name="srcRect"></param>
        /// <param name="destRect"></param>
        void Draw(Rectangle srcRect, Rectangle destRect);
        /// <summary>
        /// Draws the source area of the surface at the specified point.
        /// </summary>
        /// <param name="srcRect"></param>
        /// <param name="point"></param>
        void Draw(Rectangle srcRect, Point point);
        /// <summary>
        /// Gets the current scale values.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void GetScale(out double width, out double height);
        /// <summary>
        /// Increases the rotation angle.
        /// </summary>
        /// <param name="radians"></param>
        void IncrementRotationAngle(double radians);
        /// <summary>
        /// Increases the rotation angle.
        /// </summary>
        /// <param name="degrees"></param>
        void IncrementRotationAngleDegrees(double degrees);

		/// <summary>
		/// The amount the surface is rotated when drawn.  The angle is measured
		/// up from the horizontal axis.
		/// </summary>
		double RotationAngle { get; set; }
        /// <summary>
        /// The amount the surface is rotated when drawn.  The angle is measured
        /// up from the horizontal axis.
        /// </summary>
        double RotationAngleDegrees { get; set; }
        /// <summary>
        /// The point where the surface is rotated around when drawn.
        /// </summary>
        OriginAlignment RotationCenter { get; set; }
        /// <summary>
        /// The amount the height of the surface is scaled when drawn.  1.0 is no scaling.
        /// </summary>
        double ScaleHeight { get; set; }
        /// <summary>
        /// The amount the width of the surface is scaled when drawn.  1.0 is no scaling.
        /// </summary>
        double ScaleWidth { get; set; }
        /// <summary>
        /// Sets the width and height scale values simultaneously.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void SetScale(double width, double height);
        /// <summary>
        /// Gets the height of the surface data in pixels.
        /// </summary>
        int SurfaceHeight { get; }
        /// <summary>
        /// Gets the size of the surface data in pixels.
        /// </summary>
        Size SurfaceSize { get; }
        /// <summary>
        /// Gets the width of the surface data in pixels.
        /// </summary>
        int SurfaceWidth { get; }

        /// <summary>
        /// Gets the SurfaceState object which is used to handle drawing.
        /// </summary>
        SurfaceState State { get; }

        /// <summary>
        /// Gets or sets the interpolation that is used when scaling the surface.
        /// </summary>
        /// <returns></returns>
        InterpolationMode InterpolationHint { get; set; }

        /// <summary>
        /// Draws the surface using the specified state object.
        /// </summary>
        /// <param name="state"></param>
        void Draw(SurfaceState state);

		/// <summary>
		/// Draws the surface given the specified source rect and rotation center.
		/// </summary>
		/// <param name="mSrcRect"></param>
		/// <param name="dest"></param>
		/// <param name="rotationCenter"></param>
		void Draw(Rectangle mSrcRect, PointF dest, PointF rotationCenter);

		/// <summary>
		/// Draws the surface using an array of source and destination rectangles.
		/// This method will throw an exception if the two arrays are not the same size.
		/// </summary>
		/// <param name="srcRects"></param>
		/// <param name="destRects"></param>
		/// <param name="start">Element in the arrays to start at.</param>
		/// <param name="length">Number of elements in the arrays to use.</param>
		void DrawRects(RectangleF[] srcRects, RectangleF[] destRects, int start, int length);
	}
}
