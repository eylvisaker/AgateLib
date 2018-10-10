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

namespace AgateLib.Display.Sprites
{
    /// <summary>
    /// Event handler type for sprite events.
    /// </summary>
    /// <param name="caller"></param>
    public delegate void SpriteEventHandler(ISprite caller);

    /// <summary>
    /// Enum indicating the different types of automatic animation that
    /// take place.
    /// </summary>
    public enum AnimationType
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
