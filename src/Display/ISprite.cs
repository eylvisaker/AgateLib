using System;
using System.Collections.Generic;

namespace AgateLib.Display
{
    using Geometry;
    
    /// <summary>
    /// Basic interface implemented by different sprite classes.
    /// </summary>
    public interface ISprite : ISurface, IDisposable 
    {
        /// <summary>
        /// Shows the next frame in the sequence.  This pays attention
        /// to whether the animation is playing forwards or reverse.
        /// </summary>
        void AdvanceFrame();

        /// <summary>
        /// Updates the animation of the sprite, using the DeltaTime given
        /// by the Display object.
        /// </summary>
        void Update();
        /// <summary>
        /// Updates the animation of the sprite, using the given frame time.
        /// </summary>
        /// <param name="time_ms">The amount of time to consider passed, in milliseconds.</param>
        void Update(double time_ms);

        /// <summary>
        /// Gets or sets an enum value indicating what type of animation is happening.
        /// Looping - The animation will play from beginning to end and then restart.
        /// PingPong - The animation will play from beginning to end and then from end to beginning (continuously).
        /// Once - The animation plays once, and then shows its first frame.
        /// OnceHoldLast - The animation plays once, and leaves the last frame on.
        /// </summary>
        SpriteAnimType AnimationType { get; set; }
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

        /// <summary>
        /// Gets the list of SpriteFrame objects in this sprite.
        /// </summary>
        Sprites.IFrameList Frames { get; }

        /// <summary>
        /// The amount of time each frame should display, in milliseconds.
        /// </summary>
        double TimePerFrame { get; set; }
        /// <summary>
        /// If Visible is set to false, all calls to Draw overloads are ignored.
        /// </summary>
        bool Visible { get; set; }


        /// <summary>
        /// Event which is raised when the animation is started.
        /// </summary>
        event SpriteEventHandler AnimationStarted;

        /// <summary>
        /// Event which is raised when the animation is stopped.
        /// </summary>
        event SpriteEventHandler AnimationStopped;
        /// <summary>
        /// Event which is raised when the play direction is changed, as
        /// in the PingPong type.
        /// </summary>
        event SpriteEventHandler PlayDirectionChanged;
    }

    /// <summary>
    /// Event handler type for sprite events.
    /// </summary>
    /// <param name="caller"></param>
    public delegate void SpriteEventHandler(ISprite caller);

    /// <summary>
    /// Enum indicating the different types of automatic animation that
    /// take place.
    /// </summary>
    public enum SpriteAnimType
    {
        /// <summary>
        /// Specifies that the sprite animation should go from
        /// frame 0 to the end, and start back at frame 0 to repeat.
        /// </summary>
        Looping,
        /// <summary>
        /// Specifies that the sprite animation should go from
        /// frame 0 to the end, and then go back down to frame 0.  This
        /// cycle repeats indefinitely.
        /// </summary>
        PingPong,
        /// <summary>
        /// Specifies that the sprite animation should go from
        /// frame 0 to the end and stop, but show frame 0 once the animation
        /// is finished.
        /// </summary>
        Once,
        /// <summary>
        /// Specifies that the sprite animation should go from
        /// frame 0 to the end and stop there, with the last frame
        /// shown.
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
        /// Specifies that the sprite animation should go twice.
        /// </summary>
        Twice,
    }

}
