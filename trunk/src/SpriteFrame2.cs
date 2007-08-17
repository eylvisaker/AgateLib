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

namespace ERY.AgateLib
{
    /// <summary>
    /// Class for a single frame of a sprite.
    /// 
    /// This class can automatically trim the frame, so that extra space around the
    /// object which is transparent is not drawn.  This is taken advantage of if 
    /// surfaces are packed to create a tighter packing and fit more objects on
    /// the same texture.
    /// 
    /// SpriteFrame contains a reference count.  If you manually copy it, be sure
    /// to call AddRef unless you use the Clone method.
    /// </summary>
    public class SpriteFrame2
    {
        Point mOffset = new Point(0, 0);
        bool mIsBlank = true;

        Size mOriginalSize;
        Size mDisplaySize;
        Rectangle mSrcRect;

        internal SpriteFrame2()
        {
        }

        /// <summary>
        /// Copies this object.
        /// 
        /// Actually, this just returns this
        /// object.  Be sure to Dispose the result when finished with it.
        /// </summary>
        /// <returns></returns>
        public SpriteFrame2 Clone()
        {
            return this;
        }

        /// <summary>
        /// Gets or sets the source rectangle for this frame.
        /// </summary>
        public Rectangle SrcRect
        {
            get { return mSrcRect; }
            set { mSrcRect = value; }
        }

        /// <summary>
        /// Gets or sets the offset for drawing this frame.
        /// </summary>
        public Point Offset
        {
            get { return mOffset; }
            set { mOffset = value; }
        }
        
        /*
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
            PixelBuffer pixels = surface.ReadPixels();

            //pixels.SaveTo("test.png", ImageFileFormat.Png);

            // check to see if this frame is completely blank
            if (pixels.IsBlank())
            {
                surface.Dispose();
                mSurface = null;
                mIsBlank = true;
                return;
            }

            mIsBlank = false;
            mSurface = surface;

            if (trim)
                TrimFrame(pixels);
        }
        //static bool inTrimFrame = false;

        private void TrimFrame(PixelBuffer pixels)
        {
            //if (inTrimFrame)
            //    return;
            //inTrimFrame = true;

            Rectangle startRect = new Rectangle(0, 0, mSurface.SurfaceWidth, mSurface.SurfaceHeight);

            //mSurface.SaveTo("Test.png");

            // now get rid of extra junk
            Rectangle newRect = startRect;

            while (pixels.IsRowBlank(newRect.Top))
            {
                newRect.Y++;
                newRect.Height--;

                if (newRect.Height == 0)
                {
                    mIsBlank = true;

                    return;
                }
            }
            while (pixels.IsRowBlank(newRect.Bottom - 1))
            {
                newRect.Height--;

                if (newRect.Height == 0)
                {
                    mIsBlank = true;

                    return;
                }
            }
            while (pixels.IsColumnBlank(newRect.Left))
            {
                newRect.X++;
                newRect.Width--;


                if (newRect.Width == 0)
                {
                    mIsBlank = true;

                    return;
                }
            }
            while (pixels.IsColumnBlank(newRect.Right - 1))
            {
                newRect.Width--;

                if (newRect.Width == 0)
                {
                    mIsBlank = true;

                    return;
                }
            }

            // make sure there's a one pixel border of blanks, if possible.
            if (newRect.X > 0)
            {
                newRect.X--;
                newRect.Width++;
            }
            if (newRect.Width < mSurface.SurfaceWidth)
            {
                newRect.Width += 1;
            }

            if (newRect.Y > 0)
            {
                newRect.Y--;
                newRect.Height++;
            }
            else if (newRect.Height < mSurface.SurfaceHeight)
            {
                newRect.Height++;
            }


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
            //inTrimFrame = false;

        }
        */



        /// <summary>
        /// Returns true if the entire frame is transparent.
        /// </summary>
        public bool IsBlank()
        {
            return false;
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
        public void Draw(Surface surf, float dest_x, float dest_y, float rotationCenterX, float rotationCenterY)
        {
            // calculate scaling.
            float scaleX = mDisplaySize.Width / (float)mOriginalSize.Width;
            float scaleY = mDisplaySize.Height / (float)mOriginalSize.Height;

            surf.SetScale(scaleX, scaleY);

            surf.Draw(dest_x + (mOffset.X * scaleX),
                      dest_y + (mOffset.Y * scaleY),
                      mSrcRect,
                      rotationCenterX - (mOffset.X * scaleX),
                      rotationCenterY - (mOffset.Y * scaleY));

            //mSurface.Draw(dest_x + (mOffset.X * scaleX),
            //              dest_y + (mOffset.Y * scaleY),
            //              rotationCenterX - (mOffset.X * scaleX),
            //              rotationCenterY - (mOffset.Y * scaleY));
        }
    }

}
