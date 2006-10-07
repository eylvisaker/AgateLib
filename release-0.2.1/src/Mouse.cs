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

namespace ERY.AgateLib
{
    /// <summary>
    /// Class which encapsulates input from the mouse.
    /// </summary>
    public static class Mouse
    {
        /// <summary>
        /// Mouse Buttons enum.
        /// </summary>
        [Flags]
        public enum MouseButtons
        {
            /// <summary>
            /// No mouse buttons.
            /// </summary>
            None = 0x00,

            /// <summary>
            /// Primary button, typically the left button.
            /// </summary>
            Primary = 0x01,
            /// <summary>
            /// Secondary button, typically the right button.
            /// </summary>
            Secondary = 0x02,
            /// <summary>
            /// Middle button on some mice.
            /// </summary>
            Middle = 0x04,
            /// <summary>
            /// Extra Button
            /// </summary>
            ExtraButton1 = 0x08,
            /// <summary>
            /// Extra Button
            /// </summary>
            ExtraButton2 = 0x10,
            /// <summary>
            /// Extra Button
            /// </summary>
            ExtraButton3 = 0x11,
        }

        /// <summary>
        /// Class which encapsulates the state of the mouse.
        /// </summary>
        public class MouseState
        {
            private static MouseButtons mMouseButtons = 0;

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
                    return ((mMouseButtons & id) != 0) ? true : false;
                }
                set
                {
                    if (value == true)
                    {
                        // set the flags passed
                        mMouseButtons |= id;

                        Mouse.OnMouseDown(id);
                    }
                    else
                    {
                        // clear the flags passed
                        mMouseButtons = ~((~mMouseButtons) & id);


                        Mouse.OnMouseUp(id);
                    }


                }
            }
        }

        private static MouseState mState = new MouseState();

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
            System.Windows.Forms.Cursor.Show();
        }
        /// <summary>
        /// Hides the OS cursor.
        /// </summary>
        public static void Hide()
        {
            System.Windows.Forms.Cursor.Hide();
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
        public static event InputEventHandler MouseDoubleClickEvent;

        static bool inMouseMove = false;

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
            if (MouseDoubleClickEvent != null)
                MouseDoubleClickEvent(new InputEventArgs(btn));
        }


    }
}
