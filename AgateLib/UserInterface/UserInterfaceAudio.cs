using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface
{
    /// <summary>
    /// Interface for an object which plays sounds when the user performs actions in the
    /// user interface.
    /// </summary>
    public interface IUserInterfaceAudio
    {
        /// <summary>
        /// Plays a particular UI sound. Returns true if the sound was successfully played.
        /// </summary>
        /// <param name="originator">The render element or widget that originated the sound.</param>
        /// <param name="sound">The sound to play.</param>
        bool PlaySound(object originator, UserInterfaceSound sound);
    }

    /// <summary>
    /// Enum for describing the sounds the UI controls can generate.
    /// </summary>
    public enum UserInterfaceSound
    {
        /// <summary>
        /// Sound effect when the user navigates from one control to another.
        /// </summary>
        Navigate,

        /// <summary>
        /// Sound effect when the user accepts an item.
        /// </summary>
        Accept,

        /// <summary>
        /// Sound effect when the user cancels their action.
        /// </summary>
        Cancel,

        /// <summary>
        /// Sound effect when the user attempts to perfom an invalid action.
        /// </summary>
        Invalid,
    }
}
