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
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgateLib.UserInterface.InputMap
{
    public class UserInterfaceInputMap
    {
        #region --- Static Members ---

        /// <summary>
        /// Gets or sets the default button map.
        /// </summary>
        public static IReadOnlyDictionary<Buttons, UserInterfaceAction> DefaultButtonMap { get; set; }
            = new Dictionary<Buttons, UserInterfaceAction>
            {
                [Buttons.A] = UserInterfaceAction.Accept,
                [Buttons.B] = UserInterfaceAction.Cancel,
                [Buttons.Back] = UserInterfaceAction.Exit,

                [Buttons.LeftThumbstickUp] = UserInterfaceAction.Up,
                [Buttons.LeftThumbstickDown] = UserInterfaceAction.Down,
                [Buttons.LeftThumbstickLeft] = UserInterfaceAction.Left,
                [Buttons.LeftThumbstickRight] = UserInterfaceAction.Right,

                [Buttons.DPadUp] = UserInterfaceAction.Up,
                [Buttons.DPadDown] = UserInterfaceAction.Down,
                [Buttons.DPadLeft] = UserInterfaceAction.Left,
                [Buttons.DPadRight] = UserInterfaceAction.Right,

                [Buttons.LeftShoulder] = UserInterfaceAction.PageUp,
                [Buttons.RightShoulder] = UserInterfaceAction.PageDown,
            };

        /// <summary>
        /// Gets or sets the default key map for keyboards.
        /// </summary>
        public static IReadOnlyDictionary<Keys, UserInterfaceAction> DefaultKeyMap { get; set; }
            = new Dictionary<Keys, UserInterfaceAction>
            {
                [Keys.Enter] = UserInterfaceAction.Accept,
                [Keys.Space] = UserInterfaceAction.Accept,
                [Keys.Back] = UserInterfaceAction.Cancel,
                [Keys.LeftAlt] = UserInterfaceAction.Cancel,
                [Keys.Z] = UserInterfaceAction.Cancel,
                [Keys.Escape] = UserInterfaceAction.Exit,

                [Keys.Up] = UserInterfaceAction.Up,
                [Keys.Left] = UserInterfaceAction.Left,
                [Keys.Right] = UserInterfaceAction.Right,
                [Keys.Down] = UserInterfaceAction.Down,

                [Keys.PageDown] = UserInterfaceAction.PageDown,
                [Keys.PageUp] = UserInterfaceAction.PageUp,
            };

        #endregion

        private Dictionary<Buttons, UserInterfaceAction> buttonMap = new Dictionary<Buttons, UserInterfaceAction>();
        private Dictionary<Keys, UserInterfaceAction> keyMap = new Dictionary<Keys, UserInterfaceAction>();

        public UserInterfaceInputMap(IReadOnlyDictionary<Buttons, UserInterfaceAction> buttonMap = null,
                                     IReadOnlyDictionary<Keys, UserInterfaceAction> keyMap = null)
        {
            this.buttonMap = (buttonMap ?? DefaultButtonMap).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            this.keyMap = (keyMap ?? DefaultKeyMap).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public Dictionary<Buttons, UserInterfaceAction> ButtonMap
        {
            get => buttonMap;
            set => buttonMap = value ?? throw new ArgumentNullException(nameof(ButtonMap));
        }

        public Dictionary<Keys, UserInterfaceAction> KeyMap
        {
            get => keyMap;
            set => keyMap = value ?? throw new ArgumentNullException(nameof(KeyMap));
        }

        public UserInterfaceInputMap Clone()
        {
            return new UserInterfaceInputMap
            {
                buttonMap = new Dictionary<Buttons, UserInterfaceAction>(buttonMap),
                keyMap = new Dictionary<Keys, UserInterfaceAction>(keyMap)
            };
        }

        public void UpdateInputState(UserInterfaceInputState inputState, 
                                     PlayerIndex playerIndex, 
                                     IInputState input)
        {
            GamePadState gamePad = input.GamePadStateOf(playerIndex);

            foreach (var button in buttonMap.Keys)
            {
                inputState.UpdateGamePadButtonState(button, buttonMap[button], gamePad.IsButtonDown(button));
            }

            KeyboardState keyboard = input.KeyboardState;

            foreach (var key in keyMap.Keys)
            {
                inputState.UpdateKeyState(key, keyMap[key], keyboard.IsKeyDown(key));
            }
        }

        public void IgnoreCurrentInput(UserInterfaceInputState inputState, 
                                       PlayerIndex playerIndex, 
                                       IInputState input)
        {
            GamePadState gamePad = input.GamePadStateOf(playerIndex);
            KeyboardState keyboard = input.KeyboardState;

            foreach (var button in buttonMap.Keys)
            {
                if (gamePad.IsButtonDown(button))
                    inputState.IgnoreNextButtonPress(button);
            }

            foreach (var key in keyMap.Keys)
            {
                if (keyboard.IsKeyDown(key))
                    inputState.IgnoreNextKeyPress(key);
            }
        }
    }
}
