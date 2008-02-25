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
using System.IO;
using System.Text;

using ERY.AgateLib.Geometry;
using ERY.AgateLib.Utility;

namespace ERY.AgateLib.ImplBase
{
    /// <summary>
    /// Abstract base class for implementing the Display object.
    /// </summary>
    public abstract class DisplayImpl : DriverImplBase
    {
        private double mAlphaThreshold = 5.0 / 255.0;

        private IRenderTarget mRenderTarget;


        /// <summary>
        /// Gets or sets the current render target.
        /// </summary>
        public IRenderTarget RenderTarget
        {
            get
            {
                return mRenderTarget;
            }
            set
            {
                if (value != mRenderTarget)
                {
                    if (mInFrame)
                        throw new Exception("Cannot change render target between BeginFrame and EndFrame");

                    IRenderTarget old = mRenderTarget;
                    mRenderTarget = value;

                    OnRenderTargetChange(old);
                }
            }
        }

        /// <summary>
        /// The pixelformat that created surfaces should use.
        /// </summary>
        public abstract PixelFormat DefaultSurfaceFormat { get; }

        /// <summary>
        /// Event raised when the current render target is changed.
        /// </summary>
        /// <param name="oldRenderTarget"></param>
        protected abstract void OnRenderTargetChange(IRenderTarget oldRenderTarget);
        /// <summary>
        /// Event raised when the render target is resized.
        /// </summary>
        protected abstract void OnRenderTargetResize();

        ///// <summary>
        ///// Creates a DisplayWindowImpl derived object.
        ///// </summary>
        ///// <param name="title"></param>
        ///// <param name="clientWidth"></param>
        ///// <param name="clientHeight"></param>
        ///// <param name="allowResize"></param>
        ///// <param name="iconFile"></param>
        ///// <param name="startFullscreen"></param>
        ///// <returns></returns>
        //public abstract DisplayWindowImpl CreateDisplayWindow(string title, int clientWidth, int clientHeight, string iconFile, bool startFullscreen, bool allowResize);
        ///// <summary>
        ///// Creates a DisplayWindowImpl derived object.
        ///// </summary>
        //public abstract DisplayWindowImpl CreateDisplayWindow(System.Windows.Forms.Control renderTarget);
       
        /// <summary>
        /// Creates a DisplayWindowImpl derived object.
        /// </summary>
        /// <param name="windowParams"></param>
        /// <returns></returns>
        public abstract DisplayWindowImpl CreateDisplayWindow(CreateWindowParams windowParams);
       

        /// <summary>
        /// Creates a SurfaceImpl derived object.
        /// </summary>
        public abstract SurfaceImpl CreateSurface(string fileName);
        /// <summary>
        /// Creates a SurfaceImpl derived object from a stream containing 
        /// the file contents.
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public abstract SurfaceImpl CreateSurface(Stream fileStream);

        /// <summary>
        /// Creates a SurfaceImpl derived object.
        /// </summary>
        public abstract SurfaceImpl CreateSurface(Size surfaceSize);

        /// <summary>
        /// Creates a SurfaceImpl derived object.
        /// Forwards the call to CreateSurface(Size).
        /// </summary>
        public SurfaceImpl CreateSurface(int width, int height)
        {
            return CreateSurface(new Size(width, height));
        }
        /// <summary>
        /// Creates a FontSurfaceImpl derived object.
        /// </summary>
        /// <param name="fontFamily"></param>
        /// <param name="sizeInPoints"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public abstract FontSurfaceImpl CreateFont(string fontFamily, 
            float sizeInPoints, FontStyle style);

        /// <summary>
        /// Creates a BitmapFontImpl object from the specified options.
        /// </summary>
        /// <param name="bitmapOptions"></param>
        /// <returns></returns>
        public abstract FontSurfaceImpl CreateFont(BitmapFontOptions bitmapOptions);

        /// <summary>
        /// Gets or sets the threshold value for alpha transparency below which
        /// pixels are considered completely transparent, and may not be drawn.
        /// </summary>
        public double AlphaThreshold
        {
            get { return mAlphaThreshold; }
            set
            {
                if (value < 0)
                    mAlphaThreshold = 0;
                else if (value > 1)
                    mAlphaThreshold = 1;
                else
                    mAlphaThreshold = value;
            }
        }

        #region --- BeginFrame / EndFrame and DeltaTime stuff ---

        private bool mInFrame = false;
        private double mDeltaTime;
        private double mLastTime;
        private bool mRanOnce = false;

        private double mFPSStart;
        private int mFrames = 0;
        private double mFPS = 0;

        // BeginFrame and EndFrame must be called at the start and end of each frame.
        /// <summary>
        /// Must be called at the start of each frame.
        /// </summary>
        public void BeginFrame()
        {
            if (mInFrame)
                throw new InvalidOperationException(
                    "Called BeginFrame() while already inside a BeginFrame..EndFrame block!\n" +
                    "Did you forget to call EndFrame()?");

            mInFrame = true;

            OnBeginFrame();
        }

