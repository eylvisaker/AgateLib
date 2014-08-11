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
using System.IO;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Resources;

namespace AgateLib.Sprites
{
	/// <summary>
	/// A class for representing an animated image.
	/// </summary>
	public class Sprite : ISprite
	{
		FrameList<SpriteFrame> mFrames = new FrameList<SpriteFrame>();
		Size mSpriteSize;

		List<Surface> mOwnedSurfaces = new List<Surface>();

		private double mTimePerFrame = 60;
		private int mCurrentFrameIndex = 0;
		private double mFrameTime = 0;
		private SpriteAnimType mAnimType = SpriteAnimType.Looping;
		private bool mPlayReverse = false;
		private bool mIsAnimating = true;
		private bool mVisible = true;

		private SurfaceState mState = new SurfaceState();


		#region --- Construction / Destruction ---
		/// <summary>
		/// Constructs a Sprite object, of the specified width and height.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public Sprite(int width, int height)
			: this(new Size(width, height))
		{
		}
		/// <summary>
		/// Constructs a Sprite object, of the specified width and height.
		/// </summary>
		/// <param name="size"></param>
		public Sprite(Size size)
		{
			mSpriteSize = size;
		}
		/// <summary>
		/// Constructs a Sprite object, of the specified width and height.
		/// The given file is loaded automatically, and frames are cut out from it
		/// of the specified size.
		/// </summary>
		/// <param name="surfaceFilename"></param>
		/// <param name="size"></param>
		public Sprite(string surfaceFilename, Size size)
			: this(new Surface(surfaceFilename), true, size)
		{
		}
		/// <summary>
		/// Constructs a Sprite object, of the specified width and height.
		/// Frames are cut out from the given surface of the specified size.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="height"></param>
		/// <param name="width"></param>
		public Sprite(Stream stream, int width, int height)
			: this(new Surface(stream), true, new Size(width, height))
		{
		}
		/// <summary>
		/// Constructs a Sprite object, of the specified width and height.
		/// A surface is loaded from the passed stream, and frames are cut out from it
		/// of the specified size.
		/// </summary>
		/// <param name="surfaceData"></param>
		/// <param name="size"></param>
		public Sprite(Stream surfaceData, Size size)
			: this(new Surface(surfaceData), true, size)
		{
		}
		/// <summary>
		/// Constructs a Sprite object, of the specified width and height.
		/// The given file is loaded automatically, and frames are cut out from it
		/// of the specified size.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public Sprite(string filename, int width, int height)
			: this(filename, new Size(width, height))
		{
		}
		/// <summary>
		/// Constructs a Sprite object, of the specified width and height.
		/// Frames are cut out from the given surface of the specified size.
		/// </summary>
		/// <param name="surface"></param>
		/// <param name="ownSurface">True to indicate that this Sprite object owns the surface, so 
		/// it is disposed when this Sprite is disposed.</param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public Sprite(Surface surface, bool ownSurface, int width, int height)
			: this(surface, ownSurface, new Size(width, height))
		{
		}
		/// <summary>
		/// Constructs a Sprite object, of the specified width and height.
		/// </summary>
		/// <param name="surface"></param>
		/// <param name="ownSurface">True to indicate that this Sprite object owns the surface, so 
		/// it is disposed when this Sprite is disposed.</param>
		/// <param name="size"></param>
		public Sprite(Surface surface, bool ownSurface, Size size)
			: this(size)
		{
			AddNewFrames(surface, ownSurface, Point.Empty, Point.Empty, size, true);
		}

		/// <summary>
		/// Constructs a sprite from a resource.
		/// </summary>
		/// <param name="resources"></param>
		/// <param name="name"></param>
		public Sprite(AgateResourceCollection resources, string name)
		{
			AgateResource generic_res = resources[name];
			SpriteResource sprite_res = generic_res as SpriteResource;

			if (sprite_res == null)
				throw new AgateResourceException("Resource " + generic_res.Name + " is not a sprite.");

			BuildSpriteFromResource(resources, resources.RootDirectory, sprite_res);
		}

