﻿using System;
using System.Collections.Generic;
using System.Text;

using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Sprites
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

		Surface mSurface;

		Rectangle mSrcRect;
		Size mDisplaySize;
		Size mSpriteSize;

		internal SpriteFrame(Surface surface)
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

		#region ISpriteFrame Members

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

			mSurface.SetScale(scaleX, scaleY);

			mSurface.Draw(dest_x + (mOffset.X * scaleX),
						  dest_y + (mOffset.Y * scaleY),
						  mSrcRect,
						  rotationCenterX - (mOffset.X * scaleX),
						  rotationCenterY - (mOffset.Y * scaleY));
		}

		/// <summary>
		/// Converst to a string.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "SpriteFrame: " + SourceRect.ToString() + " Offset: " + Offset.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		public Surface Surface
		{
			get { return mSurface; }
		}

		#endregion
	}

}
