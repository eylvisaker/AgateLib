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
using System.IO;
using System.Text;

using ERY.AgateLib.Geometry;
using ERY.AgateLib.Resources;

namespace ERY.AgateLib
{
    using Sprites;

    /// <summary>
    /// The Sprite class represents a series of frames which are used
    /// to create a single animation.
    /// </summary>
    public class Sprite : ISprite, ISurface 
    {
        FrameList<SpriteFrame> mFrames = new FrameList<SpriteFrame>();
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
        /// <param name="filename"></param>
        /// <param name="size"></param>
        public Sprite(string filename, Size size)
            : this(size)
        {
            AddFrames(filename);
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
        /// <param name="size"></param>
        public Sprite(Surface surface, Size size)
            : this(size)
        {
            AddFrames(surface);
        }
        /// <summary>
        /// Constructs a Sprite object, of the specified width and height.
        /// Frames are cut out from the given surface of the specified size.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="size"></param>
        public Sprite(Stream stream, Size size)
            : this(size)
        {
            using (Surface surf = new Surface(stream))
            {
                AddFrames(surf);
            }
        }
        /// <summary>
        /// Constructs a Sprite object, of the specified width and height.
        /// Frames are cut out from the given surface of the specified size.
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Sprite(Surface surface, int width, int height)
            : this(surface, new Size(width, height))
        {
        }
        /// <summary>
        /// Constructs a Sprite object, loading it from the file specified
        /// in the given ResourceManager object.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="resources"></param>
        [Obsolete]
        public Sprite(string name, ResourceManager resources)
        {
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
        }
        /// <summary>
        /// Makes a copy of this sprite and returns it.
        /// </summary>
        /// <returns></returns>
        public Sprite Clone()
        {
            Sprite retval = new Sprite(mSpriteSize.Width, mSpriteSize.Height);

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

            foreach (SpriteFrame f in mFrames)
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
            foreach (SpriteFrame f in mFrames)
            {
                f.Dispose();
            }

            mFrames.Clear();
        }

        #endregion
        #region --- Working with Frames ---

        /// <summary>
        /// Slices and dices the image passed into frames and adds them.
        /// Automatically skips blank ones.
        /// </summary>
        /// <param name="filename"></param>
        public void AddFrames(string filename)
        {
            if (SpriteIsCached(filename))
            {
                if (LoadFromCache(filename))
                    return;
            }

            using (Surface surf = new Surface(filename))
            {
                surf.ShouldBePacked = false;

                AddFrames(surf);
            }

            SaveToCache(filename);
        }

        /// <summary>
        /// Adds frames from the given surface, using the size of this sprite.
        /// </summary>
        /// <param name="surface"></param>
        public void AddFrames(Surface surface)
        {
            AddFrames(surface, Point.Empty, Point.Empty, true);
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
            if (SpriteIsCached(filename))
            {
                if (LoadFromCache(filename))
                    return;
            }

            using (Surface surf = new Surface(filename))
            {
                AddFrames(surf, startPoint, extraSpace, skipBlank);
            }

            SaveToCache(filename);
        }
        /// <summary>
        /// Adds frames from the given surface, using the size of this sprite.
        /// Frames are taken from startPoint, and with extraSpace inbetween.
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="startPoint"></param>
        /// <param name="extraSpace"></param>
        /// <param name="skipBlank"></param>
        public void AddFrames(Surface surface, Point startPoint, Point extraSpace, bool skipBlank)
        {
            AddFrames(surface, startPoint, extraSpace, SpriteSize, skipBlank);
        }

        /// <summary>
        /// Slices and dices the image passed into frames and adds them.
        /// Frames are taken from the surface from left to right.
        /// </summary>
        /// <param name="surface">The surface to use to split up into the sprite frames.</param>
        /// <param name="startPoint">The starting point in pixels from which to parse frames.</param>
        /// <param name="size">The size of the image to cut out for each frame</param>
        /// <param name="extraSpace">How many extra pixels to insert between each frame.</param>
        /// <param name="skipBlank">Whether or not blank frames should be automatically dropped.</param>
        public void AddFrames(Surface surface, Point startPoint, Point extraSpace, Size size, bool skipBlank)
        {
            Point location = startPoint;

            do
            {
                SpriteFrame currentFrame = new SpriteFrame();
                Rectangle currentRect = new Rectangle(location, size);

                currentFrame.SetFrame(surface, currentRect);

                if (currentFrame.IsBlank == false || skipBlank == false)
                    mFrames.Add(currentFrame);

                location.X += size.Width;

                if (location.X + size.Width > surface.SurfaceWidth)
                {
                    location.X = 0;
                    location.Y += size.Height;
                }

            } while (location.Y < surface.SurfaceHeight);

        }

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
            if (SpriteIsCached(filename))
            {
                if (LoadFromCache(filename))
                    return;
            }

            using (Surface surf = new Surface(filename))
            {
                AddFrames(surf, startPoint, extraSpace, size, array);
            }

            SaveToCache(filename);
        }
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
                SpriteFrame currentFrame = new SpriteFrame();
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
        #endregion