		private void BuildSpriteFromResource(AgateResourceCollection resources,
			string root, SpriteResource resource)
		{
			Surface defaultSurface = null;

			if (string.IsNullOrEmpty(resource.Filename) == false)
			{
				defaultSurface = new Surface(resources.LoadSurfaceImpl(resource.Filename));
				mOwnedSurfaces.Add(defaultSurface);
			}

			if (resource.HasSize)
				mSpriteSize = resource.Size;
			else if (defaultSurface != null)
				mSpriteSize = defaultSurface.SurfaceSize;

			for (int i = 0; i < resource.ChildElements.Count; i++)
			{
				SpriteResource.SpriteSubResource child = resource.ChildElements[i];
				Surface thisSurface = defaultSurface;

				if (child is SpriteResource.SpriteFrameResource)
				{
					SpriteResource.SpriteFrameResource frame = (SpriteResource.SpriteFrameResource)child;
					if (string.IsNullOrEmpty(frame.Filename) == false)
					{
						thisSurface = new Surface(resources.LoadSurfaceImpl(frame.Filename));
						mOwnedSurfaces.Add(thisSurface);

						if (i == 0 && defaultSurface == null && resource.HasSize == false)
						{
							mSpriteSize = thisSurface.SurfaceSize;
						}
					}
					if (thisSurface == null)
					{
						throw new AgateException(string.Format(
							"The surface to create the sprite from in resource {0} was not specified.", resource.Name));
					}

					// we pass false to ownSurface here because the surface has already been added to the
					// owned surfaces list.
					AddFrame(thisSurface, false, frame.Bounds, frame.Offset);
				}
				else
				{
					var image = (SpriteResource.SpriteImageResource)child;

					DisplayLib.ImplementationBase.SurfaceImpl thisImpl = resources.LoadSurfaceImpl(image.Filename);
					if (i == 0 && defaultSurface == null && resource.HasSize == false)
					{
						mSpriteSize = thisImpl.SurfaceSize;
					}

					if (image.Grids.Count == 0)
					{
						AddFrame(new Surface(thisImpl), false,
							new Rectangle(0, 0, mSpriteSize.Width, mSpriteSize.Height),
							Point.Empty);
					}
					else
					{
						for (int j = 0; j < image.Grids.Count; j++)
						{
							AddFramesFromGrid(resources, thisImpl, image.Grids[j]);
						}
					}
				}
			}
		}

		private void AddFramesFromGrid(AgateResourceCollection resources, DisplayLib.ImplementationBase.SurfaceImpl thisImpl, SpriteResource.SpriteImageResource.Grid grid)
		{
			Point location = grid.Location;

			for (int y = 0; y < grid.Array.Height; y++)
			{
				for (int x = 0; x < grid.Array.Width; x++)
				{
					var surfImpl = thisImpl.CarveSubSurface(new Rectangle(location, grid.Size));
					AddFrame(new Surface(surfImpl), false, new Rectangle(0, 0, grid.Size.Width, grid.Size.Height), Point.Empty);

					location.X += grid.Size.Width;
				}

				location.Y += grid.Size.Height;
				location.X = grid.Location.X;
			}
		}

		/// <summary>
		/// Adds a surface as a frame to the sprite.
		/// </summary>
		/// <param name="surface"></param>
		public void AddFrame(Surface surface)
		{
			SpriteFrame frame = new SpriteFrame(surface);
			frame.SourceRect = new Rectangle(0, 0, surface.SurfaceWidth, surface.SurfaceHeight);
			frame.Anchor = Point.Empty;
			frame.SpriteSize = SpriteSize;

			mFrames.Add(frame);
		}

		/// <summary>
		/// Adds a frame to the sprite.
		/// </summary>
		/// <param name="surface">The surface from which to get the image data.</param>
		/// <param name="bounds">The source rectangle for the image data used in the sprite frame to be added.</param>
		/// <param name="ownSurface">Pass true to indicate that this sprite should own the surface and dispose of it when finished.</param>
		/// <param name="offset">The offset within the sprite to the upperleft corner of where the frame is drawn.</param>
		public void AddFrame(Surface surface, bool ownSurface, Rectangle bounds, Point offset)
		{
			SpriteFrame frame = new SpriteFrame(surface);
			frame.SourceRect = bounds;
			frame.Anchor = offset;
			frame.SpriteSize = SpriteSize;

			mFrames.Add(frame);
		}

