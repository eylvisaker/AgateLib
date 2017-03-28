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
using System.Collections.Generic;
using System.Text;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib.Sprites
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
	public class SpriteFrame : ISpriteFrame
	{
		Point mOffset = new Point(0, 0);
		//bool mIsBlank = true;

		ISurface mSurface;

		Rectangle mSrcRect;
		Size mDisplaySize;
		Size mSpriteSize;
		Dictionary<string, CollisionRegion> mRegions = new Dictionary<string, CollisionRegion>();

		internal SpriteFrame(ISurface surface)
		{
			mSurface = surface;
		}

		/// <summary>
		/// Copies this object.
		/// 
		/// Actually, this just returns this
		/// object, since there are no unmanaged resources to deal with.  
		/// Be sure to Dispose the result when finished with it.
		/// </summary>
		/// <returns></returns>
		public SpriteFrame Clone()
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
		public Point Anchor
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
		/// <param name="dest_x"></param>
		/// <param name="dest_y"></param>
		/// <param name="rotationCenterX"></param>
		/// <param name="rotationCenterY"></param>
		public void Draw(float dest_x, float dest_y, float rotationCenterX, float rotationCenterY)
		{
			// calculate scaling.
			float scaleX = mDisplaySize.Width / (float)SpriteSize.Width;
			float scaleY = mDisplaySize.Height / (float)SpriteSize.Height;

			PointF dest = new PointF(dest_x, dest_y);

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

			
			mSurface.SetScale(scaleX, scaleY);

			var rotationCenter =
				new PointF(rotationCenterX + (mOffset.X * Math.Abs(scaleX)),
						  rotationCenterY + (mOffset.Y * Math.Abs(scaleY)));

			mSurface.Draw(mSrcRect, dest, rotationCenter);
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
		public ISurface Surface
		{
			get { return mSurface; }
		}

		internal bool FlipVertical { get; set; }
		internal bool FlipHorizontal { get; set; }

		public Dictionary<string, CollisionRegion> Regions
		{
			get { return mRegions; }
		}
	}
}
