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
using System.Text;

using AgateLib.Geometry;

namespace AgateLib.InputLib
{
    using AgateLib.DisplayLib;

    /// <summary>
    /// Class which encapsulates input from the mouse.
    /// </summary>
    public static class Mouse
    {
        /// <summary>
        /// Mouse Buttons enum.
        /// </summary>
        public enum MouseButtons
        {
            /// <summary>
            /// No mouse button
            /// </summary>
            None,
            /// <summary>
            /// Primary button, typically the left button.
            /// </summary>
            Primary,
            /// <summary>
            /// Secondary button, typically the right button.
            /// </summary>
            Secondary,
            /// <summary>
            /// Middle button on some mice.
            /// </summary>
            Middle,
            /// <summary>
            /// First Extra Button
            /// </summary>
            ExtraButton1,
            /// <summary>
            /// Second Extra Button
            /// </summary>
            ExtraButton2,
            /// <summary>
            /// Third Extra Button
            /// </summary>
            ExtraButton3,
        }

        /// <summary>
        /// Class which encapsulates the state of the mouse.
        /// </summary>
        public class MouseState
        {
            bool[] mMouseButtons = new bool[Enum.GetValues(typeof(MouseButtons)).Length];

            internal MouseState()
            {
            }

            /// <summary>
            /// Gets or sets the pressed values of the passed mouse buttons.
            /// Generates events when buttons are pressed or released.
            /// The MouseButtons enum has the FlagsAttribute, so you can make a bitwise
            /// combination of these values.  
            /// Getting the state with a combination of flags returns true if ANY one of
            /// the buttons are down.
            /// Setting the state with a combination of flags will set the state for
            /// all flags passed.
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public bool this[MouseButtons id]
            {
                get
                {
                    return mMouseButtons[(int)id];
                }
                set
                {
                    if (id == MouseButtons.None)
                    {
                        mMouseButtons[(int)id] = false;
                        return;
                    }

                    mMouseButtons[(int)id] = value;

                    if (value == true)
                        Mouse.OnMouseDown(id);
                    else
                        Mouse.OnMouseUp(id);
                }
            }
        }

        private static MouseState mState = new MouseState();
        private static bool mIsHidden = false;

        /// <summary>
        /// Gets or sets the position of the cursor, in client coordinates
        /// of the current display window.
        /// </summary>
        public static Point Position
        {
            get { return Display.CurrentWindow.MousePosition; }
            set { Display.CurrentWindow.MousePosition = value; }
        }

        /// <summary>
        /// Gets or sets the X position of the cursor, in client coordinates 
        /// of the current display window.
        /// </summary>
        public static int X
        {
            get { return Display.CurrentWindow.MousePosition.X; }
            set { Display.CurrentWindow.MousePosition = new Point(value, Position.Y); }
        }
        /// <summary>
        /// Gets or sets the Y position of the cursor, in client coordinates
        /// of the current display window.
        /// </summary>
        public static int Y
        {
            get { return Display.CurrentWindow.MousePosition.Y; }
            set { Display.CurrentWindow.MousePosition = new Point(Position.X, value); }
        }

        /// <summary>
        /// Returns true if the mouse pointer is hidden.
        /// </summary>
        public static bool IsHidden
        {
            get { return mIsHidden; }
            private set { mIsHidden = value; }
        }
        /// <summary>
        /// Gets the MouseState structure which indicates which buttons
        /// are pressed.
        /// </summary>
        public static MouseState Buttons
        {
            get { return mState; }
        }
        /// <summary>
        /// Shows the OS cursor.
        /// </summary>
        public static void Show()
        {
            Display.ShowCursor();
            IsHidden = false;
        }
        /// <summary>
        /// Hides the OS cursor.
        /// </summary>
        public static void Hide()
        {
            Display.HideCursor();
            IsHidden = true;
        }

        /// <summary>
        /// Event which occurs when the mouse is moved.
        /// </summary>
        public static event InputEventHandler MouseMove;
        /// <summary>
        /// Event which occurs when a mouse button is pressed.
        /// </summary>
        public static event InputEventHandler MouseDown;
        /// <summary>
        /// Event which occurs when a mouse button is released.
        /// </summary>
        public static event InputEventHandler MouseUp;
        /// <summary>
        /// Event which occurs when a mouse button is double-clicked.
        /// </summary>
        public static event InputEventHandler MouseDoubleClick;
        /// <summary>
        /// Event which occurs when the mouse wheel is scrolled.  Positive numbers
        /// indicate scrolling down, while negative numbers indicate scrolling up.
        /// A value of 100 means to scroll one line.
        /// </summary>
        public static event InputEventHandler MouseWheel;

        static bool inMouseMove = false;

        /// <summary>
        /// Raises the MouseWheel event.
        /// </summary>
        /// <param name="delta"></param>
        public static void OnMouseWheel(int delta)
        {
            MouseWheel(new InputEventArgs(delta));
        }

        /// <summary>
        /// Raises the MouseMove event.
        /// </summary>
        public static void OnMouseMove()
        {
            if (inMouseMove)
                return;

            inMouseMove = true;

            if (MouseMove != null)
                MouseMove(new InputEventArgs());

            // this is required, because if the mouse position is adjusted
            // a new MouseMove event will be generated.
            Core.KeepAlive();

            inMouseMove = false;

        }
        private static void OnMouseDown(MouseButtons btn)
        {
            if (MouseDown != null)
                MouseDown(new InputEventArgs(btn));
        }
        private static void OnMouseUp(MouseButtons btn)
        {
            if (MouseUp != null)
                MouseUp(new InputEventArgs(btn));
        }
        /// <summary>
        /// Raises the MouseDoubleClick event.
        /// </summary>
        /// <param name="btn"></param>
        public static void OnMouseDoubleClick(MouseButtons btn)
        {
            if (MouseDoubleClick != null)
                MouseDoubleClick(new InputEventArgs(btn));
        }



    }
}