		public void AddFrame(SpriteFrame frame)
		{
			frame.DisplaySize = DisplaySize;
			mFrames.Add(frame);
		}

		/// <summary>
		/// Makes a copy of this sprite and returns it.
		/// </summary>
		/// <returns></returns>
		public Sprite Clone()
		{
			// TODO: Update this method to cover owned surfaces.
			Sprite retval = new Sprite(mSpriteSize.Width, mSpriteSize.Height);

			retval.mTimePerFrame = mTimePerFrame;
			retval.mCurrentFrameIndex = mCurrentFrameIndex;
			retval.mFrameTime = mFrameTime;
			retval.mAnimType = mAnimType;
			retval.mPlayReverse = mPlayReverse;
			retval.mIsAnimating = mIsAnimating;

			retval.mState = mState.Clone();

			foreach (SpriteFrame frame in mFrames)
			{
				retval.mFrames.Add(frame.Clone());
			}

			return retval;
		}
		/// <summary>
		/// Disposes of unmanaged resources associated with this sprite.
		/// </summary>
		public void Dispose()
		{
			foreach (Surface surface in mOwnedSurfaces)
				surface.Dispose();
		}

		#endregion
		#region --- Working with Frames ---


		/// <summary>
		/// Adds frames from the given filename, using the size of this sprite.
		/// Frames are taken from startPoint, and with extraSpace inbetween.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="startPoint"></param>
		/// <param name="extraSpace"></param>
		/// <param name="skipBlank"></param>
		public void AddNewFrames(string filename, Point startPoint, Point extraSpace, bool skipBlank)
		{
			AddNewFrames(new Surface(filename), true, startPoint, extraSpace, SpriteSize, skipBlank);
		}
		/// <summary>
		/// Adds frames by constructing a surface from the given stream, using the size of this sprite.
		/// Frames are taken from startPoint, and with extraSpace inbetween.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="startPoint"></param>
		/// <param name="extraSpace"></param>
		/// <param name="skipBlank"></param>
		public void AddNewFrames(Stream stream, Point startPoint, Point extraSpace, bool skipBlank)
		{
			AddNewFrames(new Surface(stream), true, startPoint, extraSpace, SpriteSize, skipBlank);
		}

		/// <summary>
		/// Slices and dices the image passed into frames and adds them.
		/// Frames are taken from the surface from left to right.
		/// </summary>
		/// <param name="surface"></param>
		/// <param name="ownSurface">Pass true to indicate that this sprite should own the surface, and handle its disposal.</param>
		/// <param name="startPoint">The starting point in pixels from which to parse frames.</param>
		/// <param name="size">The size of the image to cut out for each frame</param>
		/// <param name="extraSpace">How many extra pixels to insert between each frame.</param>
		/// <param name="skipBlank">Whether or not blank frames should be automatically dropped.</param>
		public void AddNewFrames(Surface surface, bool ownSurface, Point startPoint, Point extraSpace, Size size, bool skipBlank)
		{
			Point location = startPoint;
			PixelBuffer pixels = surface.ReadPixels();

			if (ownSurface)
				mOwnedSurfaces.Add(surface);

			do
			{
				SpriteFrame currentFrame = new SpriteFrame(surface);
				Rectangle currentRect = new Rectangle(location, size);
				bool skip = false;

				currentFrame.SourceRect = currentRect;
				currentFrame.SpriteSize = SpriteSize;

				if (currentFrame.SourceRect.Right > pixels.Width) skip = true;
				if (currentFrame.SourceRect.Bottom > pixels.Height) skip = true;

				if (skipBlank && pixels.IsRegionBlank(currentFrame.SourceRect))
					skip = true;

				if (skip == false)
					mFrames.Add(currentFrame);

				location.X += size.Width + extraSpace.X;

				if (location.X + size.Width > surface.SurfaceWidth)
				{
					location.X = 0;
					location.Y += size.Height + extraSpace.Y;
				}

			} while (location.Y + size.Height <= surface.SurfaceHeight);

		}

