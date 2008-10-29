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

namespace ERY.AgateLib.Sprites
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
    public class PackedSpriteFrame : ISpriteFrame 
    {
        Point mOffset = new Point(0, 0);
        bool mIsBlank = true;

        Size mDisplaySize;
        Rectangle mSrcRect;

        internal PackedSpriteFrame()
        {
        }

        /// <summary>
        /// Copies this object.
        /// 
        /// Actually, this just returns this
        /// object, since there are no unmanaged resources to deal with.  
        /// Be sure to Dispose the result when finished with it.
        /// </summary>
        /// <returns></returns>
        public PackedSpriteFrame Clone()
        {
            return this;
        }

        /// <summary>
        /// Gets or sets the source rectangle for this frame.
        /// </summary>
        public Rectangle SourceRect
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
        //// <summary>
        //// Gets the original size of the frame.
        //// </summary>
        //public Size OriginalSize
        //{
        //    get { return mOriginalSize; }
        //}

        internal Point FrameOffset
        {
            get { return mOffset; }
        }

        /// <summary>
        /// Draws this surface at the specified destination point with the specified rotation
        /// center.
        /// </summary>
        /// <param name="surf"></param>
        /// <param name="dest_x"></param>
        /// <param name="dest_y"></param>
        /// <param name="rotationCenterX"></param>
        /// <param name="rotationCenterY"></param>
        public void Draw(Surface surf, float dest_x, float dest_y, float rotationCenterX, float rotationCenterY)
        {
            // calculate scaling.
            float scaleX = mDisplaySize.Width / (float)mSrcRect.Width;
            float scaleY = mDisplaySize.Height / (float)mSrcRect.Height;

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

        #region ISpriteFrame Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest_x"></param>
        /// <param name="dest_y"></param>
        /// <param name="rotationCenterX"></param>
        /// <param name="rotationCenterY"></param>
        public void Draw(float dest_x, float dest_y, float rotationCenterX, float rotationCenterY)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public Surface Surface
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }

}
