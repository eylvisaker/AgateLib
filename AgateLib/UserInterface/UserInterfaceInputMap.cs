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

using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.Input;
using AgateLib.UserInterface.Widgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AgateLib.UserInterface
{
    public interface IUserInterfaceInputMap
    {
        void UpdateInputState(MenuInputState inputState, PlayerIndex playerIndex, IInputState input);
    }

    public class UserInterfaceInputMap : IUserInterfaceInputMap
    {
        #region --- Static Members ---

        private static UserInterfaceInputMap defaultMap;

        /// <summary>
        /// Gets or sets the default input map.
        /// </summary>
        public static UserInterfaceInputMap Default
        {
            get
            {
                if (defaultMap == null)
                {
                    defaultMap = new UserInterfaceInputMap
                    {
                        ButtonMap = new Dictionary<Buttons, MenuInputButton>
                        {
                            [Buttons.A] = MenuInputButton.Accept,
                            [Buttons.B] = MenuInputButton.Cancel,
                            [Buttons.Start] = MenuInputButton.Accept,
                            [Buttons.Back] = MenuInputButton.Exit,

                            [Buttons.LeftThumbstickUp] = MenuInputButton.Up,
                            [Buttons.LeftThumbstickDown] = MenuInputButton.Down,
                            [Buttons.LeftThumbstickLeft] = MenuInputButton.Left,
                            [Buttons.LeftThumbstickRight] = MenuInputButton.Right,

                            [Buttons.DPadUp] = MenuInputButton.Up,
                            [Buttons.DPadDown] = MenuInputButton.Down,
                            [Buttons.DPadLeft] = MenuInputButton.Left,
                            [Buttons.DPadRight] = MenuInputButton.Right,

                            [Buttons.LeftShoulder] = MenuInputButton.PageUp,
                            [Buttons.RightShoulder] = MenuInputButton.PageDown,
                        },

                        KeyMap = new Dictionary<Keys, MenuInputButton>
                        {
                            [Keys.Enter] = MenuInputButton.Accept,
                            [Keys.Space] = MenuInputButton.Accept,
                            [Keys.Back] = MenuInputButton.Cancel,
                            [Keys.Z] = MenuInputButton.Cancel,
                            [Keys.Escape] = MenuInputButton.Exit,

                            [Keys.Up] = MenuInputButton.Up,
                            [Keys.Left] = MenuInputButton.Left,
                            [Keys.Right] = MenuInputButton.Right,
                            [Keys.Down] = MenuInputButton.Down,

                            [Keys.PageDown] = MenuInputButton.PageDown,
                            [Keys.PageUp] = MenuInputButton.PageUp,
                        },
                    };
                }

                return defaultMap;
            }
            set
            {
                defaultMap = value ?? throw new ArgumentNullException(nameof(Default));
            }
        }


        #endregion

        Dictionary<Buttons, MenuInputButton> buttonMap = new Dictionary<Buttons, MenuInputButton>();
        Dictionary<Keys, MenuInputButton> keyMap = new Dictionary<Keys, MenuInputButton>();

        public Dictionary<Buttons, MenuInputButton> ButtonMap
        {
            get => buttonMap;
            set => buttonMap = value ?? throw new ArgumentNullException(nameof(ButtonMap));
        }

        public Dictionary<Keys, MenuInputButton> KeyMap
        {
            get => keyMap;
            set => keyMap = value ?? throw new ArgumentNullException(nameof(ButtonMap));
        }

        public UserInterfaceInputMap Clone()
        {
            return new UserInterfaceInputMap
            {
                buttonMap = new Dictionary<Buttons, MenuInputButton>(buttonMap),
                keyMap = new Dictionary<Keys, MenuInputButton>(keyMap)
            };
        }

        public void UpdateInputState(MenuInputState inputState, PlayerIndex playerIndex, IInputState input)
        {
            inputState.Clear();

            var gamePad = input.GamePadStateOf(playerIndex);

            foreach (var button in buttonMap.Keys)
            {
                if (gamePad.IsButtonDown(button))
                    inputState[buttonMap[button]] = true;
            }

            var keyboard = input.KeyboardState;

            foreach (var key in keyMap.Keys)
            {
                if (keyboard.IsKeyDown(key))
                    inputState[keyMap[key]] = true;
            }
        }

    }
}
