//
//    Copyright (c) 2006-2018 Erik Ylvisaker
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

using AgateLib.Input;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace AgateLib.UserInterface.InputMap
{
    public class UserInterfaceInputState
    {
        private HashSet<Buttons> pressedButtons = new HashSet<Buttons>();
        private HashSet<Keys> pressedKeys = new HashSet<Keys>();

        private HashSet<UserInterfaceAction> pressedActions = new HashSet<UserInterfaceAction>();
        private HashSet<UserInterfaceAction> releasedActions = new HashSet<UserInterfaceAction>();

        private HashSet<Buttons> ignoredButtons = new HashSet<Buttons>();
        private HashSet<Keys> ignoredKeys = new HashSet<Keys>();

        public void IgnoreNextButtonPress(Buttons button)
        {
            ignoredButtons.Add(button);
        }

        public void IgnoreNextKeyPress(Keys key)
        {
            ignoredKeys.Add(key);
        }

        public void UpdateGamePadButtonState(Buttons button, UserInterfaceAction action, bool value)
        {
            if (value)
            {
                if (!ignoredButtons.Contains(button))
                {
                    pressedButtons.Add(button);
                    pressedActions.Add(action);
                }
            }
            if (!value)
            {
                ignoredButtons.Remove(button);

                if (pressedButtons.Contains(button))
                {
                    pressedButtons.Remove(button);
                    pressedActions.Remove(action);
                    releasedActions.Add(action);
                }
            }
        }

        public void UpdateKeyState(Keys key, UserInterfaceAction action, bool value)
        {
            if (value)
            {
                if (!ignoredKeys.Contains(key))
                {
                    pressedKeys.Add(key);
                    pressedActions.Add(action);
                }
            }
            if (!value)
            {
                ignoredKeys.Remove(key);

                if (pressedKeys.Contains(key))
                {
                    pressedKeys.Remove(key);

                    pressedActions.Remove(action);
                    releasedActions.Add(action);
                }
            }
        }

        public ICollection<UserInterfaceAction> PressedActions => pressedActions;

        public ICollection<UserInterfaceAction> ReleasedActions => releasedActions;

        public void Clear()
        {
            ClearPressedActions();
            ClearReleasedActions();
        }

        public void ClearReleasedActions() => releasedActions.Clear();

        private void ClearPressedActions() => pressedActions.Clear();
    }
}