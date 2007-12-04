using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using ERY.AgateLib.Geometry;
using ERY.AgateLib.Resources;

namespace ERY.AgateLib
{
    public class Sprite2 : ISurface
    {
        Surface mSurface;
        bool mOwnSurface;
        List<SpriteFrame2> mFrames = new List<SpriteFrame2>();
        Size mSpriteSize;

        private double mTimePerFrame = 60;
        private int mCurrentFrameIndex = 0;
        private double mFrameTime = 0;
        private AnimType mAnimType = AnimType.Looping;
        private bool mPlayReverse = false;
        private bool mIsAnimating = true;
        private bool mVisible = true;

        private double mScaleX = 1.0;
        private double mScaleY = 1.0;

        private OriginAlignment mAlignment = OriginAlignment.TopLeft;
        private double mRotation = 0;
        private OriginAlignment mRotationSpot = OriginAlignment.Center;
        private Gradient mGradient = new Gradient(Color.White);

        /// <summary>
        /// Enum indicating the different types of automatic animation that
        /// take place.
        /// </summary>
        public enum AnimType
        {
            /// <summary>
            /// Specifies that the sprite animation should go from
            /// frame 0 to the end, and start back at frame 0.
            /// </summary>
            Looping,
            /// <summary>
            /// Specifies that the sprite animation should go from
            /// frame 0 to the end, and then go back down to frame 0.
            /// </summary>
            PingPong,
            /// <summary>
            /// Specifies that the sprite animation should go from
            /// frame 0 to the end and then back to frame 0, stopping there.
            /// </summary>
            Once,
            /// <summary>
            /// Specifies that the sprite animation should go from
            /// frame 0 to the end and stop there.
            /// </summary>
            OnceHoldLast,
            /// <summary>
            /// Specifies that the sprite animation should go from
            /// frame 0 to the end, and then disappear.  The Visible
            /// property of the Sprite object is set to false once
            /// the animation is complete.
            /// </summary>
            OnceDisappear,

            /// <summary>
            /// Specifies that the sprite animation should go twice
            /// </summary>
            Twice,
        }

        #region --- Construction / Destruction ---

