using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.InputLib
{


    /// <summary>
    /// Class which describes details about an input event.
    /// </summary>
    public class InputEventArgs
    {
        KeyCode mKeyCode;
        KeyModifiers mModifiers;
        int mRepeatCount;
        string mKeyString;
        Point mMousePosition;
        Mouse.MouseButtons mButtons;
        int mWheelDelta;

        internal InputEventArgs()
        {
            Initialize();
        }
        internal InputEventArgs(KeyCode keyID, KeyModifiers mods)
        {
            mKeyCode = keyID;
            mKeyString = Keyboard.GetKeyString(keyID, mods);
            mModifiers = mods;

            Initialize();
        }
        internal InputEventArgs(KeyCode keyID, KeyModifiers mods, int repeatCount)
            : this(keyID, mods)
        {
            mRepeatCount = repeatCount;
        }

        internal InputEventArgs(Mouse.MouseButtons mouseButtons)
        {
            mButtons = mouseButtons;

            Initialize();
        }
        internal InputEventArgs(int wheelDelta)
        {
            mWheelDelta = wheelDelta;

            Initialize();
        }

        private void Initialize()
        {
            mMousePosition = Mouse.Position;
        }

        /// <summary>
        /// Gets which key was pressed.
        /// </summary>
        [Obsolete("Use KeyCode instead.")]
        public KeyCode KeyID
        {
            get { return mKeyCode; }
            internal set { mKeyCode = value; }
        }
        /// <summary>
        /// Gets which key was pressed.
        /// </summary>
        public KeyCode KeyCode
        {
            get { return mKeyCode; }
            internal set { mKeyCode = value; }
        }
        /// <summary>
        /// Gets the text created by the key which was pressed.
        /// </summary>
        public string KeyString
        {
            get { return mKeyString; }
        }

        /// <summary>
        /// The mouse position during this event
        /// </summary>
        public Point MousePosition
        {
            get { return mMousePosition; }
        }

        /// <summary>
        /// Gets how many times the keypress has been repeated.
        /// This is zero for the first time a key is pressed, and increases
        /// as the key is held down and KeyDown events are generated after that.
        /// </summary>
        public int RepeatCount
        {
            get { return mRepeatCount; }
        }

        /// <summary>
        /// Gets which mouse buttons were pressed.
        /// </summary>
        public Mouse.MouseButtons MouseButtons
        {
            get { return mButtons; }
        }

        /// <summary>
        /// Gets the amount the mouse wheel moved in this event.
        /// </summary>
        public int WheelDelta
        {
            get { return mWheelDelta; }
        }
    }

}
