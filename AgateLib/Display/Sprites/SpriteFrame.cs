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
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace AgateLib.Display.Sprites
{
    /// <summary>
    /// Basic interface implemented by a particular frame in a sprite.
    /// </summary>
    public interface ISpriteFrame
    {
        /// <summary>
        /// Gets the surface object the frame is drawn from
        /// </summary>
        Texture2D Texture { get; }

        /// <summary>
        /// Gets the source rectangle on the surface the frame is drawn from.
        /// </summary>
        Rectangle? SourceRect { get; }

        /// <summary>
        /// Draws the sprite frame.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="dest"></param>
        /// <param name="rotationCenter"></param>
        /// <param name="color"></param>
        /// <param name="rotationAngle"></param>
        /// <param name="layerDepth"></param>
        void Draw(SpriteBatch spriteBatch, Vector2 dest, Vector2 rotationCenter, Color color, float rotationAngle, float layerDepth);
    }

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
    public class SpriteFrame : ISpriteFrame
    {
        private Texture2D texture;
        private Point mOffset = new Point(0, 0);

        private Rectangle sourceRect;
        private Size mDisplaySize;
        private Size mSpriteSize;
        private Dictionary<string, CollisionRegion> mRegions = new Dictionary<string, CollisionRegion>();

        public SpriteFrame(Texture2D texture)
        {
            this.texture = texture;
        }

        /// <summary>
        /// Gets or sets the source rectangle for this frame.
        /// </summary>
        public Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }

        /// <summary>
        /// Gets or sets the offset for drawing this frame.
        /// </summary>
        public Point Anchor
        {
            get { return mOffset; }
            set { mOffset = value; }
        }

        internal Size SpriteSize
        {
            get { return mSpriteSize; }
            set { mSpriteSize = value; }
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
        /// Draws the sprite frame at the specified location rotated around the specified point.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, Vector2 dest, Vector2 rotationCenter, Vector2 scale, Color color, float rotationAngle, float layerDepth)
        {
            if (FlipHorizontal)
            {
                dest.X -= (SourceRect.Width - mOffset.X) * scaleX;

                scaleX *= -1;
            }
            else
            {
                dest.X -= mOffset.X * scaleX;
            }
            if (FlipVertical)
            {
                dest.Y -= (SourceRect.Height - mOffset.Y) * scaleY;

                scaleY *= -1;
            }
            else
                dest.Y -= mOffset.Y * scaleY;


            texture.SetScale(scaleX, scaleY);

            var actualRotationCenter = new Vector2(rotationCenter.X + (mOffset.X * Math.Abs(scaleX)),
                                             rotationCenter.Y + (mOffset.Y * Math.Abs(scaleY)));

            texture.Draw(sourceRect, dest, actualRotationCenter);

            spriteBatch.Draw(texture,
                             dest,
                             (Rectangle?)sourceRect,
                             color,
                             rotationAngle,
                             actualRotationCenter,
                             new Vector2(scaleX, scaleY),
                             SpriteEffects.None,
                             layerDepth);
        }

        /// <summary>
        /// Converst to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "SpriteFrame: " + SourceRect.ToString() + " Anchor: " + Anchor.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
        }

        public bool FlipVertical { get; set; }
        public bool FlipHorizontal { get; set; }

        public Dictionary<string, CollisionRegion> Regions
        {
            get { return mRegions; }
        }

        /// <summary>
        /// Gets a timespan that indicates how long this frame should be displayed.
        /// </summary>
        public TimeSpan? Time { get; set; }
    }
}