        /// <summary>
        /// A version of EndFrame must be called at the end of each frame.
        /// This version allows the caller to indicate to the implementation whether or
        /// not it is preferred to wait for the vertical blank to do the drawing.
        /// </summary>
        public void EndFrame()
        {
            CheckInFrame("EndFrame");

            OnEndFrame();

            mFrames++;
            mInFrame = false;


            CalcDeltaTime();

        }

        private void CalcDeltaTime()
        {
            double now = Core.Platform.GetTime();

            if (mRanOnce)
            {
                mDeltaTime = now - mLastTime;
                mLastTime = now;

                if (now - mFPSStart > 200)
                {
                    double time = (now - mFPSStart) * 0.001;

                    // average current framerate with that of the last update
                    mFPS = (mFrames / time) * 0.8 + mFPS * 0.2;

                    mFPSStart = now;
                    mFrames = 0;

                }

                // hack to make sure delta time is never zero.
                if (mDeltaTime == 0.0)
                {
                    System.Threading.Thread.Sleep(1);
                    CalcDeltaTime();
                    return;
                }
            }
            else
            {
                mDeltaTime = 0;
                mLastTime = now;

                mFPSStart = now;
                mFrames = 0;

                mRanOnce = true;
            }
        }

        /// <summary>
        /// Called by BeginFrame to let the driver know to do its setup stuff for starting
        /// the next render pass.
        /// </summary>
        protected abstract void OnBeginFrame();
        /// <summary>
        /// Called by EndFrame to let the driver know that it's time to swap buffers or whatever
        /// is required to finish rendering the frame.
        /// </summary>
        protected abstract void OnEndFrame();

        /// <summary>
        /// Checks to see whether or not we are currently inside a
        /// BeginFrame..EndFrame block, and throws an exception if
        /// we are not.  This is only meant to be called
        /// from functions which must operate between these calls.  
        /// </summary>
        /// <param name="functionName">The name of the calling function, 
        /// for debugging purposes.</param>
        public void CheckInFrame(string functionName)
        {
            if (!mInFrame)
            {
                throw new InvalidOperationException(
                    functionName + " called outside of BeginFrame..EndFrame block!" +
                    "Did you forget to call BeginFrame() before doing drawing?");

            }
        }

        /// <summary>
        /// Gets the amount of time in milliseconds that has passed between this frame
        /// and the last one.
        /// </summary>
        public double DeltaTime
        {
            get
            {
                return mDeltaTime;
            }
        }
        /// <summary>
        /// Provides a means to set the value returned by DeltaTime.
        /// </summary>
        /// <param name="deltaTime"></param>
        public void SetDeltaTime(double deltaTime)
        {
            mDeltaTime = deltaTime;
        }
        /// <summary>
        /// Gets the framerate
        /// </summary>
        public double FramesPerSecond
        {
            get { return mFPS; }
        }

        #endregion
        /// <summary>
        /// Returns the maximum size a surface object can be.
        /// </summary>
        public abstract Size MaxSurfaceSize { get; }

        #region --- Clip Rect stuff ---

        /// <summary>
        /// Set the current clipping rect.
        /// </summary>
        /// <param name="newClipRect"></param>
        public abstract void SetClipRect(Rectangle newClipRect);
        /// <summary>
        /// Pushes a clip rect onto the clip rect stack.
        /// </summary>
        /// <param name="newClipRect"></param>
        public abstract void PushClipRect(Rectangle newClipRect);
        /// <summary>
        /// Pops the clip rect and restores the previous clip rect.
        /// </summary>
        public abstract void PopClipRect();

        #endregion
        #region --- Direct modification of the back buffer ---

        /// <summary>
        /// Clears the buffer to black.
        /// </summary>
        public virtual void Clear()
        {
            Clear(Color.Black);
        }
        /// <summary>
        /// Clears the buffer to the specified color.
        /// </summary>
        /// <param name="color"></param>
        public abstract void Clear(Color color);
        /// <summary>
        /// Clears a region of the buffer to the specified color.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="dest"></param>
        public abstract void Clear(Color color, Rectangle dest);

        /// <summary>
        /// Draws an ellipse by making a bunch of connected lines.
        /// 
        /// Info for developers:
        /// The base class implements this by calculating points on the circumference of
        /// the ellipse, then making a call to DrawLines.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        public virtual void DrawEllipse(Rectangle rect, Color color)
        {
            Point center = new Point(rect.Left + rect.Width / 2,
                                     rect.Top + rect.Height / 2);

            double radiusX = rect.Width / 2;
            double radiusY = rect.Height / 2;
            double h = Math.Pow(radiusX - radiusY, 2) / Math.Pow(radiusX + radiusY, 2);

            //Ramanujan's second approximation to the circumference of an ellipse.
            double circumference =
                Math.PI * (radiusX + radiusY) * (1 + 3 * h / (10 + Math.Sqrt(4 - 3 * h)));

            // we will take the circumference as being the number of points to draw
            // on the ellipse.
            Point[] pts = new Point[(int)Math.Ceiling(circumference)];
            double step = 2 * Math.PI / (pts.Length - 1);

            for (int i = 0; i < pts.Length; i++)
            {
                pts[i] = new Point((int)(center.X + radiusX * Math.Cos(step * i) + 0.5),
                                   (int)(center.Y + radiusY * Math.Sin(step * i) + 0.5));
            }

            DrawLines(pts, color);

            //Public Sub DrawCircle(ByVal NumPoints As Integer, ByVal CenterX As Single, ByVal CenterY As Single, ByVal Radius As Single, ByVal Color As Integer, ByVal Thickness As Single)
            //Dim pts(NumPoints) As Vector2, pointStep As Single

            //pointStep = (Math.PI * 2) / NumPoints

            //For i As Integer = 0 To NumPoints
            //pts(i).X = CenterX + Math.Cos(pointStep * i) * Radius
            //pts(i).Y = CenterY + Math.Sin(pointStep * i) * Radius
            //Next

            //Try
            //mLine.Width = Thickness
            //mLine.Begin()
            //mLine.Draw(pts, Color)
            //mLine.End()
            //Catch Err As Exception

            //End Try
            //End Sub
        }

