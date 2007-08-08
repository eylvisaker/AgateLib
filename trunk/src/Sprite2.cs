using System;
using System.Collections.Generic;
using System.Text;

using ERY.AgateLib.Geometry;

namespace ERY.AgateLib
{
    public class Sprite2
    {
        List<SpriteFrame2> mFrames = new List<SpriteFrame2>();
        Size mSpriteSize;

        private double mTimePerFrame = 60;
        private int mCurrentFrameIndex = 0;
        private double mFrameTime = 0;
        private AnimType mAnimType = AnimType.Looping;
        private bool mPlayReverse = false;
        private bool mAnimating = true;
        private bool mVisible = true;

        private double mScaleX = 1.0;
        private double mScaleY = 1.0;

        private OriginAlignment mAlignment = OriginAlignment.TopLeft;
        private double mRotation = 0;
        private OriginAlignment mRotationSpot = OriginAlignment.Center;
        private Color mColor = Color.White;


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

    }
}