		#endregion

		#region --- Drawing the sprite to the screen ---

		/// <summary>
		/// Draws the sprite at the specified position on screen.
		/// </summary>
		/// <param name="destX"></param>
		/// <param name="destY"></param>
		public void Draw(int destX, int destY)
		{
			Draw((float)destX, (float)destY);
		}
		/// <summary>
		/// Draws the sprite at the specified position on screen.
		/// </summary>
		/// <param name="destX"></param>
		/// <param name="destY"></param>
		public void Draw(float destX, float destY)
		{
			DrawImpl(destX, destY);
		}

		private void DrawImpl(float destX, float destY)
		{
			if (mFrames.Count == 0)
				return;
			if (mVisible == false)
				return;

			SpriteFrame currentFrame = CurrentFrame;
			Surface surf = currentFrame.Surface;

			surf.InterpolationHint = InterpolationHint;
			currentFrame.DisplaySize = DisplaySize;

			PointF alignment = Origin.CalcF(DisplayAlignment, DisplaySize);
			PointF rotation = new PointF();// Origin.CalcF(RotationCenter, DisplaySize);

			surf.Alpha = Alpha;
			surf.DisplayAlignment = OriginAlignment.TopLeft;
			surf.RotationAngle = RotationAngle;
			surf.Color = Color;

			currentFrame.FlipHorizontal = FlipHorizontal;
			currentFrame.FlipVertical = FlipVertical;

			currentFrame.Draw(destX - alignment.X, destY - alignment.Y,
								  rotation.X, rotation.Y);
		}
		/// <summary>
		/// Draws the sprite at the specified position on screen.
		/// </summary>
		/// <param name="destPt"></param>
		public void Draw(Point destPt)
		{
			Draw((float)destPt.X, (float)destPt.Y);
		}
		/// <summary>
		/// Draws the sprite at the specified position on screen.
		/// </summary>
		/// <param name="destPt"></param>
		public void Draw(Vector2 destPt)
		{
			Draw((float)destPt.X, (float)destPt.Y);
		}
		/// <summary>
		/// Draws the sprite at the specified position on screen.
		/// </summary>
		/// <param name="destPt"></param>
		public void Draw(PointF destPt)
		{
			Draw(destPt.X, destPt.Y);
		}


		#endregion
		#region --- Queueing rects to draw to the screen ---

		#endregion

		#region --- Sprite properties ---

		/// <summary>
		/// Gets width of the sprite.
		/// </summary>
		public int SpriteWidth
		{
			get { return mSpriteSize.Width; }
		}
		/// <summary>
		/// Gets height of the sprite.
		/// </summary>
		public int SpriteHeight
		{
			get { return mSpriteSize.Height; }
		}
		/// <summary>
		/// Gets the size of the sprite.
		/// </summary>
		public Size SpriteSize
		{
			get
			{
				return mSpriteSize;
			}
		}

		int ISurface.SurfaceWidth
		{
			get { return SpriteWidth; }
		}
		int ISurface.SurfaceHeight
		{
			get { return SpriteHeight; }
		}
		Size ISurface.SurfaceSize
		{
			get { return SpriteSize; }
		}
		/// <summary>
		/// Gets or sets the amount the width is scaled.
		/// </summary>
		public double ScaleWidth
		{
			get { return mState.ScaleWidth; }
			set { mState.ScaleWidth = value; }
		}
		/// <summary>
		/// Gets or sets the amount the height is scaled.
		/// </summary>
		public double ScaleHeight
		{
			get { return mState.ScaleHeight; }
			set { mState.ScaleHeight = value; }
		}
		/// <summary>
		/// Gets the width of the sprite when displayed.
		/// </summary>
		public int DisplayWidth
		{
			get { return (int)(ScaleWidth * SpriteWidth); }
			set { ScaleWidth = value / (double)SpriteWidth; }
		}
		/// <summary>
		/// Gets the height of the sprite when displayed.
		/// </summary>
		public int DisplayHeight
		{
			get { return (int)(ScaleHeight * SpriteHeight); }
			set { ScaleHeight = value / (double)SpriteHeight; }
		}
		/// <summary>
		/// Gets or sets the size of the sprite when displayed.
		/// </summary>
		public Size DisplaySize
		{
			get
			{
				Size sz = SpriteSize;
				sz.Width = (int)(sz.Width * ScaleWidth);
				sz.Height = (int)(sz.Height * ScaleHeight);

				return sz;
			}
			set
			{
				DisplayWidth = value.Width;
				DisplayHeight = value.Height;
			}
		}