        /// <summary>
        /// Draws a line between the two specified end-points.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="color"></param>
        public abstract void DrawLine(int x1, int y1, int x2, int y2, Color color);
        /// <summary>
        /// Draws a line between the two specified endpoints.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="color"></param>
        public abstract void DrawLine(Point a, Point b, Color color);
        /// <summary>
        /// Draws a bunch of connected points.
        /// 
        /// Info for developers:
        /// The base class implements this by making several calls to DrawLine.
        /// You may want to override this one to minimize state changes.
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="color"></param>
        public virtual void DrawLines(Point[] pt, Color color)
        {
            for (int i = 0; i < pt.Length - 1; i++)
                DrawLine(pt[i], pt[i + 1], color);
        }
        /// <summary>
        /// Draws a bunch of unconnected lines.
        /// <para>
        /// Info for developers:
        /// pt should be an array whose length is even.</para>
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="color"></param>
        public virtual void DrawLineSegments(Point[] pt, Color color)
        {
            for (int i = 0; i < pt.Length; i += 2)
                DrawLine(pt[i], pt[i + 1], color);
        }

        /// <summary>
        /// Draws the outline of a rectangle.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        public abstract void DrawRect(Rectangle rect, Color color);
        /// <summary>
        /// Draws a filled rectangle.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        public abstract void FillRect(Rectangle rect, Color color);
        /// <summary>
        /// Draws a filled rectangle with a gradient.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        public abstract void FillRect(Rectangle rect, Gradient color);

        #endregion

        /// <summary>
        /// Builds a surface of the specified size, using the information
        /// generated by the SurfacePacker.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="packedRects"></param>
        /// <returns></returns>
        public virtual Surface BuildPackedSurface(Size size, SurfacePacker.RectPacker<Surface> packedRects)
        {
            PixelBuffer buffer = new PixelBuffer(Display.DefaultSurfaceFormat, size);

            foreach (SurfacePacker.RectHolder<Surface> rect in packedRects)
            {
                Surface surf = rect.Tag;
                Rectangle dest = rect.Rect;

                PixelBuffer pixels = surf.ReadPixels();

                buffer.CopyFrom(pixels, new Rectangle(Point.Empty, surf.SurfaceSize),
                    dest.Location, false);
            }

            Surface retval = new Surface(buffer);

            foreach (SurfacePacker.RectHolder<Surface> rect in packedRects)
            {
                rect.Tag.SetSourceSurface(retval, rect.Rect);
            }

            return retval;
            /*
            Surface retval = new Surface(size);

            IRenderTarget old = Display.RenderTarget;
            Display.RenderTarget = retval;

            Display.BeginFrame();
            Display.Clear(0, 0, 0, 0);

            foreach (SurfacePacker.RectHolder<Surface> rect in packedRects)
            {
                rect.Tag.Draw(rect.Rect);
            }
            
            Display.EndFrame();
            Display.RenderTarget = old;

            foreach (SurfacePacker.RectHolder<Surface> rect in packedRects)
            {
                rect.Tag.SetSourceSurface(retval, rect.Rect);
            }

            return retval;
             * */
        }

        /// <summary>
        /// Gets or sets VSync flag.
        /// There is no need to call base.VSync if overriding this member.
        /// </summary>
        public virtual bool VSync
        {
            get { return true; }
            set { }
        }


        /// <summary>
        /// Enumerates a list of screen modes.
        /// </summary>
        /// <returns></returns>
        public virtual ScreenMode[] EnumScreenModes()
        {
            return new ScreenMode[] { };
        }

        /// <summary>
        /// Flushes the 2D draw buffer, if applicable.
        /// </summary>
        public abstract void FlushDrawBuffer();

        /// <summary>
        /// Sets the boundary coordinates of the window.
        /// </summary>
        /// <param name="region"></param>
        public abstract void SetOrthoProjection(Rectangle region);

        /// <summary>
        /// Gets the capabilities of the Display object.
        /// </summary>
        public abstract IDisplayCaps Caps { get; }

        /// <summary>
        /// Gets all the light settings from the LightManager.
        /// </summary>
        /// <param name="lights"></param>
        public abstract void DoLighting(LightManager lights);

    }
}
