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

using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.ImplementationBase
{
    /// <summary>
    /// Base class for implementing a Surface structure.
    /// </summary>
    public abstract class SurfaceImpl : IRenderTargetImpl, IDisposable
    {
        #region --- Private Fields ---

        private bool mIsDisposed = false;
        private bool mShouldBePacked = true;

        private int mTesselate = 1;

        private SurfaceState mState = new SurfaceState();

        #endregion

        #region --- Creation / Destruction ---

        /// <summary>
        /// Constructs a SurfaceImpl object.
        /// </summary>
        public SurfaceImpl()
        {
        }
        /// <summary>
        /// Frees unmanaged resources.
        /// </summary>
        public abstract void Dispose();

        #endregion

        #region --- Drawing the surface to the screen ---

        /// <summary>
        /// For function use, see documentation of Surface.
        /// 
        /// Info for developers:
        /// This method should Draw the surface to the screen, ignoring
        /// all scaling, rotation and alignment state data.
        /// Color and Alpha are still to be used.
        /// 
        /// It is recommended to override this method, as the base class
        /// implementation saves the state, draws, then restores the state.
        /// </summary>
        /// <param name="destRect"></param>
        public virtual void Draw(Rectangle destRect)
        {
            // save the state
            Size displaySize = DisplaySize;
            OriginAlignment displayAlignment = DisplayAlignment ;
            double angle = RotationAngle;

            // reset the state
            DisplaySize = destRect.Size;
            DisplayAlignment = OriginAlignment.TopLeft;
            RotationAngle = 0;

            // draw with no state values
            Draw(destRect.Location);

            // restore the state.
            RotationAngle = angle;
            DisplayAlignment = displayAlignment;
            DisplaySize = displaySize;
        }
        /// <summary>
        /// For function use, see documentation of Surface.
        /// 
        /// Info for developers:
        /// This method should draw a portion of the surface to the screen, ignoring
        /// all scaling, rotation and alignment state data.
        /// Color and Alpha are still to be used.
        /// 
        /// This method automatically converts Rectangle structures into RectangleF 
        /// structures and calls the Draw overload which takes them.  This can be 
        /// overriden if the implementation is more natural to use integral values.
        /// </summary>
        /// <param name="srcRect"></param>
        /// <param name="destRect"></param>
        public virtual void Draw(Rectangle srcRect, Rectangle destRect)
        {
            Draw(new RectangleF(srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height),
                 new RectangleF(destRect.X, destRect.Y, destRect.Width, destRect.Height));
        }


        /// <summary>
        /// For function use, see documentation of Surface.
        /// 
        /// Info for developers:
        /// This method should draw a portion of the surface to the screen, using
        /// all scaling, rotation and alignment state data.
        /// 
        /// This method must be overriden.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="srcRect"></param>
        /// <param name="rotationCenterX"></param>
        /// <param name="rotationCenterY"></param>
        public abstract void Draw(float x, float y, Rectangle srcRect, float rotationCenterX, float rotationCenterY);

        /// <summary>
        /// For function use, see documentation of Surface.
        /// 
        /// Info for developers:
        /// This method should draw a portion of the surface to the screen, ignoring
        /// all scaling, rotation and alignment state data.
        /// Color and Alpha are still to be used.
        /// 
        /// This method must be overriden.
        /// </summary>
        /// <param name="srcRect"></param>
        /// <param name="destRect"></param>
        public abstract void Draw(RectangleF srcRect, RectangleF destRect);

        /// <summary>
        /// For function use, see documentation of Surface.
        /// 
        /// Info for developers:
        /// This method should draw the surface to the screen, using all the
        /// scaling, rotation, etc. state data in the stored Surface object.
        /// The base class method calls Draw(PointF).
        /// 
        /// This method may be overriden, if it is convenient to provide an 
        /// alternate implementation which takes integral drawing values.
        /// </summary>
        /// <param name="destPt"></param>
        public virtual void Draw(Point destPt)
        {
            Draw((float)destPt.X, (float)destPt.Y);
        }
        /// <summary>
        /// For function use, see documentation of Surface.
        /// 
        /// Info for developers:
        /// This method should draw the surface to the screen, using all the
        /// scaling, rotation, etc. state data in the stored Surface object.
        /// 
        /// This method must be overriden.
        /// </summary>
        /// <param name="destX"></param>
        /// <param name="destY"></param>
        public abstract void Draw(float destX, float destY);
        /// <summary>
        /// For function use, see documentation of Surface.
        /// 
        /// Info for developers:
        /// This method should draw the surface to the screen, using all the
        /// scaling, rotation, etc. state data in the stored Surface object,
        /// except for RotationCenter.  Use the point passed for the center
        /// of rotation.
        /// 
        /// This method must be overriden.
        /// </summary>
        /// <param name="destX"></param>
        /// <param name="destY"></param>
        /// <param name="rotationCenterX"></param>
        /// <param name="rotationCenterY"></param>
        public abstract void Draw(float destX, float destY, float rotationCenterX, float rotationCenterY);
        /// <summary>
        /// For function use, see documentation of Surface.
        /// 
        /// Info for developers:
        /// This method should draw the surface to the screen, with the same result
        /// as if Draw was called once for each Point passed.
        /// </summary>
        /// <param name="destPts"></param>
        public virtual void DrawPoints(Point[] destPts)
        {
            for (int i = 0; i < destPts.Length; i++)
                Draw(destPts[i]);
        }
        /// <summary>
        /// For function use, see documentation of Surface.
        /// 
        /// Info for developers:
        /// This method draws the surface at (0, 0).  The base class implementation
        /// simply calls Draw(Point.Empty).
        /// </summary>
        public virtual void Draw() { Draw(Point.Empty); }

        /// <summary>
        /// For function use, see documentation of Surface.
        /// 
        /// Info for developers:
        /// This method should draw the surface to the screen, with the same result
        /// as if Draw was called once for each src and dest rect pairs.
        /// It should be overridden, to minimize calls across managed/unmanaged boundaries.
        /// </summary>
        /// <param name="srcRects"></param>
        /// <param name="destRects"></param>
        /// <param name="start">Element in the array to start with.</param>
        /// <param name="length">Number of elements in the arrays to use.</param>
        public virtual void DrawRects(Rectangle[] srcRects, Rectangle[] destRects, int start, int length)
        {
            for (int i = 0; i < length; i++)
            {
                Draw(srcRects[i + start], destRects[i + start]);
            }
        }

        /// <summary>
        /// For function use, see documentation of Surface.
        /// 
        /// Info for developers:
        /// This method should draw the surface to the screen, with the same result
        /// as if Draw was called once for each src and dest rect pairs.
        /// It should be overridden, to minimize calls across managed/unmanaged boundaries.
        /// </summary>
        /// <param name="srcRects"></param>
        /// <param name="destRects"></param>
        /// <param name="start">Element in the arrays to start with.</param>
        /// <param name="length">Number of elements in the arrays to use.</param>
        public virtual void DrawRects(RectangleF[] srcRects, RectangleF[] destRects, int start, int length)
        {
            for (int i = 0; i < length; i++)
            {
                Draw(srcRects[i + start], destRects[i + start]);
            }
        }
        #endregion
        #region --- Queueing rects to draw to the screen ---

        private Queue<Rectangle> srcRectList;
        private Queue<Rectangle> destRectList;

        /// <summary>
        /// Sets up data structures to queue rects to draw to the screen.
        /// </summary>
        public void BeginQueueRects()
        {
            BeginQueueRects(100);
        }
        /// <summary>
        /// Sets up data structures to queue rects to draw to the screen.
        /// </summary>
        /// <param name="guessCount">A good guess for how many rects you are going to draw.</param>
        public void BeginQueueRects(int guessCount)
        {
            srcRectList = new Queue<Rectangle>(guessCount);
            destRectList = new Queue<Rectangle>(guessCount);
        }
        /// <summary>
        /// Adds a src/dest rectangle pair to the queue.  Make sure to call
        /// BeginQueueRects first.
        /// </summary>
        /// <param name="src_rect"></param>
        /// <param name="dest_rect"></param>
        public void QueueRect(Rectangle src_rect, Rectangle dest_rect)
        {
            srcRectList.Enqueue(src_rect);
            destRectList.Enqueue(dest_rect);
        }
        /// <summary>
        /// Ends adding rects to the queue and draws all of them to the screen.
        /// </summary>
        public void EndQueueRects()
        {
            Rectangle[] srcRects = srcRectList.ToArray();
            Rectangle[] destRects = destRectList.ToArray();

            DrawRects(srcRects, destRects, 0, srcRects.Length);

            srcRectList = null;
            destRectList = null;
        }

        #endregion
        #region --- Surface Data Manipulations ---

        /// <summary>
        /// Saves the surface data to the specified file in the specified format.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public abstract void SaveTo(string filename, ImageFileFormat format);

        /// <summary>
        /// Creates a new SurfaceImpl object which comes from a small sub-rectangle on this surface.
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="srcRect"></param>
        /// <returns></returns>
        public abstract SurfaceImpl CarveSubSurface(Surface surface, Rectangle srcRect);

        /// <summary>
        /// Used by Display.BuildPackedSurface.
        /// </summary>
        /// <param name="surf"></param>
        /// <param name="srcRect"></param>
        public abstract void SetSourceSurface(SurfaceImpl surf, Rectangle srcRect);


        /// <summary>
        /// Creates a PixelBuffer object with a copy of the pixel data, in the specified format.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public virtual PixelBuffer ReadPixels(PixelFormat format)
        {
            return ReadPixels(format, new Rectangle(Point.Empty, SurfaceSize));
        }
        /// <summary>
        /// Creates a PixelBuffer object with a copy of the pixel data in the 
        /// specified rectangle, in the specified format.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public abstract PixelBuffer ReadPixels(PixelFormat format, Rectangle rect);
        /// <summary>
        /// Writes pixel data to the surface.
        /// </summary>
        /// <param name="buffer"></param>
        public abstract void WritePixels(PixelBuffer buffer);
        /// <summary>
        /// Writes pixel data to the surface.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="startPoint"></param>
        public virtual void WritePixels(PixelBuffer buffer, Point startPoint)
        {
            // poor man's method
            PixelBuffer pixels = ReadPixels(PixelFormat.RGBA8888);

            pixels.CopyFrom(buffer, new Rectangle(Point.Empty, buffer.Size), startPoint, false);

            WritePixels(pixels);
        }

        #endregion


        #region --- Surface properties ---

        /// <summary>
        /// Gets or sets the state of the surface.
        /// </summary>
        public SurfaceState State
        {
            get { return mState; }
            set { mState = value; }
        }

        /// <summary>
        /// Gets or sets how many squares the surface should be broken into when drawn.
        /// </summary>
        public int TesselateFactor
        {
            get { return mTesselate; }
            set
            {
                if (value < 1) value = 1;

                mTesselate = value;
            }
        }
        /// <summary>
        /// Returns true if Dispose() has been called on this surface.
        /// </summary>
        public bool IsDisposed
        {
            get { return mIsDisposed; }
        }

        /// <summary>
        /// Gets or sets a bool value which indicates whether or not this surface
        /// should be considered for packing when Display.PackAllSurfaces() is 
        /// called.
        /// </summary>
        public bool ShouldBePacked
        {
            get { return mShouldBePacked; }
            set { mShouldBePacked = value; }
        }

        /// <summary>
        /// Get or sets the width of the surface in pixels when it will be displayed on screen.
        /// </summary>
        public int DisplayWidth
        {
            get { return (int)(mState.ScaleWidth * SurfaceWidth); }
            set { ScaleWidth = value / (double)SurfaceWidth; }
        }
        /// <summary>
        /// Gets or sets the height of the surface in pixels when it is displayed on screen.
        /// </summary>
        public int DisplayHeight
        {
            get { return (int)(mState.ScaleHeight * SurfaceHeight); }
            set { ScaleHeight = value / (double)SurfaceHeight; }
        }
        /// <summary>
        /// Gets or sets the Size of the area used by this surface when displayed on screen.
        /// </summary>
        public Size DisplaySize
        {
            get
            {
                Size sz = new Size(DisplayWidth, DisplayHeight);

                return sz;
            }
            set
            {
                DisplayWidth = value.Width;
                DisplayHeight = value.Height;
            }
        }

        /// <summary>
        /// Alpha value for displaying this surface.
        /// Valid values range from 0.0 (completely transparent) to 1.0 (completely opaque).
        /// Internally stored as a byte, so granularity is only 1/255.0.
        /// If a gradient is used, getting this property returns the alpha value for the top left
        /// corner of the gradient.
        /// </summary>
        public double Alpha
        {
            get { return Color.A / 255.0; }
            set
            {
                mState.ColorGradient.SetAlpha(value);
            }
        }
        /// <summary>
        /// Gets or sets the rotation angle in radians.
        /// Positive angles indicate rotation in the Counter-Clockwise direction.
        /// </summary>
        public virtual double RotationAngle
        {
            get { return mState.RotationAngle; }
            set { mState.RotationAngle = value; }
        }
        /// <summary>
        /// Gets or sets the rotation angle in degrees.
        /// Positive angles indicate rotation in the Counter-Clockwise direction.
        /// </summary>
        public double RotationAngleDegrees
        {
            get { return RotationAngle * 180.0 / Math.PI; }
            set { RotationAngle = value * Math.PI / 180.0; }
        }
        /// <summary>
        /// Gets or sets the point on the surface which is used to rotate around.
        /// </summary>
        public virtual OriginAlignment RotationCenter
        {
            get { return mState.RotationCenter; }
            set { mState.RotationCenter = value; }
        }
        /// <summary>
        /// Gets or sets the point where the surface is aligned to when drawn.
        /// </summary>
        public virtual OriginAlignment DisplayAlignment
        {
            get { return mState.DisplayAlignment; }
            set { mState.DisplayAlignment = value; }
        }

        /// <summary>
        /// Gets or sets the amount the width is scaled when this surface is drawn.
        /// 1.0 is no scaling.
        /// Scale values can be negative, this causes the surface to be mirrored
        /// in that direction.  This does not affect how the surface is aligned;
        /// eg. if DisplayAlignment is top-left and ScaleWidth &lt; 0, the surface 
        /// will still be drawn to the right of the point supplied to Draw(Point).
        /// </summary>
        public double ScaleWidth
        {
            get { return mState.ScaleWidth; }
            set { mState.ScaleWidth = value; }
        }
        /// <summary>
        /// Gets or sets the amount the height is scaled when this surface is drawn.
        /// 1.0 is no scaling.
        /// </summary>
        public double ScaleHeight
        {
            get { return mState.ScaleHeight; }
            set { mState.ScaleHeight = value; }
        }
        /// <summary>
        /// Sets the amount of scaling when this surface is drawn.  
        /// 1.0 is no scaling.
        /// Scale values can be negative, this causes the surface to be mirrored
        /// in that direction.  This does not affect how the surface is aligned;
        /// eg. if DisplayAlignment is top-left and ScaleWidth &lt; 0, the surface 
        /// will still be drawn to the right of the point supplied to Draw(Point).
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetScale(double width, double height)
        {
            ScaleWidth = width;
            ScaleHeight = height;
        }
        /// <summary>
        /// Gets the amount of scaling when this surface is drawn.
        /// 1.0 is no scaling.
        /// Scale values can be negative, this causes the surface to be mirrored
        /// in that direction.  This does not affect how the surface is aligned;
        /// eg. if DisplayAlignment is top-left and ScaleWidth &lt; 0, the surface 
        /// will still be drawn to the right of the point supplied to Draw(Point).
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void GetScale(out double width, out double height)
        {
            width = mState.ScaleWidth;
            height = mState.ScaleHeight;
        }


        /// <summary>
        /// Gets or sets the multiplicative color for this surface.
        /// Setting this is equivalent to setting the ColorGradient property
        /// with a gradient with the same color in all corners.  If a gradient
        /// is being used, getting this property returns the top-left color in the gradient.
        /// </summary>
        public virtual Color Color
        {
            get { return mState.Color; }
            set { mState.Color = value; }
        }
        /// <summary>
        /// Gets or sets the gradient for this surface.
        /// </summary>
        public virtual Gradient ColorGradient
        {
            get { return mState.ColorGradient; }
            set { mState.ColorGradient = value; }
        }
        /// <summary>
        /// Increments the rotation angle of this surface.
        /// </summary>
        /// <param name="radians">Value in radians to increase the rotation by.</param>
        public void IncrementRotationAngle(double radians)
        {
            mState.RotationAngle += radians;
        }
        /// <summary>
        /// Increments the rotation angle of this surface.  Value supplied is in degrees.
        /// </summary>
        /// <param name="degrees"></param>
        public void IncrementRotationAngleDegrees(double degrees)
        {
            mState.IncrementRotationAngleDegrees(degrees);
        }

        /// <summary>
        /// Gets the width of the source surface in pixels.
        /// </summary>
        public virtual int SurfaceWidth { get { return SurfaceSize.Width; } }
        /// <summary>
        /// Gets the height of the source surface in pixels.
        /// </summary>
        public virtual int SurfaceHeight { get { return SurfaceSize.Height; } }
        /// <summary>
        /// Gets the Size of the source surface in pixels.
        /// </summary>
        public abstract Size SurfaceSize { get; }


        /// <summary>
        /// Checks to see whether the surface pixels all have
        /// alpha value less than the value of the AlphaThreshold of the
        /// display object..
        /// </summary>
        /// <returns></returns>
        public abstract bool IsSurfaceBlank();
        /// <summary>
        /// Checks to see whether the surface pixels all have
        /// alpha value less than the given value.
        /// </summary>
        /// <param name="alphaThreshold">The alpha value below which to consider 
        /// a pixel blank.  In the range 0 &lt;= alphaThreshold &lt;= 255.</param>
        /// <returns></returns>
        public abstract bool IsSurfaceBlank(int alphaThreshold);

        /// <summary>
        /// Checks to see whether all the pixels along the given row are all
        /// transparent, within the threshold.
        /// </summary>
        /// <param name="row">Which row.  Valid range is between 0 and SurfaceSize.Height - 1.</param>
        /// <returns></returns>
        public abstract bool IsRowBlank(int row);
        /// <summary>
        /// Checks to see whether all the pixels along the given column are all
        /// transparent, within the threshold.
        /// </summary>
        /// <param name="col">Which column.  Valid range is between 0 and SurfaceSize.Width - 1.</param>
        /// <returns></returns>
        public abstract bool IsColumnBlank(int col);

        #endregion

        #region --- Protected Methods ---

        /// <summary>
        /// Scans a memory area to see if it entirely contains pixels which won't be
        /// seen when drawn.
        /// </summary>
        /// <param name="pixelData">Pointer to the data</param>
        /// <param name="row">Which row to check</param>
        /// <param name="cols">How many columns to check</param>
        /// <param name="strideInBytes">The stride of each row</param>
        /// <param name="alphaThreshold">The maximum value of alpha to consider a pixel transparent.</param>
        /// <param name="alphaMask">The mask to use to extract the alpha value from the data.</param>
        /// <param name="alphaShift">How many bits to shift it to get alpha in the range of 0-255.
        /// For example, if alphaMask = 0xff000000 then alphaShift should be 24.</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        protected bool IsRowBlankScanARGB(IntPtr pixelData, int row, int cols, int strideInBytes,
            int alphaThreshold, uint alphaMask, int alphaShift)
        {
            unsafe
            {
                uint* ptr = (uint*)pixelData;

                int start = row * strideInBytes / sizeof(uint);

                for (int i = 0; i < cols; i++)
                {
                    int index = start + i;
                    uint pixel = ptr[index];
                    byte alpha = (byte)((pixel & alphaMask) >> alphaShift);

                    if (alpha > alphaThreshold)
                    {
                        return false;
                    }

                }
            }

            return true;
        }
        /// <summary>
        /// Scans a memory area to see if it entirely contains pixels which won't be
        /// seen when drawn.
        /// </summary>
        /// <param name="pixelData">Pointer to the data</param>
        /// <param name="col">Which col to check</param>
        /// <param name="rows">How many columns to check</param>
        /// <param name="strideInBytes">The stride of each row</param>
        /// <param name="alphaThreshold">The maximum value of alpha to consider a pixel transparent.</param>
        /// <param name="alphaMask">The mask to use to extract the alpha value from the data.</param>
        /// <param name="alphaShift">How many bits to shift it to get alpha in the range of 0-255.
        /// For example, if alphaMask = 0xff000000 then alphaShift should be 24.</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        protected bool IsColBlankScanARGB(IntPtr pixelData, int col, int rows, int strideInBytes,
            int alphaThreshold, uint alphaMask, int alphaShift)
        {
            unsafe
            {
                uint* ptr = (uint*)pixelData;


                for (int i = 0; i < rows; i++)
                {
                    int index = col + i * strideInBytes / sizeof(uint);
                    uint pixel = ptr[index];
                    byte alpha = (byte)((pixel & alphaMask) >> alphaShift);

                    if (alpha > alphaThreshold)
                    {
                        return false;
                    }

                }
            }

            return true;

        }
        #endregion



        #region --- IRenderTargetImpl Members ---

        /// <summary>
        /// Utility function which can be called by BeginFrame to begin
        /// a render pass.
        /// </summary>
        public abstract void BeginRender();
        /// <summary>
        /// Utility function which can be called by EndFrame to end a render pass.
        /// </summary>
        public abstract void EndRender();

        Size IRenderTargetImpl.Size
        {
            get { return SurfaceSize; }
        }

        int IRenderTargetImpl.Width
        {
            get { return SurfaceWidth; }
        }

        int IRenderTargetImpl.Height
        {
            get { return SurfaceHeight; }
        }

        #endregion







    };

}