		/// <summary>
		/// Gets or sets transparency value.
		/// 0.0 is completely transparent
		/// 1.0 is completely opaque.
		/// </summary>
		public double Alpha
		{
			get { return mState.Alpha; }
			set
			{
				mState.Alpha = value;
			}
		}
		/// <summary>
		/// Gets or sets the rotation angle in radians.
		/// </summary>
		public double RotationAngle
		{
			get { return mState.RotationAngle; }
			set { mState.RotationAngle = value; }
		}
		/// <summary>
		/// Gets or sets the rotation angle in degrees.
		/// </summary>
		public double RotationAngleDegrees
		{
			get { return mState.RotationAngleDegrees; }
			set { mState.RotationAngleDegrees = value; }
		}
		/// <summary>
		/// Gets or sets the center of rotation.
		/// </summary>
		public OriginAlignment RotationCenter
		{
			get { return mState.RotationCenter; }
			set { mState.RotationCenter = value; }
		}
		/// <summary>
		/// Gets or sets the interpretation of the position.
		/// </summary>
		public OriginAlignment DisplayAlignment
		{
			get { return mState.DisplayAlignment; }
			set { mState.DisplayAlignment = value; }
		}
		/// <summary>
		/// Sets the scale of the sprite.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void SetScale(double x, double y)
		{
			mState.ScaleWidth = x;
			mState.ScaleHeight = y;
		}
		/// <summary>
		/// Gets the scale of the sprite.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void GetScale(out double x, out double y)
		{
			x = mState.ScaleWidth;
			y = mState.ScaleHeight;
		}

		/// <summary>
		/// Gets or sets the color of the sprite.
		/// </summary>
		public Color Color
		{
			get { return mState.Color; }
			set { mState.Color = value; }
		}
		/// <summary>
		/// Gets or sets the color gradient on the sprite.
		/// </summary>
		public Gradient ColorGradient
		{
			get { return mState.ColorGradient; }
			set { mState.ColorGradient = value; }
		}
		/// <summary>
		/// Increments the rotation angle by the specified number of radians.
		/// </summary>
		/// <param name="radians"></param>
		[Obsolete("Use RotationAngle += *value*")]
		public void IncrementRotationAngle(double radians)
		{
			mState.RotationAngle += radians;
		}
		/// <summary>
		/// Increments the rotation angle by the specified number of degrees.
		/// </summary>
		/// <param name="degrees"></param>
		public void IncrementRotationAngleDegrees(double degrees)
		{
			mState.IncrementRotationAngleDegrees(degrees);
		}

		/// <summary>
		/// Gets or sets whether or not the sprite should be drawn when Draw is called.
		/// </summary>
		public bool Visible
		{
			get { return mVisible; }
			set { mVisible = value; }
		}

		public Point Anchor { get; set; }
		public Rectangle InnerRect { get; set; }

		#endregion

		#region --- Animation Properties and Methods ---