        /// <summary>
        /// Constructs a Sprite object, of the specified width and height.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Sprite2(int width, int height)
            : this(new Size(width, height))
        {
        }
        /// <summary>
        /// Constructs a Sprite object, of the specified width and height.
        /// </summary>
        /// <param name="size"></param>
        public Sprite2(Size size)
        {
            mSpriteSize = size;
        }
        /// <summary>
        /// Constructs a Sprite object, of the specified width and height.
        /// The given file is loaded automatically, and frames are cut out from it
        /// of the specified size.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="size"></param>
        public Sprite2(string filename, Size size)
            : this(size)
        {
            mSurface = new Surface(filename);
            mOwnSurface = true;

            AddFrames();
        }
        /// <summary>
        /// Constructs a Sprite object, of the specified width and height.
        /// The given file is loaded automatically, and frames are cut out from it
        /// of the specified size.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Sprite2(string filename, int width, int height)
            : this(filename, new Size(width, height))
        {
        }
        /// <summary>
        /// Constructs a Sprite object, of the specified width and height.
        /// Frames are cut out from the given surface of the specified size.
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="ownSurface">True to indicate that this Sprite2 object owns the surface, so 
        /// it is disposed when this Sprite is disposed.</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Sprite2(Surface surface, bool ownSurface, int width, int height)
            : this(surface, ownSurface, new Size(width, height))
        {
        }
        /// <summary>
        /// Constructs a Sprite object, of the specified width and height.
        /// Frames are cut out from the given surface of the specified size.
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="ownSurface">True to indicate that this Sprite2 object owns the surface, so 
        /// it is disposed when this Sprite is disposed.</param>
        /// <param name="size"></param>
        public Sprite2(Surface surface, bool ownSurface, Size size)
            : this(size)
        {
            mSurface = surface;
            mOwnSurface = ownSurface;

            AddFrames();
        }
        /// <summary>
        /// Constructs a Sprite object, of the specified width and height.
        /// Frames are cut out from the given surface of the specified size.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="size"></param>
        public Sprite2(Stream stream, Size size)
            : this(new Surface(stream), true, size)
        {
        }
        /// <summary>
        /// Constructs a Sprite object, loading it from the file specified
        /// in the given ResourceManager object.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="resources"></param>
        public Sprite2(string name, ResourceManager resources)
        {
            throw new NotImplementedException();
            /*
            Resource rs = resources.GetResource("sprite", name);

            IList<Resource> images = rs.GetSubElements("image");

            for (int i = 0; i < images.Count; i++)
            {
                Resource surfResource = images[i];
                Surface surf = new Surface(surfResource.GetStringAttribute("file"));
                surf.ShouldBePacked = false;
                Resource grid = surfResource.GetFirstSubElement("grid");

                if (grid != null)
                {
                    Point pos = grid.GetPointAttribute("pos");
                    Size size = grid.GetSizeAttribute("size");
                    Size array = grid.GetSizeAttribute("array");

                    AddFrames(surf, pos, new Point(0, 0), size, array);

                    if (i == 0)
                        mSpriteSize = size;
                }
                else
                {
                    if (i == 0)
                    {
                        mSpriteSize = surf.SurfaceSize;
                    }

                    AddFrames(surf);
                }
            }
             * */
        }
        /// <summary>
        /// Makes a copy of this sprite and returns it.
        /// </summary>
        /// <returns></returns>
        public Sprite2 Clone()
        {
            Sprite2 retval = new Sprite2(mSpriteSize.Width, mSpriteSize.Height);

            if (mOwnSurface)
            {
                retval.mSurface = new Surface(mSurface.ReadPixels());
                retval.mOwnSurface = true;
            }
            else
            {
                retval.mSurface = mSurface;
                retval.mOwnSurface = false;
            }

            retval.mTimePerFrame = mTimePerFrame;
            retval.mCurrentFrameIndex = mCurrentFrameIndex;
            retval.mFrameTime = mFrameTime;
            retval.mAnimType = mAnimType;
            retval.mPlayReverse = mPlayReverse;
            retval.mIsAnimating = mIsAnimating;

            retval.mScaleX = mScaleX;
            retval.mScaleY = mScaleY;

            retval.mAlignment = mAlignment;
            retval.mRotation = mRotation;
            retval.mRotationSpot = mRotationSpot;
            retval.mGradient = mGradient;

            foreach (SpriteFrame2 f in mFrames)
            {
                retval.mFrames.Add(f.Clone());
            }

            return retval;
        }
        /// <summary>
        /// Disposes of unmanaged resources associated with this sprite.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (mOwnSurface)
                    mSurface.Dispose();