        #region --- Caching frames ---

        private string CacheRootPath(string filename)
        {
            filename = Path.GetFileName(filename);

            string retval = Path.Combine(mSpriteCachePath, filename);

            return retval;
        }
        private string CacheInfoPath(string filename)
        {
            return CacheRootPath(filename) + ".cache.xml";
        }
        private bool SpriteIsCached(string filename)
        {
            if (UseSpriteCache == false)
                return false;

            string cacheInfoPath = CacheInfoPath(filename);

            if (File.Exists(cacheInfoPath) == false)
                return false;

            // check to see if the source file is newer than the dest file
            if (File.GetLastWriteTime(filename) > File.GetLastWriteTime(cacheInfoPath))
                return false;
            else
                return true;
        }

        private void SaveToCache(string filename)
        {
            if (UseSpriteCache == false)
                return;

            string cacheInfoPath = CacheInfoPath(filename);

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            System.Xml.XmlNode rootNode = doc.CreateElement("SpriteCache");
            doc.AppendChild(rootNode);

            for (int i = 0; i < mFrames.Count; i++)
            {
                SpriteFrame frame = mFrames[i];
                string frameFile = cacheInfoPath.Replace('\\', '/') + string.Format(".frame{0}.png", i);

                System.Xml.XmlElement element = doc.CreateElement("Frame");

                element.SetAttribute("FileName", frameFile);
                element.SetAttribute("OriginalWidth", frame.OriginalSize.Width.ToString());
                element.SetAttribute("OriginalHeight", frame.OriginalSize.Height.ToString());
                element.SetAttribute("OffsetX", frame.FrameOffset.X.ToString());
                element.SetAttribute("OffsetY", frame.FrameOffset.Y.ToString());

                rootNode.AppendChild(element);
                frame.Surface.SaveTo(frameFile, ImageFileFormat.Png);
            }

            doc.Save(cacheInfoPath);
        }
        private bool LoadFromCache(string filename)
        {
            string cacheInfoPath = CacheInfoPath(filename);

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(cacheInfoPath);

            System.Xml.XmlNode rootNode = doc.ChildNodes[0];

            foreach (System.Xml.XmlNode frame in rootNode.ChildNodes)
            {
                string frameFilename = frame.Attributes["FileName"].Value;
                int originalWidth = int.Parse(frame.Attributes["OriginalWidth"].Value);
                int originalHeight = int.Parse(frame.Attributes["OriginalHeight"].Value);
                int offsetX = int.Parse(frame.Attributes["OffsetX"].Value);
                int offsetY = int.Parse(frame.Attributes["OffsetY"].Value);

                SpriteFrame spFrame = new SpriteFrame();

                if (System.IO.File.Exists(frameFilename) == false)
                    return false;

                spFrame.SetFrameNoTrim(new Surface(frameFilename),
                                     new Point(offsetX, offsetY),
                                     new Size(originalWidth, originalHeight));

                mFrames.Add(spFrame);
            }


            return true;
        }