		/// <summary>
		/// Updates the animation of the sprite, using the DeltaTime given
		/// by the Display object.
		/// </summary>
		public void Update()
		{
			Update(Display.DeltaTime);
		}
		/// <summary>
		/// Updates the animation of the sprite, using the given frame time.
		/// </summary>
		/// <param name="time_ms">The amount of time to consider passed, in milliseconds.</param>
		public void Update(double time_ms)
		{
			if (IsAnimating == false)
				return;

			mFrameTime += time_ms;

			while (mFrameTime >= mTimePerFrame)
			{
				mFrameTime -= mTimePerFrame;

				AdvanceFrame();
			}
		}
		/// <summary>
		/// Shows the next frame in the sequence.  This pays attention
		/// to whether the animation is playing forwards or reverse.
		/// </summary>
		public void AdvanceFrame()
		{
			int newFrameIndex = CurrentFrameIndex;

			if (PlayReverse)
				newFrameIndex--;
			else
				newFrameIndex++;

			switch (AnimationType)
			{
				case SpriteAnimType.Looping:
					while (newFrameIndex < 0)
						newFrameIndex += mFrames.Count;

					newFrameIndex = newFrameIndex % mFrames.Count;

					break;

				case SpriteAnimType.Once:
					if (PlayReverse && newFrameIndex == -1)
					{
						newFrameIndex = mFrames.Count - 1;
						IsAnimating = false;
					}
					else if (PlayReverse == false && newFrameIndex == mFrames.Count)
					{
						newFrameIndex = 0;
						IsAnimating = false;
					}

					break;

				case SpriteAnimType.Twice:
					if (PlayReverse && newFrameIndex == -1)
					{
						newFrameIndex = mFrames.Count - 1;
						mAnimType = SpriteAnimType.Once;
					}
					else if (PlayReverse == false && newFrameIndex == mFrames.Count)
					{
						newFrameIndex = 0;
						mAnimType = SpriteAnimType.Once;
					}

					break;

				case SpriteAnimType.OnceHoldLast:
					if (PlayReverse && newFrameIndex == -1)
					{
						newFrameIndex = 0;
						IsAnimating = false;
					}
					else if (PlayReverse == false && newFrameIndex == mFrames.Count)
					{
						newFrameIndex = mFrames.Count - 1;
						IsAnimating = false;
					}

					break;

				case SpriteAnimType.PingPong:
					/*
					// this makes it so that you can have a 10 frame pingpong animation, 
					// set it to frame 12, and it will actually show frame 8, because of 
					// the reflection at the end.
					newFrameIndex %= (mFrames.Count * 2);

					if (newFrameIndex >= mFrames.Count)
						newFrameIndex = 2 * mFrames.Count - 1 - newFrameIndex;
					
					 * // this is old stuff need to figure out how/whether to include it
					 * // in new implementation
					 * */

					if (Frames.Count <= 1)
						break;

					// check for the ping-ponging.
					if (PlayReverse && newFrameIndex == -1)
					{
						PlayReverse = false;
						newFrameIndex = 1;
					}
					else if (PlayReverse == false && newFrameIndex == mFrames.Count)
					{
						PlayReverse = true;
						newFrameIndex = mFrames.Count - 2;
					}

					break;

				case SpriteAnimType.OnceDisappear:
					if (PlayReverse && newFrameIndex == -1)
					{
						newFrameIndex = 0;
						mVisible = false;
					}
					else if (PlayReverse == false && newFrameIndex == mFrames.Count)
					{
						newFrameIndex = mFrames.Count - 1;
						mVisible = false;
					}

					break;

				default:
					throw new AgateException("Error: AnimationType not valid!");
			}

			CurrentFrameIndex = newFrameIndex;

			if (mCurrentFrameIndex < 0 || mCurrentFrameIndex >= mFrames.Count)
				throw new AgateException("Error: Frame Index is in the wrong place!");
		}


		/// <summary>
		/// The amount of time each frame should display, in milliseconds.
		/// </summary>
		public double TimePerFrame
		{
			get { return mTimePerFrame; }
			set
			{
				if (value > 0)
					mTimePerFrame = value;
			}
		}
		/// <summary>
		/// The index of the current frame.
		/// </summary>
		public int CurrentFrameIndex
		{
			get { return mCurrentFrameIndex; }
			set
			{
				if (mFrames.Count <= 1)
				{
					mCurrentFrameIndex = 0;
					return;
				}

				if (value < 0 || value >= mFrames.Count)
					throw new ArgumentOutOfRangeException("CurrentFrameIndex must be between 0 and mFrames.Count - 1");

				mCurrentFrameIndex = value;

			}
		}
		/// <summary>
		/// Gets the currently displaying frame.
		/// </summary>
		public SpriteFrame CurrentFrame
		{
			get { return mFrames[CurrentFrameIndex]; }
		}
		ISpriteFrame ISprite.CurrentFrame
		{
			get { return CurrentFrame; }
		}

