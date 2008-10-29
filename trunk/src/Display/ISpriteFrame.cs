using System;

namespace AgateLib.Display
{
    using Geometry;

    /// <summary>
    /// Basic interface implemented by a particular frame in a sprite.
    /// </summary>
    public interface ISpriteFrame
    {
        /// <summary>
        /// Draws the frame.
        /// </summary>
        /// <param name="dest_x"></param>
        /// <param name="dest_y"></param>
        /// <param name="rotationCenterX"></param>
        /// <param name="rotationCenterY"></param>
        void Draw(float dest_x, float dest_y, float rotationCenterX, float rotationCenterY);

        /// <summary>
        /// Gets the surface object the frame is drawn from
        /// </summary>
        Surface Surface { get; }

        /// <summary>
        /// Gets the source rectangle on the surface the frame is drawn from.
        /// </summary>
        Rectangle SourceRect { get; }
    }
}