        #endregion

        #region --- Static Members ---

        private static bool mUseSpriteCache = false;
        private static string mSpriteCachePath = "";

        /// <summary>
        /// Bool which indicates whether sprite frames should be cached to 
        /// disk after they've been cut out.  Speed up loading if used.
        /// </summary>
        public static bool UseSpriteCache
        {
            get { return mUseSpriteCache; }
            set { mUseSpriteCache = value; }
        }
        /// <summary>
        /// Sets the path where SpriteFrames are saved, if the sprite cache
        /// is used.
        /// </summary>
        public static string SpriteCachePath
        {
            get { return mSpriteCachePath; }
            set
            {
                if (Directory.Exists(value) == false)
                {
                    Directory.CreateDirectory(value);
                }

                mSpriteCachePath = value;
                mUseSpriteCache = true;
            }
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

            SpriteFrame current = CurrentFrame;
            Surface surf = current.Surface;

            current.DisplaySize = destRect.Size;

            PointF alignment = Origin.CalcF(DisplayAlignment, DisplaySize);
            PointF rotation = Origin.CalcF(RotationCenter, DisplaySize);

            surf.Alpha = Alpha;
            surf.DisplayAlignment = DisplayAlignment;
            surf.RotationAngle = RotationAngle;
            surf.RotationCenter = RotationCenter;
            surf.Color = Color;

            current.Draw(destRect.X - alignment.X, destRect.Y - alignment.Y, rotation.X, rotation.Y);
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

            SpriteFrame current = CurrentFrame;
            Surface surf = current.Surface;

            current.DisplaySize = DisplaySize;

            PointF alignment = Origin.CalcF(DisplayAlignment, DisplaySize);
            PointF rotation = Origin.CalcF(RotationCenter, DisplaySize);

            surf.Alpha = Alpha;
            surf.DisplayAlignment = OriginAlignment.TopLeft;
            surf.RotationAngle = RotationAngle;
            surf.Color = Color;

            current.Draw(destX - alignment.X, destY - alignment.Y, rotation.X, rotation.Y);
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
                            IsAnimating = false;
                        }
                        else if (PlayReverse == false && value == mFrames.Count - 1)
                        {
                            mCurrentFrameIndex = 0;
                            IsAnimating = false;
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
        SpriteFrame CurrentFrame
        {
            get { return mFrames[CurrentFrameIndex]; }
        }
        ISpriteFrame ISprite.CurrentFrame
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
        /// Gets the list of SpriteFrame objects in this sprite.
        /// </summary>
        public IFrameList Frames
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
        public delegate void SpriteEventHandler(Sprite sprite);

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

        #region --- ISprite Members with different types --


        event ERY.AgateLib.SpriteEventHandler ISprite.AnimationStarted
        {
            add { throw new Exception("The method or operation is not implemented."); }
            remove { throw new Exception("The method or operation is not implemented."); }
        }
        event ERY.AgateLib.SpriteEventHandler ISprite.AnimationStopped
        {
            add { throw new Exception("The method or operation is not implemented."); }
            remove { throw new Exception("The method or operation is not implemented."); }
        }
        event ERY.AgateLib.SpriteEventHandler ISprite.PlayDirectionChanged
        {
            add { throw new Exception("The method or operation is not implemented."); }
            remove { throw new Exception("The method or operation is not implemented."); }
        }


        SpriteAnimType ISprite.AnimationType
        {
            get
            {
                return (SpriteAnimType)this.AnimationType;
            }
            set
            {
                this.AnimationType = (AnimType)value;
            }
        }

        #endregion
    }
}