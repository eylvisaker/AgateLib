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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
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
