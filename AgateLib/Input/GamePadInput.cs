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
using AgateLib.Quality;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AgateLib.Input
{
    /// <summary>
    /// Interface for a gamepad
    /// </summary>
    public interface IGamePad
    {
        /// <summary>
        /// Event raised when a button is pressed.
        /// </summary>
        event EventHandler<GamepadButtonEventArgs> ButtonPressed;

        /// <summary>
        /// Event raised when a button is released.
        /// </summary>
        event EventHandler<GamepadButtonEventArgs> ButtonReleased;

        /// <summary>
        /// Event raised when the left stick is moved.
        /// </summary>
        event EventHandler LeftStickChanged;

        /// <summary>
        /// Event raised when the right stick is moved.
        /// </summary>
        event EventHandler RightStickChanged;

        /// <summary>
        /// The current value of the left stick.
        /// </summary>
        Vector2 LeftStick { get; }

        /// <summary>
        /// The current value of the right stick.
        /// </summary>
        Vector2 RightStick { get; }

        /// <summary>
        /// Gets the state of the direction pad.
        /// </summary>
        GamePadDPad DPad { get; }

        /// <summary>
        /// The current state of the A button.
        /// </summary>
        bool A { get; }

        /// <summary>
        /// The current state of the B button.
        /// </summary>
        bool B { get; }

        /// <summary>
        /// The current state of the X button.
        /// </summary>
        bool X { get; }

        /// <summary>
        /// The current state of the Y button.
        /// </summary>
        bool Y { get; }

        /// <summary>
        /// The current state of the left bumper.
        /// </summary>
        bool LeftBumper { get; }

        /// <summary>
        /// The current state of the right bumper.
        /// </summary>
        bool RightBumper { get; }

        /// <summary>
        /// The current state of the back button.
        /// </summary>
        bool Back { get; }

        /// <summary>
        /// The current state of the start button.
        /// </summary>
        bool Start { get; }

        /// <summary>
        /// The current state of the left stick button.
        /// </summary>
        bool LeftStickButton { get; }

        /// <summary>
        /// The current state of the right stick button.
        /// </summary>
        bool RightStickButton { get; }

        /// <summary>
        /// The current state of the left trigger.
        /// </summary>
        float LeftTrigger { get; }

        /// <summary>
        /// The current state of the right trigger.
        /// </summary>
        float RightTrigger { get; }

        /// <summary>
        /// The index of this gamepad.
        /// </summary>
        PlayerIndex PlayerIndex { get; }
    }

    /// <summary>
    /// Class which represents a gamepad.
    /// </summary>
    public class GamePadInput : IGamePad
    {
        private static Buttons[] ButtonNames = (Buttons[])Enum.GetValues(typeof(Buttons));

        private GamePadState state;
        private KeyboardState lastKeyState;
        
        /// <summary>
        /// Constructs a Gamepad object and sets it to track a low-level joystick.
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <param name="keyMap"></param>
        /// <param name="gamePadGetState"></param>
        /// <param name="keyboardGetState"></param>
        public GamePadInput(PlayerIndex playerIndex, KeyboardGamepadMap keyMap)
        {
            PlayerIndex = playerIndex;
            KeyMap = keyMap;
        }

        public bool InvertLeftStickY { get; set; } = false;
        public bool InvertRightStickY { get; set; } = false;

        public KeyboardGamepadMap KeyMap { get; set; }

        /// <summary>
        /// Event raised when a button is pressed.
        /// </summary>
        public event EventHandler<GamepadButtonEventArgs> ButtonPressed;

        /// <summary>
        /// Event raised when a button is released.
        /// </summary>
        public event EventHandler<GamepadButtonEventArgs> ButtonReleased;

        /// <summary>
        /// Event raised when the left stick is moved.
        /// </summary>
        public event EventHandler LeftStickChanged;

        /// <summary>
        /// Event raised when the right stick is moved.
        /// </summary>
        public event EventHandler RightStickChanged;

        /// <summary>
        /// The index of this gamepad.
        /// </summary>
        public PlayerIndex PlayerIndex { get; }

        public bool IsConnected => state.IsConnected;

        /// <summary>
        /// The current value of the left stick.
        /// </summary>
        public Vector2 LeftStick => new Vector2(
            state.ThumbSticks.Left.X,
            (InvertLeftStickY ? -1 : 1) * state.ThumbSticks.Left.Y);

        /// <summary>
        /// The current value of the right stick.
        /// </summary>
        public Vector2 RightStick => new Vector2(
            state.ThumbSticks.Right.X,
            (InvertRightStickY ? -1 : 1) * state.ThumbSticks.Right.Y);

        /// <summary>
        /// The current state of the direction pad.
        /// </summary>
        public GamePadDPad DPad => state.DPad;

        /// <summary>
        /// The current state of the A button.
        /// </summary>
        public bool A => state.Buttons.A == ButtonState.Pressed;

        /// <summary>
        /// The current state of the B button.
        /// </summary>
        public bool B => state.Buttons.B == ButtonState.Pressed;

        /// <summary>
        /// The current state of the X button.
        /// </summary>
        public bool X => state.Buttons.X == ButtonState.Pressed;

        /// <summary>
        /// The current state of the Y button.
        /// </summary>
        public bool Y => state.Buttons.Y == ButtonState.Pressed;

        /// <summary>
        /// The current state of the left bumper.
        /// </summary>
        public bool LeftBumper => state.Buttons.LeftShoulder == ButtonState.Pressed;

        /// <summary>
        /// The current state of the right bumper.
        /// </summary>
        public bool RightBumper => state.Buttons.RightShoulder == ButtonState.Pressed;

        /// <summary>
        /// The current state of the back button.
        /// </summary>
        public bool Back => state.Buttons.Back == ButtonState.Pressed;

        /// <summary>
        /// The current state of the start button.
        /// </summary>
        public bool Start => state.Buttons.Start == ButtonState.Pressed;

        /// <summary>
        /// The current state of the left stick button.
        /// </summary>
        public bool LeftStickButton => state.Buttons.LeftStick == ButtonState.Pressed;

        /// <summary>
        /// The current state of the right stick button.
        /// </summary>
        public bool RightStickButton => state.Buttons.RightStick == ButtonState.Pressed;

        /// <summary>
        /// The current state of the left trigger.
        /// </summary>
        public float LeftTrigger => state.Triggers.Left;

        /// <summary>
        /// The current state of the right trigger.
        /// </summary>
        public float RightTrigger => state.Triggers.Right;

        private void OnButtonPressed(Buttons button)
        {
            ButtonPressed?.Invoke(this,
                new GamepadButtonEventArgs(button));
        }

        private void OnButtonReleased(Buttons button)
        {
            ButtonReleased?.Invoke(this,
                new GamepadButtonEventArgs(button));
        }
        
        public void Poll(IInputState inputState)
        {
            GamePadState oldState = state;
            state = inputState.GamePadStateOf(PlayerIndex);

            ApplyKeyMap(ref state, inputState);

            foreach (var button in ButtonNames)
            {
                var oldDown = oldState.IsButtonDown(button);
                var newDown = state.IsButtonDown(button);

                if (oldDown != newDown)
                {
                    if (newDown)
                        OnButtonPressed(button);
                    else
                        OnButtonReleased(button);
                }
            }
            
            if (state.ThumbSticks.Left != oldState.ThumbSticks.Left)
                LeftStickChanged?.Invoke(this, EventArgs.Empty);
            if (state.ThumbSticks.Right != oldState.ThumbSticks.Right)
                RightStickChanged?.Invoke(this, EventArgs.Empty);
        }

        // Feature is disabled because it isn't working and has questionable value.
        // If enabled, be sure to enable the unit tests.
        private void ApplyKeyMap(ref GamePadState padState, IInputState inputState)
        {
            if (KeyMap == null)
                return;

            var keys = inputState.KeyboardState;

            Buttons buttons = 0;

            foreach (var button in ButtonNames)
            {
                if (padState.IsButtonDown(button))
                {
                    buttons |= button;
                }
            }

            foreach (var key in KeyMap.Keys)
            {
                var map = KeyMap[key];

                if (keys.IsKeyDown(key))
                {
                    var item = KeyMap[key];

                    if (item.Button != 0)
                    {
                        buttons |= item.Button;
                    }
                }
            }

            padState = new GamePadState(
                padState.ThumbSticks.Left,
                padState.ThumbSticks.Right,
                padState.Triggers.Left,
                padState.Triggers.Right,
                buttons);
        }

        public bool IsButtonDown(Buttons button)
        {
            return state.IsButtonDown(button);
        }
    }
}