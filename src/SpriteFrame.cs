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

namespace ERY.AgateLib
{
    /// <summary>
    /// Class for a single frame of a sprite.
    /// 
    /// This class can automatically trim the frame, so that extra space around the
    /// object which is transparent is not drawn.  This is taken advantage of if 
    /// surfaces are packed to create a tighter packing and fit more objects on
    /// the same texture.
    /// </summary>
    public class SpriteFrame : IDisposable
    {
        Surface mSurface;
        Point mOffset = new Point(0, 0);
        bool mIsBlank = true;

        Size mOriginalSize;
        Size mDisplaySize;

        internal SpriteFrame()
        {
        }
        /// <summary>
        /// Destroys this SpriteFrame.
        /// </summary>
        ~SpriteFrame()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes of unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {

            if (mSurface != null)
            {
                mSurface.Dispose();
                mSurface = null;

                mOriginalSize = new Size(-1, -1);
            }

            if (disposing)
                GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Sets the frame of this object, and does not trim it.
        /// </summary>
        /// <param name="srcSurface"></param>
        /// <param name="offset"></param>
        /// <param name="originalSize"></param>
        public void SetFrameNoTrim(Surface srcSurface, Point offset, Size originalSize)
        {
            mSurface = srcSurface;
            mOffset = offset;

            mOriginalSize = originalSize;
            mDisplaySize = mOriginalSize;

            mIsBlank = false;
        }
        /// <summary>
        /// Sets the frame from a section of the source surface.
        /// </summary>
        /// <param name="srcSurface"></param>
        /// <param name="location"></param>
        /// <param name="size"></param>
        public void SetFrame(Surface srcSurface, Point location, Size size)
        {
            SetFrame(srcSurface, new Rectangle(location, size));
        }
        /// <summary>
        /// Sets the frame from a section of the source surface.
        /// </summary>
        /// <param name="srcSurface"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetFrame(Surface srcSurface, int x, int y, int width, int height)
        {
            SetFrame(srcSurface, new Rectangle(x, y, width, height));
        }
        /// <summary>
        /// Sets the frame from a section of the source surface.
        /// </summary>
        /// <param name="srcSurface"></param>
        /// <param name="rect"></param>
        public void SetFrame(Surface srcSurface, Rectangle rect)
        {
            mOriginalSize = rect.Size;

            Private_SetFrame(srcSurface, rect);
        }
        private void Private_SetFrame(Surface srcSurface, Rectangle rect)
        {
            Private_SetFrame(srcSurface, rect, true);
        }
        private void Private_SetFrame(Surface srcSurface, Rectangle rect, bool trim)
        {
            // copy the source surface to a new memory surface
            Surface surface = srcSurface.CarveSubSurface(rect); 
            
            // check to see if this frame is completely blank
            if (surface.IsSurfaceBlank())
            {
                surface.Dispose();
                mSurface = null;
                mIsBlank = true;
                return;
            }

            mIsBlank = false;
            mSurface = surface;

            if (trim)
                TrimFrame();
        }

        private void TrimFrame()
        {
            Rectangle startRect = new Rectangle(0, 0, mSurface.SurfaceWidth, mSurface.SurfaceHeight);

            //mSurface.SaveTo("Test.png");

            // now get rid of extra junk
            Rectangle newRect = startRect;

            while (mSurface.IsRowBlank(newRect.Top))
            {
                newRect.Y++;
                newRect.Height--;

                if (newRect.Height == 0)
                {
                    mIsBlank = true;

                    return;
                }
            }
            while (mSurface.IsRowBlank(newRect.Bottom - 1))
            {
                newRect.Height--;

                if (newRect.Height == 0)
                {
                    mIsBlank = true;

                    return;
                }
            }
            while (mSurface.IsColumnBlank(newRect.Left))
            {
                newRect.X++;
                newRect.Width--;


                if (newRect.Width == 0)
                {
                    mIsBlank = true;

                    return;
                }
            }
            while (mSurface.IsColumnBlank(newRect.Right - 1))
            {
                newRect.Width--;

                if (newRect.Width == 0)
                {
                    mIsBlank = true;

                    return;
                }
            }
            /*
            // make sure there's a one pixel border of blanks, if possible.
            if (newRect.Height < mSurface.SurfaceHeight)
            {
                newRect.Y--;
                newRect.Height += 2;
            }
            if (newRect.Width < mSurface.SurfaceWidth)
            {
                newRect.X--;
                newRect.Width += 2;
            }
            */
            mIsBlank = false;

            // now check to see if we need to redefine our existing rect.
            if (newRect.Equals(startRect) == false)
            {
                Surface oldSurface = mSurface;

                Private_SetFrame(oldSurface, newRect);

                System.Diagnostics.Debug.Assert(oldSurface != mSurface);

                mOffset = newRect.Location;

                oldSurface.Dispose();

            }
        }

        /// <summary>
        /// Returns true if the entire frame is transparent.
        /// </summary>
        public bool IsBlank
        {
            get { return mIsBlank; }
        }

        /// <summary>
        /// Returns the surface object which is drawn.
        /// </summary>
        public Surface Surface
        {
            get { return mSurface; }
        }
        /// <summary>
        /// Gets or sets the display size.
        /// </summary>
        public Size DisplaySize
        {
            get { return mDisplaySize; }
            set { mDisplaySize = value; }
        }
        /// <summary>
        /// Gets the original size of the frame.
        /// </summary>
        public Size OriginalSize
        {
            get { return mOriginalSize; }
        }

        internal Point FrameOffset
        {
            get { return mOffset; }
        }

        /// <summary>
        /// Draws this surface at the specified destination point with the specified rotation
        /// center.
        /// </summary>
        /// <param name="dest_x"></param>
        /// <param name="dest_y"></param>
        /// <param name="rotationCenterX"></param>
        /// <param name="rotationCenterY"></param>
        public void Draw(float dest_x, float dest_y, float rotationCenterX, float rotationCenterY)
        {
            // calculate scaling.
            float scaleX = mDisplaySize.Width / (float)mOriginalSize.Width;
            float scaleY = mDisplaySize.Height / (float)mOriginalSize.Height;

            mSurface.SetScale(scaleX, scaleY);

            mSurface.Draw(dest_x + (mOffset.X * scaleX),
                          dest_y + (mOffset.Y * scaleY),
                          rotationCenterX - (mOffset.X * scaleX),
                          rotationCenterY - (mOffset.Y * scaleY));
        }
    }

}
