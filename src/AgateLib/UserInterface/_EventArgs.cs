﻿using AgateLib.UserInterface.InputMap;
using System;

namespace AgateLib.UserInterface
{
    public class UserInterfaceActionEventArgs : EventArgs
    {
        public UserInterfaceActionEventArgs()
        { }

        public UserInterfaceActionEventArgs(UserInterfaceAction action)
        {
            Reset(action);
        }

        /// <summary>
        /// Gets the action initiated by the user.
        /// </summary>
        public UserInterfaceAction Action { get; private set; }

        /// <summary>
        /// Set this to true to prevent further processing of this event.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Resets this event argument structure to handle a new event type.
        /// This method returns the object it is called on.
        /// </summary>
        /// <param name="action"></param>
        public UserInterfaceActionEventArgs Reset(UserInterfaceAction action)
        {
            this.Action = action;
            Handled = false;

            return this;
        }
    }

    public class ButtonStateEventArgs : EventArgs
    {
        public UserInterfaceAction ActionButton { get; internal set; }
        public UserInterfaceInputState InputState { get; internal set; }
        public bool Handled { get; set; }
    }
}
