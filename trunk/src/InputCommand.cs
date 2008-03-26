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
    /// Class which encapsulates a set of key codes and joystick buttons that
    /// all have the same function.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class InputCommand<T> 
        // \cond doxygenignore
        where T : struct
        // \endcond
    {
        bool mPressed = false;
        bool mRequireTap = false;
        bool mRaiseEvent = true;
        bool mWaiting = false;
        List<JoystickAxis> mJoystickAxes = new List<JoystickAxis>();
        List<JoystickButton> mJoystickButtons = new List<JoystickButton>();
        List<KeyCode> mKeys = new List<KeyCode>();

        T mIdentifier;

        /// <summary>
        /// Class which represents a button press on a particular joystick.
        /// </summary>
        private class JoystickButton
        {
            public int joyID;
            public int buttonID;

            public JoystickButton(int joyID, int button)
            {
                this.joyID = joyID;
                this.buttonID = button;
            }

            public static bool operator ==(JoystickButton a, JoystickButton b)
            {
                return a.Equals(b);
            }
            public static bool operator !=(JoystickButton a, JoystickButton b)
            {
                return !a.Equals(b);
            }
            public override int GetHashCode()
            {
                return buttonID;
            }
            public override bool Equals(object obj)
            {
                if (obj is JoystickButton)
                    return Equals(obj as JoystickButton);
                else
                    return base.Equals(obj);
            }
            public bool Equals(JoystickButton btn)
            {
                if (joyID == btn.joyID && buttonID == btn.buttonID)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Class which represents a button-like on a particular joystick
        /// </summary>
        private class JoystickAxis
        {
            public int joyID;
            public int axisID;
            public bool positiveDirection;

            public JoystickAxis(int joyid, int axisid, bool positiveDir)
            {
                joyID = joyid;
                axisID = axisid;
                positiveDirection = positiveDir;
            }

            public override int GetHashCode()
            {
                return joyID * 10000 + axisID * 100 + (positiveDirection ? 1 : 0);
            }
            public override bool Equals(object obj)
            {
                if (obj is JoystickAxis)
                    return Equals(obj as JoystickAxis);
                else
                    return base.Equals(obj);
            }
            public bool Equals(JoystickAxis axis)
            {
                if (axisID == axis.axisID &&
                    joyID == axis.joyID &&
                    positiveDirection == axis.positiveDirection)

                    return true;
                else
                    return false;
            }
            public static bool operator ==(JoystickAxis a, JoystickAxis b)
            {
                return a.Equals(b);
            }
            public static bool operator !=(JoystickAxis a, JoystickAxis b)
            {
                return !a.Equals(b);
            }
        }
        /// <summary>
        /// Updates the state of this input command.
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < mKeys.Count; i++)
            {
                if (Keyboard.Keys[mKeys[i]])
                {
                    SetOn();
                    return;
                }
            }

            for (int i = 0; i < mJoystickButtons.Count; i++)
            {
                JoystickButton btn = mJoystickButtons[i];

                if (Input.JoystickCount <= btn.joyID)
                    continue;

                if (Input.Joysticks[btn.joyID].Buttons[btn.buttonID])
                {
                    SetOn();
                    return;
                }
            }

            for (int i = 0; i < mJoystickAxes.Count; i++)
            {
                JoystickAxis axis = mJoystickAxes[i];

                if (Input.JoystickCount <= axis.joyID)
                    continue;

                double value = Input.Joysticks[axis.joyID].GetAxisValue(axis.axisID);

                if (axis.positiveDirection)
                {
                    if (value > 0.5)
                    {
                        SetOn();
                        return;
                    }
                }
                else
                {
                    if (value < -0.5)
                    {
                        SetOn();
                        return;
                    }
                }
            }

            Clear(false);
        }

        /// <summary>
        /// Adds a key to the list of buttons which will trigger this command.
        /// </summary>
        /// <param name="c"></param>
        public void AddKey(KeyCode c)
        {
            if (mKeys.Contains(c) == false)
            {
                mKeys.Add(c);
            }
        }
        /// <summary>
        /// Removes a key from the list of buttons which will trigger this command.
        /// </summary>
        /// <param name="c"></param>
        public void RemoveKey(KeyCode c)
        {
            mKeys.Remove(c);
        }

        /// <summary>
        /// Adds a joystick button to the list of buttons which will trigger this command.
        /// </summary>
        /// <param name="joystickIndex"></param>
        /// <param name="button"></param>
        public void AddJoystickButton(int joystickIndex, int button)
        {
            JoystickButton btn = new JoystickButton(joystickIndex, button);

            if (mJoystickButtons.Contains(btn) == false)
                mJoystickButtons.Add(btn);
        }
        /// <summary>
        /// Removes a joystick button from the list of buttons which trigger this command.
        /// </summary>
        /// <param name="joystickIndex"></param>
        /// <param name="button"></param>
        public void RemoveJoystickButton(int joystickIndex, int button)
        {
            for (int i = 0; i < mJoystickButtons.Count; i++)
            {
                if (mJoystickButtons[i].joyID == joystickIndex && mJoystickButtons[i].buttonID == button)
                {
                    mJoystickButtons.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Adds a joystick axis to the list of buttons which will trigger this command.
        /// This treats the axis as if it were a gamepad, only responding of the axis moves
        /// pas a value of 0.5.
        /// </summary>
        /// <param name="joystickIndex">The index of the joystick, in Input.Joysticks.</param>
        /// <param name="axisIndex">The index of the axis.  0 is x-axis, 1 is y-axis.</param>
        /// <param name="positiveDirection">True if the command responds to the positive
        /// direction.  For example, with the x-axis, if this is true 
        /// the command will respond to pushing the joystick to the right.</param>
        public void AddJoystickAxis(int joystickIndex, int axisIndex, bool positiveDirection)
        {
            JoystickAxis axis = new JoystickAxis(joystickIndex, axisIndex, positiveDirection);

            if (mJoystickAxes.Contains(axis) == false)
                mJoystickAxes.Add(axis);
        }
        /// <summary>
        /// Sets this command as being on, as if a key or button listed in it was pressed.
        /// </summary>
        public void SetOn()
        {
            if (mRequireTap && mWaiting)
                return;

            mPressed = true;

            if (mRaiseEvent)
            {
                if (InputCommandActivate != null)
                    InputCommandActivate(mIdentifier);

            }
        }
        /// <summary>
        /// Clears the setting on this command, as if all the keys and joystick buttons
        /// associated with this command were released.
        /// </summary>
        /// <param name="waitForTap">true indicates that this should not fire events or register
        /// as on until the keys associated with it have been released.</param>
        public void Clear(bool waitForTap)
        {
            mWaiting = waitForTap;
            mPressed = false;
        }
        /// <summary>
        /// Set to true to indicate that this command should not fire another event
        /// until the key is released and pressed again.
        /// </summary>
        public bool RequireTap
        {
            get { return mRequireTap; }
            set
            {
                if (value == false)
                {
                    mRequireTap = false;
                    mWaiting = false;
                }
                else
                    mRequireTap = true;
            }
        }
        /// <summary>
        /// Set to true to indicate that this command should fire events when the buttons
        /// are pressed (and held, according to the RequireTap setting).
        /// </summary>
        public bool RaiseEvent
        {
            get { return mRaiseEvent; }
            set { mRaiseEvent = value; }
        }

        internal T Identifier
        {
            get { return mIdentifier; }
            set { mIdentifier = value; }
        }

        /// <summary>
        /// Gets or sets whether this command is "pusshed."
        /// </summary>
        public bool Value
        {
            get { return mPressed; }
            set
            {
                if (value)
                    SetOn();
                else
                    Clear(mRequireTap);
            }
        }


        /// <summary>
        /// Delegate for activation of this command.
        /// </summary>
        /// <param name="commandIdentifier"></param>
        public delegate void InputCommandActivateDelegate(T commandIdentifier);

        /// <summary>
        /// Event which fires when this command has been activated, by the user pressing
        /// an associated button, or calling the SetOn() method.
        /// </summary>
        public event InputCommandActivateDelegate InputCommandActivate;

    }
}