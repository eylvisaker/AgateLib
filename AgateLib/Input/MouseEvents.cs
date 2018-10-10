using Microsoft.Xna.Framework;
using System;

namespace AgateLib.Input
{
    public class MouseEvents
    {
        public event EventHandler<MouseButtonEventArgs> MouseDown;
        public event EventHandler<MouseButtonEventArgs> MouseUp;
        public event EventHandler<MouseEventArgs> MouseMove;
    }

    public class MouseEventArgs : EventArgs
    {
        public MouseEventArgs(Point mousePosition)
        {
            MousePosition = mousePosition;
        }

        public Point MousePosition { get; }
    }

    public class MouseButtonEventArgs : MouseEventArgs
    {
        public MouseButtonEventArgs(Point mousePosition, int button)
            : base(mousePosition)
        {
            Button = button;
        }

        public int Button { get; }
    }
}
