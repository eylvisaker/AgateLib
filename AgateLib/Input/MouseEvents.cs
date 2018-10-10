using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace AgateLib.Input
{
    public class MouseEvents
    {
        private MouseEventArgs eventArgs = new MouseEventArgs(new Point(0, 0));
        private MouseButtonEventArgs buttonEventArgs = new MouseButtonEventArgs(new Point(0, 0), MouseButton.Left);
        private MouseState lastMouseState;
        private MouseState mouseState;

        public event EventHandler<MouseButtonEventArgs> MouseDown;
        public event EventHandler<MouseButtonEventArgs> MouseUp;
        public event EventHandler<MouseEventArgs> MouseMove;

        public void Update(GameTime time)
        {
            lastMouseState = mouseState;
            mouseState = Mouse.GetState();

            buttonEventArgs.MousePosition = mouseState.Position;

            CheckMouseMove();
            CheckButtonState(MouseButton.Left, mouseState.LeftButton, lastMouseState.LeftButton);
            CheckButtonState(MouseButton.Right, mouseState.RightButton, lastMouseState.RightButton);
            CheckButtonState(MouseButton.Middle, mouseState.MiddleButton, lastMouseState.MiddleButton);
        }

        private void CheckMouseMove()
        {
            if (mouseState.Position != lastMouseState.Position)
            {
                MouseMove?.Invoke(this, eventArgs);
            }
        }

        private void CheckButtonState(MouseButton button, ButtonState newState, ButtonState oldState)
        {
            if (newState != oldState)
            {
                buttonEventArgs.Button = button;

                if (newState == ButtonState.Pressed)
                {
                    MouseDown?.Invoke(this, buttonEventArgs);
                }
                else
                {
                    MouseUp?.Invoke(this, buttonEventArgs);
                }
            }
        }
    }

    public class MouseEventArgs : EventArgs
    {
        public MouseEventArgs(Point mousePosition)
        {
            MousePosition = mousePosition;
        }

        public Point MousePosition { get; protected internal set; }
    }

    public class MouseButtonEventArgs : MouseEventArgs
    {
        public MouseButtonEventArgs(Point mousePosition, MouseButton button)
            : base(mousePosition)
        {
            Button = button;
        }

        public MouseButton Button { get; protected internal set; }
    }

    public enum MouseButton
    {
        Left,
        Middle,
        Right,
    }
}
