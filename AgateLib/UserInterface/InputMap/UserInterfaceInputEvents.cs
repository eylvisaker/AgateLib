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
using System.Linq;

namespace AgateLib.UserInterface.InputMap
{
    public class UserInterfaceInputEvents
    {
        private UserInterfaceInputState inputState = new UserInterfaceInputState();
        private UserInterfaceInputMap inputMap = new UserInterfaceInputMap();
        private HashSet<UserInterfaceAction> activeNavActions = new HashSet<UserInterfaceAction>();

        private UserInterfaceActionEventArgs eventArgs = new UserInterfaceActionEventArgs();
        private ButtonStateEventArgs buttonStateEventArgs = new ButtonStateEventArgs();

        private List<UserInterfaceAction> actionCache = new List<UserInterfaceAction>();

        private const float timeToFirstRepeat = 0.5f;
        private const float timeToNextRepeat = 0.1f;

        private bool repeating = false;
        private float timeSinceRepeat = 0;

        public UserInterfaceInputEvents()
        {
            buttonStateEventArgs.InputState = inputState;

        }

        public event Action<UserInterfaceActionEventArgs> UIAction;

        public event Action<ButtonStateEventArgs> ButtonDown;
        public event Action<ButtonStateEventArgs> ButtonUp;

        public UserInterfaceInputMap InputMap
        {
            get => inputMap;
            set => inputMap = value ?? throw new ArgumentNullException(nameof(InputMap));
        }

        public bool AnyNavigateActionHeld
            => inputState.HeldActions.Contains(UserInterfaceAction.Up)
            || inputState.HeldActions.Contains(UserInterfaceAction.Down)
            || inputState.HeldActions.Contains(UserInterfaceAction.Left)
            || inputState.HeldActions.Contains(UserInterfaceAction.Right);

        public void UpdateState(IInputState input)
        {
            inputMap.UpdateInputState(inputState, PlayerIndex.One, input);
        }

        public void IgnoreCurrentInput(IInputState input)
        {
            inputMap.IgnoreCurrentInput(inputState, PlayerIndex.One, input);
        }

        public void IgnoreCurrentInput(IInputState input, UserInterfaceAction action)
        {
            inputMap.IgnoreCurrentInput(inputState, PlayerIndex.One, input, action);
        }

        public void TriggerEvents(GameTime time)
        {
            foreach (UserInterfaceAction action in inputState.PressedActions)
            {
                TriggerPressedAction(action);
                timeSinceRepeat = 0;
            }

            if (inputState.HeldActions.Any())
            {
                float timeToRepeat = repeating ? timeToNextRepeat : timeToFirstRepeat;

                timeSinceRepeat += (float)time.ElapsedGameTime.TotalSeconds;

                if (timeSinceRepeat > timeToRepeat)
                {
                    timeSinceRepeat = 0;
                    repeating = true;

                    foreach (UserInterfaceAction action in inputState.HeldActions)
                    {
                        TriggerPressedAction(action);
                    }
                }
            }
            else
            {
                repeating = false;
            }

            actionCache.Clear();
            actionCache.AddRange(inputState.ReleasedActions);

            foreach (UserInterfaceAction action in actionCache)
            {
                TriggerReleasedAction(action);
            }

            inputState.ActionsProcessed();
        }

        private void TriggerPressedAction(UserInterfaceAction action)
        {
            if (timeSinceRepeat > 0 && activeNavActions.Contains(action))
                return;

            TriggerButtonDown(action);

            switch (action)
            {
                case UserInterfaceAction.Up:
                case UserInterfaceAction.Down:
                case UserInterfaceAction.Left:
                case UserInterfaceAction.Right:
                    TriggerActionEvent(action);
                    activeNavActions.Add(action);
                    break;
            }
        }

        private void TriggerReleasedAction(UserInterfaceAction action)
        {
            TriggerButtonUp(action);

            switch (action)
            {
                case UserInterfaceAction.Accept:
                case UserInterfaceAction.Cancel:
                case UserInterfaceAction.Exit:
                case UserInterfaceAction.PageUp:
                case UserInterfaceAction.PageDown:
                    TriggerActionEvent(action);
                    break;

                default:
                    activeNavActions.Remove(action);
                    break;
            }
        }

        private void TriggerButtonDown(UserInterfaceAction action)
        {
            buttonStateEventArgs.ActionButton = action;
            buttonStateEventArgs.Handled = false;

            ButtonDown?.Invoke(buttonStateEventArgs);
        }

        private void TriggerButtonUp(UserInterfaceAction action)
        {
            buttonStateEventArgs.ActionButton = action;
            buttonStateEventArgs.Handled = false;

            ButtonUp?.Invoke(buttonStateEventArgs);
        }

        private void TriggerActionEvent(UserInterfaceAction action)
        {
            eventArgs.Reset(action);

            UIAction?.Invoke(eventArgs);
        }

        internal void ClearPressedButtons()
        {
            inputState.ClearEverything();
        }
    }
}