		/// <summary>
		/// Gets or sets a flag which indicates whether or not this animation plays in 
		/// reverse instead.
		/// </summary>
		public bool PlayReverse
		{
			get { return mPlayReverse; }
			set
			{
				bool doEvent = false;
				if (value != mPlayReverse)
					doEvent = true;

				mPlayReverse = value;

				if (doEvent && PlayDirectionChanged != null)
				{
					PlayDirectionChanged(this);
				}
			}
		}
		/// <summary>
		/// Gets or sets an enum value indicating what type of animation is happening.
		/// Looping - The animation will play from beginning to end and then restart.
		/// PingPong - The animation will play from beginning to end and then from end to beginning (continuously).
		/// Once - The animation plays once, and then shows its first frame.
		/// OnceHoldLast - The animation plays once, and leaves the last frame on.
		/// </summary>
		public SpriteAnimType AnimationType
		{
			get { return mAnimType; }
			set { mAnimType = value; }
		}
		/// <summary>
		/// Gets or sets a flag which indicates:
		/// True if the animation is running.
		/// False if a single frame will be shown indefinitely.
		/// </summary>
		public bool IsAnimating
		{
			get { return mIsAnimating; }
			set
			{
				bool doEvent = false;

				if (value != mIsAnimating)
					doEvent = true;

				mIsAnimating = value;

				if (value &&
					(AnimationType == SpriteAnimType.Once || AnimationType == SpriteAnimType.OnceDisappear ||
					 AnimationType == SpriteAnimType.OnceHoldLast))
				{

					if (this.PlayReverse && mCurrentFrameIndex == 0)
						mCurrentFrameIndex = mFrames.Count - 1;
					else if (mCurrentFrameIndex == mFrames.Count - 1)
						mCurrentFrameIndex = 0;

				}

				if (doEvent)
				{
					if (mIsAnimating == true && AnimationStarted != null)
						AnimationStarted(this);
					else if (mIsAnimating == false && AnimationStopped != null)
						AnimationStopped(this);
				}
			}
		}

		/// <summary>
		/// Gets the list of frames in this sprite.
		/// </summary>
		public FrameList<SpriteFrame> Frames
		{
			get { return mFrames; }
		}

		/// <summary>
		/// Restarts the animation.
		/// </summary>
		public void StartAnimation()
		{
			if (PlayReverse)
				CurrentFrameIndex = mFrames.Count - 1;
			else
				CurrentFrameIndex = 0;

			mIsAnimating = true;
			mVisible = true;
		}

		#endregion

		#region --- Events ---

		/// <summary>
		/// Event which is raised when the animation is stopped.
		/// </summary>
		public event SpriteEventHandler AnimationStopped;
		/// <summary>
		/// Event which is raised when the animation is started.
		/// </summary>
		public event SpriteEventHandler AnimationStarted;
		/// <summary>
		/// Event which is raised when the play direction is changed, as
		/// in the PingPong type.
		/// </summary>
		public event SpriteEventHandler PlayDirectionChanged;

		#endregion

		/// <summary>
		/// Gets or sets the SurfaceState used when drawing.
		/// </summary>
		public SurfaceState State
		{
			get { return mState; }
			set { mState = value; }
		}
		/// <summary>
		/// Draws the sprite with the specified state object.  (Not currently implemented.)
		/// </summary>
		/// <param name="state"></param>
		public void Draw(SurfaceState state)
		{
			throw new NotImplementedException();
		}

		public InterpolationMode InterpolationHint { get; set; }
		public bool FlipHorizontal { get; set; }
		public bool FlipVertical { get; set; }

		/// <summary>
		/// Returns the total animation time in milliseconds.
		/// </summary>
		public double TotalAnimTime
		{
			get { return TimePerFrame * Frames.Count; }
		}
	}
}
