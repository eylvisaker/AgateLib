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
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{
    public class UserInterfaceInputEvents
    {
        private MenuInputState inputState = new MenuInputState();
        private HashSet<MenuInputButton> downEventFired = new HashSet<MenuInputButton>();
        private HashSet<MenuInputButton> upEventsNeeded = new HashSet<MenuInputButton>();
        private HashSet<MenuInputButton> ignoredDownButtons = new HashSet<MenuInputButton>();
        private IUserInterfaceInputMap inputMap = UserInterfaceInputMap.Default;

        public UserInterfaceInputEvents()
        {

        }

        public IUserInterfaceInputMap InputMap
        {
            get => inputMap;
            set => inputMap = value ?? throw new ArgumentNullException(nameof(InputMap));
        }

        public MenuInputState InputState
        {
            get => inputState;
            set
            {
                inputState = value;

                ClearPressedButtons();
            }
        }

        public event Action<MenuInputButton> ButtonDown;
        public event Action<MenuInputButton> ButtonUp;

        internal void UpdateState(IInputState input)
        {
            inputMap.UpdateInputState(inputState, PlayerIndex.One, input);
        }

        public void TriggerEvents()
        {
            foreach (var pressedButton in inputState.PressedButtons)
            {
                if (downEventFired.Contains(pressedButton))
                    continue;
                if (ignoredDownButtons.Contains(pressedButton))
                    continue;

                downEventFired.Add(pressedButton);

                ButtonDown?.Invoke(pressedButton);
            }

            upEventsNeeded.Clear();

            foreach (var downEventButton in downEventFired)
            {
                if (!inputState[downEventButton])
                    upEventsNeeded.Add(downEventButton);
            }

            foreach (var upEvent in upEventsNeeded)
            {
                downEventFired.Remove(upEvent);
                ignoredDownButtons.Remove(upEvent);

                ButtonUp?.Invoke(upEvent);
            }

            ignoredDownButtons.RemoveWhere(ignoredDownButton => !inputState[ignoredDownButton]);
        }

        internal void ClearPressedButtons()
        {
            foreach (var downEvent in inputState.PressedButtons)
            {
                ignoredDownButtons.Add(downEvent);
            }

            downEventFired.Clear();
        }

    }
}
