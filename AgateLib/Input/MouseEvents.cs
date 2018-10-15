using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace AgateLib.Input
{
    public interface IMouseEvents
    {
        event EventHandler<MouseButtonEventArgs> MouseDown;
        event EventHandler<MouseButtonEventArgs> MouseUp;
        event EventHandler<MouseEventArgs> MouseMove;

        void Update(GameTime time);
    }

    public class MouseEvents : IMouseEvents
    {
        private MouseEventArgs eventArgs = new MouseEventArgs(new Point(0, 0));
        private MouseButtonEventArgs buttonEventArgs = new MouseButtonEventArgs(new Point(0, 0), MouseButton.Left);
        private MouseState lastMouseState;
        private MouseState mouseState;

        private readonly GraphicsDevice device;
        private readonly GameWindow window;

        public event EventHandler<MouseButtonEventArgs> MouseDown;
        public event EventHandler<MouseButtonEventArgs> MouseUp;
        public event EventHandler<MouseEventArgs> MouseMove;

        public MouseEvents(GraphicsDevice device, GameWindow window)
        {
            this.device = device;
            this.window = window;
        }

        public void Update(GameTime time)
        {
            lastMouseState = mouseState;
            mouseState = Mouse.GetState();

            eventArgs.MousePosition = ConstrainMousePosition(mouseState.Position);
            buttonEventArgs.MousePosition = ConstrainMousePosition(mouseState.Position);

            CheckMouseMove();

            CheckButtonState(MouseButton.Left, mouseState.LeftButton, lastMouseState.LeftButton);
            CheckButtonState(MouseButton.Right, mouseState.RightButton, lastMouseState.RightButton);
            CheckButtonState(MouseButton.Middle, mouseState.MiddleButton, lastMouseState.MiddleButton);
        }

        private void CheckMouseMove()
        {
            if (mouseState.Position == lastMouseState.Position)
                return;

            if (!IsInWindow(lastMouseState.Position) && !IsInWindow(mouseState.Position))
                return;

            Point lastPosition = ConstrainMousePosition(lastMouseState.Position);
            Point currentPosition = ConstrainMousePosition(mouseState.Position);

            if (currentPosition != lastPosition)
            {
                MouseMove?.Invoke(this, eventArgs);
            }
        }

        private bool IsInWindow(Point position)
        {
            Rectangle localBounds = window.ClientBounds;
            localBounds.Location = Point.Zero;

            return localBounds.Contains(position);
        }

        private Point ConstrainMousePosition(Point position)
        {
            Point result = position;

            if (result.X > window.ClientBounds.Width)
            {
                result.X = window.ClientBounds.Width;
            }
            if (result.Y > window.ClientBounds.Height)
            {
                result.Y = window.ClientBounds.Height;
            }
            if (result.X < 0) result.X = 0;
            if (result.Y < 0) result.Y = 0;

            return result;
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
