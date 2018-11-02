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
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Quality;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgateLib.Display.Sprites
{ /// <summary>
  /// Basic interface implemented by different sprite classes.
  /// </summary>
    public interface ISprite
    {
        /// <summary>
        /// Draws the sprite at the specified position.
        /// </summary>
        /// <param name="destX"></param>
        /// <param name="destY"></param>
        void Draw(SpriteBatch spriteBatch, Vector2 position, int layerDepth = 0);

        /// <summary>
        /// Shows the next frame in the sequence.  This pays attention
        /// to whether the animation is playing forwards or reverse.
        /// </summary>
        void AdvanceFrame();

        /// <summary>
        /// Updates the animation of the sprite, using the given frame time.
        /// </summary>
        /// <param name="time_ms">The amount of time to consider passed, in milliseconds.</param>
        void Update(GameTime time);

        /// <summary>
        /// Gets or sets an enum value indicating what type of animation is happening.
        /// Looping - The animation will play from beginning to end and then restart.
        /// PingPong - The animation will play from beginning to end and then from end to beginning (continuously).
        /// Once - The animation plays once, and then shows its first frame.
        /// OnceHoldLast - The animation plays once, and leaves the last frame on.
        /// </summary>
        AnimationType AnimationType { get; set; }

        /// <summary>
        /// Gets the currently displaying frame.
        /// </summary>
        ISpriteFrame CurrentFrame { get; }

        /// <summary>
        /// The index of the current frame.
        /// </summary>
        int CurrentFrameIndex { get; set; }
        /// <summary>
        /// Gets or sets a flag which indicates:
        /// True if the animation is running.
        /// False if a single frame will be shown indefinitely.
        /// </summary>
        bool IsAnimating { get; set; }
        /// <summary>
        /// Gets or sets a flag which indicates whether or not this animation plays in 
        /// reverse instead.
        /// </summary>
        bool PlayReverse { get; set; }

        /// <summary>
        /// Gets height of the sprite.
        /// </summary>
        int SpriteHeight { get; }
        /// <summary>
        /// Gets the size of the sprite.
        /// </summary>
        Size SpriteSize { get; }
        /// <summary>
        /// Gets width of the sprite.
        /// </summary>
        int SpriteWidth { get; }
        /// <summary>
        /// Restarts the animation.
        /// </summary>
        void StartAnimation();

        int DisplayWidth { get; }
        int DisplayHeight { get; }

        /// <summary>
        /// Gets the list of SpriteFrame objects in this sprite.
        /// </summary>
        IReadOnlyList<ISpriteFrame> Frames { get; }

        /// <summary>
        /// The amount of time each frame should display, in milliseconds.
        /// </summary>
        TimeSpan FrameTime { get; set; }

        /// <summary>
        /// If Visible is set to false, all calls to Draw overloads are ignored.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the transparency. Valid values are between 0 (transparent) and 1 (opaque).
        /// </summary>
        float Alpha { get; set; }

        /// <summary>
        /// Rotation center point.
        /// </summary>
        Vector2 RotationCenter { get; set; }

        /// <summary>
        /// Rotation angle in radians.
        /// </summary>
        float RotationAngle { get; set; }
        /// <summary>
        /// Rotation angle in degrees.
        /// </summary>
        float RotationAngleDegrees { get; set; }

        /// <summary>
        /// Gets or sets the color of the sprite.
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// Event raised when the animation is started.
        /// </summary>
        event SpriteEventHandler AnimationStarted;

        /// <summary>
        /// Event raised when the animation is stopped.
        /// </summary>
        event SpriteEventHandler AnimationStopped;
        /// <summary>
        /// Event which is raised when the play direction is changed, as
        /// in the PingPong type.
        /// </summary>
        event SpriteEventHandler PlayDirectionChanged;
    }

    /// <summary>
    /// A class for representing an animated image.
    /// </summary>
    public class Sprite : ISprite
    {
        /// <summary>
        /// The default amount of time for a frame to be displayed.
        /// </summary>
        public const int DEFAULT_FRAME_TIME_MS = 60;

        private List<SpriteFrame> frames = new List<SpriteFrame>();
        private int currentFrameIndex = 0;
        private double frameTime = 0;
        private bool mPlayReverse = false;
        private bool isAnimating = true;

        #region --- Construction ---

        public Sprite() { }

        public Sprite(Sprite copyFrom)
        {
            SpriteSize = copyFrom.SpriteSize;
            ScaleWidth = copyFrom.ScaleWidth;
            ScaleHeight = copyFrom.ScaleHeight;
            Color = copyFrom.Color;
            RotationAngle = copyFrom.RotationAngle;
            RotationCenter = copyFrom.RotationCenter;
            DisplayAlignment = copyFrom.DisplayAlignment;
            Visible = copyFrom.Visible;

            foreach(var frame in copyFrom.Frames)
            {
                AddFrame(frame);
            }
        }

        public Sprite Clone()
        {
            return new Sprite(this);
        }

        public void AddFrame(SpriteFrame frame)
        {
            if (SpriteSize.IsZero)
                SpriteSize = frame.SpriteSize;

            frame.SpriteSize = SpriteSize;

            frames.Add(frame);
        }

        #endregion
        #region --- Working with Frames ---

        /// <summary>
        /// Adds a surface as a frame to the sprite.
        /// </summary>
        /// <param name="texture"></param>
        public void AddFrame(Texture2D texture)
        {
            SpriteFrame frame = new SpriteFrame(texture);
            frame.SourceRect = new Rectangle(0, 0, texture.Width, texture.Height);
            frame.Anchor = Point.Zero;

            AddFrame(frame);
        }

        /// <summary>
        /// Adds a frame to the sprite.
        /// </summary>
        /// <param name="surface">The surface from which to get the image data.</param>
        /// <param name="sourceRect">The source rectangle for the image data used in the sprite frame to be added.</param>
        /// <param name="ownSurface">Pass true to indicate that this sprite should own the surface and dispose of it when finished.</param>
        /// <param name="anchor">The offset within the sprite to the upperleft corner of where the frame is drawn.</param>
        public void AddFrame(Texture2D texture, bool ownSurface, Rectangle sourceRect, Point anchor)
        {
            SpriteFrame frame = new SpriteFrame(texture);
            frame.SourceRect = sourceRect;
            frame.Anchor = anchor;

            AddFrame(frame);
        }

        /// <summary>
        /// Slices and dices the image passed into frames and adds them.
        /// Frames are taken from the surface from left to right.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="ownSurface">Pass true to indicate that this sprite should own the surface, and handle its disposal.</param>
        /// <param name="startPoint">The starting point in pixels from which to parse frames.</param>
        /// <param name="size">The size of the image to cut out for each frame</param>
        /// <param name="extraSpace">How many extra pixels to insert between each frame.</param>
        public void AddNewFrames(Texture2D texture, Point startPoint, Point extraSpace, Size size)
        {
            Point location = startPoint;

            do
            {
                SpriteFrame currentFrame = new SpriteFrame(texture);
                Rectangle currentRect = new Rectangle(location, size);
                bool skip = false;

                currentFrame.SourceRect = currentRect;

                if (currentFrame.SourceRect.Right > texture.Width) skip = true;
                if (currentFrame.SourceRect.Bottom > texture.Height) skip = true;

                if (skip == false)
                    frames.Add(currentFrame);

                location.X += size.Width + extraSpace.X;

                if (location.X + size.Width > texture.Width)
                {
                    location.X = 0;
                    location.Y += size.Height + extraSpace.Y;
                }

            } while (location.Y + size.Height <= texture.Height);
        }

        #endregion

        #region --- Sprite properties ---

        /// <summary>
        /// Gets width of the sprite.
        /// </summary>
        public int SpriteWidth => SpriteSize.Width;

        /// <summary>
        /// Gets height of the sprite.
        /// </summary>
        public int SpriteHeight => SpriteSize.Height;

        /// <summary>
        /// Gets the size of the sprite.
        /// </summary>
        public Size SpriteSize { get; set; }

        /// <summary>
        /// Gets or sets the amount the width is scaled.
        /// </summary>
        public float ScaleWidth { get; set; } = 1;

        /// <summary>
        /// Gets or sets the amount the height is scaled.
        /// </summary>
        public float ScaleHeight { get; set; } = 1;

        /// <summary>
        /// Gets the width of the sprite when displayed.
        /// </summary>
        public int DisplayWidth
        {
            get { return (int)(ScaleWidth * SpriteWidth); }
            set { ScaleWidth = value / (float)SpriteWidth; }
        }

        /// <summary>
        /// Gets the height of the sprite when displayed.
        /// </summary>
        public int DisplayHeight
        {
            get { return (int)(ScaleHeight * SpriteHeight); }
            set { ScaleHeight = value / (float)SpriteHeight; }
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
        public float Alpha
        {
            get => Color.A / 255.0f;
            set => Color = new Color(Color, value);
        }

        /// <summary>
        /// Gets or sets the rotation angle in radians.
        /// </summary>
        public float RotationAngle { get; set; }

        /// <summary>
        /// Gets or sets the rotation angle in degrees.
        /// </summary>
        public float RotationAngleDegrees
        {
            get => RotationAngle * 180 / MathF.PI;  // magic number to convert radians to degrees.
            set => RotationAngle = value * MathF.PI / 180;
        }

        public Vector2 RotationCenter { get; set; }

        /// <summary>
        /// Gets or sets the interpretation of the position.
        /// </summary>
        public OriginAlignment DisplayAlignment { get; set; }

        /// <summary>
        /// Gets or sets the scale of the sprite.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2 Scale
        {
            get => new Vector2(ScaleWidth, ScaleHeight);
            set
            {
                ScaleWidth = value.X;
                ScaleHeight = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the color of the sprite.
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Gets or sets whether or not the sprite should be drawn when Draw is called.
        /// </summary>
        public bool Visible { get; set; } = true;

        #endregion

        #region --- Animation Properties and Methods ---

        /// <summary>
        /// Updates the animation of the sprite, using the given frame time.
        /// </summary>
        /// <param name="time_ms">The amount of time to consider as passed, in milliseconds.</param>
        public void Update(GameTime time)
        {
            if (IsAnimating == false)
                return;

            frameTime -= time.ElapsedGameTime.TotalMilliseconds;

            while (frameTime <= 0)
            {
                AdvanceFrame();

                frameTime += (CurrentFrame.Time ?? FrameTime).TotalMilliseconds;
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
                case AnimationType.Looping:
                    while (newFrameIndex < 0)
                        newFrameIndex += frames.Count;

                    newFrameIndex = newFrameIndex % frames.Count;

                    break;

                case AnimationType.Once:
                    if (PlayReverse && newFrameIndex == -1)
                    {
                        newFrameIndex = frames.Count - 1;
                        IsAnimating = false;
                    }
                    else if (PlayReverse == false && newFrameIndex == frames.Count)
                    {
                        newFrameIndex = 0;
                        IsAnimating = false;
                    }

                    break;

                case AnimationType.Twice:
                    if (PlayReverse && newFrameIndex == -1)
                    {
                        newFrameIndex = frames.Count - 1;
                        AnimationType = AnimationType.Once;
                    }
                    else if (PlayReverse == false && newFrameIndex == frames.Count)
                    {
                        newFrameIndex = 0;
                        AnimationType = AnimationType.Once;
                    }

                    break;

                case AnimationType.OnceHoldLast:
                    if (PlayReverse && newFrameIndex == -1)
                    {
                        newFrameIndex = 0;
                        IsAnimating = false;
                    }
                    else if (PlayReverse == false && newFrameIndex == frames.Count)
                    {
                        newFrameIndex = frames.Count - 1;
                        IsAnimating = false;
                    }

                    break;

                case AnimationType.PingPong:
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
                    else if (PlayReverse == false && newFrameIndex == frames.Count)
                    {
                        PlayReverse = true;
                        newFrameIndex = frames.Count - 2;
                    }

                    break;

                case AnimationType.OnceDisappear:
                    if (PlayReverse && newFrameIndex == -1)
                    {
                        newFrameIndex = 0;
                        Visible = false;
                    }
                    else if (PlayReverse == false && newFrameIndex == frames.Count)
                    {
                        newFrameIndex = frames.Count - 1;
                        Visible = false;
                    }

                    break;

                default:
                    throw new SpriteException("Error: AnimationType not valid!");
            }

            CurrentFrameIndex = newFrameIndex;

            if (currentFrameIndex < 0 || currentFrameIndex >= frames.Count)
                throw new SpriteException("Error: Frame Index is in the wrong place!");
        }

        /// <summary>
        /// The amount of time each frame should display, in milliseconds.
        /// </summary>
        public TimeSpan FrameTime { get; set; } = TimeSpan.FromMilliseconds(DEFAULT_FRAME_TIME_MS);

        /// <summary>
        /// The index of the current frame.
        /// </summary>
        public int CurrentFrameIndex
        {
            get
            {
                return currentFrameIndex;
            }
            set
            {
                Require.ArgumentInRange((value >= 0 && value < Frames.Count) || Frames.Count == 0 && value == 0,
                    nameof(CurrentFrameIndex), "Current Frame Index must be between 0 and Frames.Count - 1");

                if (Frames.Count <= 1)
                {
                    currentFrameIndex = 0;
                    return;
                }

                currentFrameIndex = value;
            }
        }
        /// <summary>
        /// Gets the currently displaying frame.
        /// </summary>
        public SpriteFrame CurrentFrame
        {
            get { return frames[CurrentFrameIndex]; }
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
        public AnimationType AnimationType { get; set; } = AnimationType.Looping;
        /// <summary>
        /// Gets or sets a flag which indicates:
        /// True if the animation is running.
        /// False if a single frame will be shown indefinitely.
        /// </summary>
        public bool IsAnimating
        {
            get => isAnimating;
            set
            {
                bool doEvent = false;

                if (value != isAnimating)
                    doEvent = true;

                isAnimating = value;

                if (value && (AnimationType == AnimationType.Once
                           || AnimationType == AnimationType.OnceDisappear
                           || AnimationType == AnimationType.OnceHoldLast))
                {

                    if (this.PlayReverse && currentFrameIndex == 0)
                        currentFrameIndex = frames.Count - 1;
                    else if (currentFrameIndex == frames.Count - 1)
                        currentFrameIndex = 0;

                }

                if (doEvent)
                {
                    if (isAnimating == true && AnimationStarted != null)
                        AnimationStarted(this);
                    else if (isAnimating == false && AnimationStopped != null)
                        AnimationStopped(this);
                }
            }
        }

        /// <summary>
        /// Gets the list of frames in this sprite.
        /// </summary>
        public IReadOnlyList<SpriteFrame> Frames => frames;
        IReadOnlyList<ISpriteFrame> ISprite.Frames => Frames;

        /// <summary>
        /// Restarts the animation.
        /// </summary>
        public void StartAnimation()
        {
            if (PlayReverse)
                CurrentFrameIndex = frames.Count - 1;
            else
                CurrentFrameIndex = 0;

            isAnimating = true;
            Visible = true;
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
        /// Draws the sprite at the specified position.
        /// </summary>
        /// <param name="state"></param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, int layerDepth = 0)
        {
            var frame = CurrentFrame;

            Vector2 actualScale = new Vector2(frame.SpriteSize.Width / (float)SpriteSize.Width,
                                              frame.SpriteSize.Height / (float)SpriteSize.Height);

            actualScale.X *= ScaleWidth;
            actualScale.Y *= ScaleHeight;

            CurrentFrame.Draw(spriteBatch, position, RotationCenter, actualScale, PremultipliedColor, RotationAngle, layerDepth);
        }

        public bool FlipHorizontal { get; set; }

        public bool FlipVertical { get; set; }

        /// <summary>
        /// Returns the total animation time in milliseconds.
        /// </summary>
        public TimeSpan TotalAnimationTime
        {
            get => TimeSpan.FromSeconds(Frames.Sum(x => (x.Time ?? TimeSpan.FromMilliseconds(DEFAULT_FRAME_TIME_MS)).TotalSeconds));
        }

        public Color PremultipliedColor
        {
            get
            {
                float a = Color.A / 255.0f;
                float r = Color.R / 255.0f * a;
                float g = Color.G / 255.0f * a;
                float b = Color.B / 255.0f * a;

                return new Color(r, g, b, a);
            }
        }

        public void SetRotationCenter(OriginAlignment alignment)
                {
                    RotationCenter = Origin.CalcF(alignment, SpriteSize);
                }
            }

        }