                GC.SuppressFinalize(this);
            }
        }
        /// <summary>
        /// Destroyes this sprite object.
        /// </summary>
        ~Sprite2()
        {
            Dispose(false);
        }

        #endregion
        #region --- Working with Frames ---


        /// <summary>
        /// Adds frames from the given surface, using the size of this sprite.
        /// </summary>
        /// <param name="surface"></param>
        public void AddFrames()
        {
            AddFrames(Point.Empty, Point.Empty, SpriteSize, true);
        }

        /// <summary>
        /// Slices and dices the image passed into frames and adds them.
        /// Automatically skips blank ones.
        /// This can only called if there is no surface object backing the sprite.
        /// </summary>
        /// <param name="filename"></param>
        public void AddFrames(string filename)
        {
            AddFrames(filename, Point.Empty, Point.Empty, true);
        }

        /// <summary>
        /// Adds frames from the given filename, using the size of this sprite.
        /// Frames are taken from startPoint, and with extraSpace inbetween.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="startPoint"></param>
        /// <param name="extraSpace"></param>
        /// <param name="skipBlank"></param>
        public void AddFrames(string filename, Point startPoint, Point extraSpace, bool skipBlank)
        {
            if (mSurface != null)
                throw new InvalidOperationException();

            mSurface = new Surface(filename);
            mOwnSurface = true;

            AddFrames(startPoint, extraSpace, SpriteSize, skipBlank);
        }
        
        /// <summary>
        /// Slices and dices the image passed into frames and adds them.
        /// Frames are taken from the surface from left to right.
        /// </summary>
        /// <param name="startPoint">The starting point in pixels from which to parse frames.</param>
        /// <param name="size">The size of the image to cut out for each frame</param>
        /// <param name="extraSpace">How many extra pixels to insert between each frame.</param>
        /// <param name="skipBlank">Whether or not blank frames should be automatically dropped.</param>
        public void AddFrames(Point startPoint, Point extraSpace, Size size, bool skipBlank)
        {
            Point location = startPoint;

            PixelBuffer pixels = mSurface.ReadPixels();

            do
            {
                SpriteFrame2 currentFrame = new SpriteFrame2();
                Rectangle currentRect = new Rectangle(location, size);
                bool skip = false;

                currentFrame.SrcRect = currentRect;

                if (currentFrame.SrcRect.Right > pixels.Width) skip = true;
                if (currentFrame.SrcRect.Bottom > pixels.Height) skip = true;

                if (!skip && skipBlank && IsFrameBlank(pixels, currentFrame)) skip = true;

                if (skip == false)
                    mFrames.Add(currentFrame);

                location.X += size.Width;

                if (location.X + size.Width > mSurface.SurfaceWidth)
                {
                    location.X = 0;
                    location.Y += size.Height;
                }

            } while (location.Y < mSurface.SurfaceHeight);

        }

        /// <summary>
        /// Compresses the frames of the sprite eliminating blank space so that sprites can be
        /// packed on a surface.
        /// </summary>
        public void CompressFrames()
        {
            PixelBuffer buffer = mSurface.ReadPixels();

            foreach (SpriteFrame2 frame in mFrames)
            {
                CompressFrame(buffer, frame);
            }
        }

        private void CompressFrame(PixelBuffer buffer, SpriteFrame2 frame)
        {
            Rectangle newRect = frame.SrcRect;

            while (buffer.IsRowBlank(newRect.Y, newRect.X, newRect.Width))
            {
                newRect.Y++;
                newRect.Height--;

                if (newRect.Height == 0)
                    goto done;
            }
            while (buffer.IsRowBlank(newRect.Bottom - 1, newRect.X, newRect.Width))
            {
                newRect.Height -= 1;
            }
            while (buffer.IsColumnBlank(newRect.X, newRect.Y, newRect.Height))
            {
                newRect.X++;
                newRect.Width--;
            }
            while (buffer.IsColumnBlank(newRect.Right - 1, newRect.Y, newRect.Height))
            {
                newRect.Width--;
            }

            // add a border of blanks
            if (newRect.X > frame.SrcRect.X)
            {
                newRect.X--;
                newRect.Width++;
            }
            if (newRect.Width < frame.SrcRect.Width)
                newRect.Width++;

            if (newRect.Y > frame.SrcRect.Y)
            {
                newRect.Y--;
                newRect.Height++;
            }
            if (newRect.Height < frame.SrcRect.Height)
                newRect.Height++;

        done:
            Point offset = new Point(
                newRect.X - frame.SrcRect.Location.X,
                newRect.Y - frame.SrcRect.Location.Y);

            frame.SrcRect = newRect;
            frame.Offset = offset;
        }

        /*
        /// <summary>
        /// Slices and dices the image passed into frames and adds them.
        /// Frames are taken from the surface from left to right.
        /// </summary>
        /// <param name="filename">Filename of the image to load.</param>
        /// <param name="startPoint">The starting point in pixels from which to parse frames.</param>
        /// <param name="size">The size of the image to cut out for each frame</param>
        /// <param name="extraSpace">How many extra pixels to insert between each frame.</param>
        /// <param name="array">How many frames to cut out.  eg. If array = {4, 1}, four frames will be
        /// taken from left to right.</param>
        public void AddFrames(string filename, Point startPoint, Point extraSpace, Size size, Size array)
        {
            using (Surface surf = new Surface(filename))
            {
                AddFrames(surf, startPoint, extraSpace, size, array);
            }
        }
        */
        /*
        /// <summary>
        /// Slices and dices the image passed into frames and adds them.
        /// Frames are taken from the surface from left to right.
        /// </summary>
        /// <param name="surface">The surface to use to split up into the sprite frames.</param>
        /// <param name="startPoint">The starting point in pixels from which to parse frames.</param>
        /// <param name="size">The size of the image to cut out for each frame</param>
        /// <param name="extraSpace">How many extra pixels to insert between each frame.</param>
        /// <param name="array">How many frames to cut out.  eg. If array = {4, 1}, four frames will be
        /// taken from left to right.</param>
        public void AddFrames(Surface surface, Point startPoint, Point extraSpace, Size size, Size array)
        {
            Point location = startPoint;

            int array_x = 0;
            int array_y = 0;

            do
            {
                SpriteFrame2 currentFrame = new SpriteFrame2();
                Rectangle currentRect = new Rectangle(location, size);
                
                currentFrame.SetFrame(surface, currentRect);
                mFrames.Add(currentFrame);

                location.X += size.Width;
                array_x++;


                if (location.X + size.Width > surface.SurfaceWidth || array_x >= array.Width)
                {
                    location.X = startPoint.X;
                    location.Y += size.Height;

                    array_x = 0;
                    array_y++;
                }
            
            } while (location.Y < surface.SurfaceHeight && array_y < array.Height);

        }
        */

        private bool IsFrameBlank(SpriteFrame2 currentFrame)
        {
            return IsFrameBlank(mSurface.ReadPixels(), currentFrame);
        }

        private bool IsFrameBlank(PixelBuffer pixels, SpriteFrame2 currentFrame)
        {
            for (int y = 0; y < currentFrame.SrcRect.Height; y++)
            {
                for (int x = 0; x < currentFrame.SrcRect.Width; x++)
                {
                    Point pt = new Point(x + currentFrame.SrcRect.X, y + currentFrame.SrcRect.Y);

                    if (pixels.GetPixel(pt.X, pt.Y).A > Display.AlphaThreshold)
                        return false;
                }
            }

            return true;
        }


        #endregion

        #region --- Drawing the sprite to the screen ---

        /// <summary>
        /// Draw the sprite to the given destination rectangle.
        /// Overrides scaling settings.
        /// </summary>
        /// <param name="destRect"></param>
        public void Draw(Rectangle destRect)
        {
            if (mFrames.Count == 0)
                return;

            SpriteFrame2 current = CurrentFrame;
            Surface surf = mSurface;

            current.DisplaySize = destRect.Size;

            PointF alignment = Origin.CalcF(DisplayAlignment, DisplaySize);
            PointF rotation = Origin.CalcF(RotationCenter, DisplaySize);

            surf.Alpha = Alpha;
            surf.DisplayAlignment = DisplayAlignment;
            surf.RotationAngle = RotationAngle;
            surf.RotationCenter = RotationCenter;
            surf.Color = Color;

            current.Draw(mSurface, destRect.X - alignment.X, destRect.Y - alignment.Y, 
                                    rotation.X, rotation.Y);
        }
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
            if (mFrames.Count == 0)
                return;
            if (mVisible == false)
                return;

            SpriteFrame2 current = CurrentFrame;
            Surface surf = mSurface;

            current.DisplaySize = DisplaySize;

            PointF alignment = Origin.CalcF(DisplayAlignment, DisplaySize);
            PointF rotation = Origin.CalcF(RotationCenter, DisplaySize);

            surf.Alpha = Alpha;
            surf.DisplayAlignment = OriginAlignment.TopLeft;
            surf.RotationAngle = RotationAngle;
            surf.Color = Color;

            current.Draw(mSurface, destX - alignment.X, destY - alignment.Y, 
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

        /// <summary>
        /// Draws the sprite at all the specified positions on screen.
        /// </summary>
        /// <param name="dest_pts"></param>
        public void DrawPoints(Point[] dest_pts)
        {
            for (int i = 0; i < dest_pts.Length; i++)
                Draw(dest_pts[i]);
        }
        /// <summary>
        /// Draws the sprite at the origin.
        /// </summary>
        public void Draw() { Draw(0, 0); }

        /// <summary>
        /// Draws the sprite at the specified rectangles.
        /// </summary>
        /// <param name="dest_rects"></param>
        public void DrawRects(Rectangle[] dest_rects)
        {
            foreach (Rectangle r in dest_rects)
                Draw(r);
        }

        #endregion
        #region --- Queueing rects to draw to the screen ---

        private System.Collections.Generic.Queue<Rectangle> srcRectList;
        private System.Collections.Generic.Queue<Rectangle> destRectList;
        /// <summary>
        /// 
        /// </summary>
        [Obsolete()]
        public virtual void BeginQueueRects()
        {
            srcRectList = new Queue<Rectangle>(100);
            destRectList = new Queue<Rectangle>(100);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guessCount"></param>
        [Obsolete()]
        public virtual void BeginQueueRects(int guessCount)
        {
            srcRectList = new Queue<Rectangle>(guessCount);
            destRectList = new Queue<Rectangle>(guessCount);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="src_rect"></param>
        /// <param name="dest_rect"></param>
        [Obsolete()]
        public virtual void QueueRect(Rectangle src_rect, Rectangle dest_rect)
        {
            srcRectList.Enqueue(src_rect);
            destRectList.Enqueue(dest_rect);
        }
        /// <summary>
        /// 
        /// </summary>
        [Obsolete()]
        public virtual void EndQueueRects()
        {
            DrawRects(destRectList.ToArray());
        }

        #endregion
        #region --- Sprite properties ---

        /// <summary>
        /// Returns true if this Sprite2 owns the surface backing the image data.
        /// </summary>
        public bool OwnSurface
        {
            get { return mOwnSurface; }
        }
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
            get { return mScaleX; }
            set { mScaleX = value; }
        }
        /// <summary>
        /// Gets or sets the amount the height is scaled.
        /// </summary>
        public double ScaleHeight
        {
            get { return mScaleY; }
            set { mScaleY = value; }
        }
        /// <summary>
        /// Gets the width of the sprite when displayed.
        /// </summary>
        public int DisplayWidth
        {
            get { return (int)(mScaleX * SpriteWidth); }
            set { mScaleX = value / (double)SpriteWidth; }
        }
        /// <summary>
        /// Gets the height of the sprite when displayed.
        /// </summary>
        public int DisplayHeight
        {
            get { return (int)(mScaleY * SpriteHeight); }
            set { mScaleY = value / (double)SpriteHeight; }
        }
        /// <summary>
        /// Gets or sets the size of the sprite when displayed.
        /// </summary>
        public Size DisplaySize
        {
            get
            {
                Size sz = SpriteSize;
                sz.Width = (int)(sz.Width * mScaleX);
                sz.Height = (int)(sz.Height * mScaleY);

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
            get { return Color.A / 255.0; }
            set
            {
                mGradient.SetAlpha(value);
            }
        }
        /// <summary>
        /// Gets or sets the rotation angle in radians.
        /// </summary>
        public double RotationAngle
        {
            get { return mRotation; }
            set { mRotation = value % (2 * Math.PI); }
        }
        /// <summary>
        /// Gets or sets the rotation angle in degrees.
        /// </summary>
        public double RotationAngleDegrees
        {
            get { return RotationAngle * 180.0 / Math.PI; }
            set { RotationAngle = value * Math.PI / 180.0; }
        }
        /// <summary>
        /// Gets or sets the center of rotation.
        /// </summary>
        public OriginAlignment RotationCenter
        {
            get { return mRotationSpot; }
            set { mRotationSpot = value; }
        }
        /// <summary>
        /// Gets or sets the interpretation of the position.
        /// </summary>
        public OriginAlignment DisplayAlignment
        {
            get { return mAlignment; }
            set { mAlignment = value; }
        }
        /// <summary>
        /// Sets the scale of the sprite.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetScale(double x, double y)
        {
            mScaleX = x;
            mScaleY = y;
        }
        /// <summary>
        /// Gets the scale of the sprite.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void GetScale(out double x, out double y)
        {
            x = mScaleX;
            y = mScaleY;
        }

        /// <summary>
        /// Gets or sets the color of the sprite.
        /// </summary>
        public Color Color
        {
            get { return mGradient.TopLeft; }
            set { mGradient = new Gradient(value); }
        }
        /// <summary>
        /// Gets or sets the color gradient on the sprite.
        /// </summary>
        public Gradient ColorGradient
        {
            get { return mGradient; }
            set { mGradient = value; }
        }
        /// <summary>
        /// Increments the rotation angle by the specified number of radians.
        /// </summary>
        /// <param name="radians"></param>
        public void IncrementRotationAngle(double radians)
        {
            mRotation += radians;
        }
        /// <summary>
        /// Increments the rotation angle by the specified number of degrees.
        /// </summary>
        /// <param name="degrees"></param>
        public void IncrementRotationAngleDegrees(double degrees)
        {
            IncrementRotationAngle(degrees * Math.PI / 180.0);
        }

        /// <summary>
        /// Gets or sets whether or not the sprite should be drawn when Draw is called.
        /// </summary>
        public bool Visible
        {
            get { return mVisible; }
            set { mVisible = value; }
        }

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
            if (Animating == false)
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
            if (PlayReverse)
                CurrentFrameIndex--;
            else
                CurrentFrameIndex++;
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
                if (mFrames.Count == 0)
                {
                    mCurrentFrameIndex = 0;
                    return;
                }

                switch (AnimationType)
                {
                    case AnimType.Looping:

                        while (value < 0)
                            value += mFrames.Count;

                        mCurrentFrameIndex = value % mFrames.Count;



                        break;

                    case AnimType.Once:
                        if (PlayReverse && value == 0)
                        {
                            mCurrentFrameIndex = mFrames.Count - 1;
                            Animating = false;
                        }
                        else if (PlayReverse == false && value == mFrames.Count - 1)
                        {
                            mCurrentFrameIndex = 0;
                            Animating = false;
                        }
                        else
                        {
                            mCurrentFrameIndex = value % mFrames.Count;
                        }

                        break;

                    case AnimType.Twice:
                        if (PlayReverse && value == 0)
                        {
                            mCurrentFrameIndex = mFrames.Count - 1;
                            mAnimType = AnimType.Once;
                        }
                        else if (PlayReverse == false && value == mFrames.Count - 1)
                        {
                            mCurrentFrameIndex = 0;
                            mAnimType = AnimType.Once;
                        }
                        else
                        {
                            mCurrentFrameIndex = value % mFrames.Count;
                        }

                        break;
                    case AnimType.OnceHoldLast:
                        if (PlayReverse && value == 0)
                        {
                            mCurrentFrameIndex = 0;
                            mIsAnimating = false;
                        }
                        else if (PlayReverse == false && value == mFrames.Count - 1)
                        {
                            mCurrentFrameIndex = mFrames.Count - 1;
                            mIsAnimating = false;
                        }
                        else
                        {
                            mCurrentFrameIndex = value % mFrames.Count;
                        }

                        break;

                    case AnimType.PingPong:
                        // this makes it so that you can have a 10 frame pingpong animation, 
                        // set it to frame 12, and it will actually show frame 8, because of 
                        // the reflection at the end.
                        value %= (mFrames.Count * 2);

                        if (value >= mFrames.Count)
                            value = 2 * mFrames.Count - 1 - value;

                        mCurrentFrameIndex = value;

                        // check for the ping-ponging.
                        if (PlayReverse && value == 0)
                        {
                            PlayReverse = false;
                        }
                        else if (PlayReverse == false && value == mFrames.Count - 1)
                        {
                            PlayReverse = true;
                        }

                        break;

                    case AnimType.OnceDisappear:
                        if (PlayReverse && value == 0)
                        {
                            mCurrentFrameIndex = value;
                            mVisible = false;
                        }
                        else if (PlayReverse == false && value == mFrames.Count - 1)
                        {
                            mCurrentFrameIndex = value;
                            mVisible = false;
                        }
                        else
                        {
                            mCurrentFrameIndex = value % mFrames.Count;
                        }

                        break;

                    default:
                        throw new Exception("Error: AnimationType not valid!");
                }

                if (mCurrentFrameIndex < 0 || mCurrentFrameIndex >= mFrames.Count)
                    throw new Exception("Error: Frame Index is in the wrong place!");
            }
        }
        /// <summary>
        /// Gets the currently displaying frame.
        /// </summary>
        public SpriteFrame2 CurrentFrame
        {
            get { return mFrames[CurrentFrameIndex]; }
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
        public AnimType AnimationType
        {
            get { return mAnimType; }
            set { mAnimType = value; }
        }
        /// <summary>
        /// Gets or sets a flag which indicates:
        /// True if the animation is running.
        /// False if a single frame will be shown indefinitely.
        /// </summary>
        [Obsolete("Use IsAnimating property instead.")]
        public bool Animating
        {
            get { return IsAnimating; }
            set { IsAnimating = value; }
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
                    (AnimationType == AnimType.Once || AnimationType == AnimType.OnceDisappear ||
                     AnimationType == AnimType.OnceHoldLast))
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
        /// Gets the list of SpriteFrame2 objects in this sprite.
        /// </summary>
        public List<SpriteFrame2> Frames
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
        /// Event handler type for sprite events.
        /// </summary>
        /// <param name="sprite"></param>
        public delegate void SpriteEventHandler(Sprite2 sprite);

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
    }
}
