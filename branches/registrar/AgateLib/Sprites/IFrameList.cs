using System;
using System.Collections;
using System.Text;
using AgateLib.DisplayLib;

namespace AgateLib.Sprites
{
    /// <summary>
    /// Iterface implemented by a list of sprite frames.
    /// This provides a read-only view into the frames in a sprite.
    /// </summary>
    public interface IFrameList : IEnumerable
    {

        /// <summary>
        /// Returns the number of frames in the list.
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Gets a frame by index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        ISpriteFrame this[int index] { get; }

        /// <summary>
        /// Searches for a particular frame.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int IndexOf(ISpriteFrame item);

        /// <summary>
        /// Returns a bool indicating whether the specified frame is
        /// contained in the list.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Contains(ISpriteFrame item);

        /// <summary>
        /// Copies the list of frame to a target array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        void CopyTo(ISpriteFrame[] array, int arrayIndex);
    }
}
