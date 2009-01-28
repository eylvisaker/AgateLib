using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Gui
{
    public class Window : Container
    {
        public new Point Location
        {
            get { return base.Location; }
            set { base.Location = value; }
        }
        public new Size Size
        {
            get { return base.Size; }
            set { base.Size = value; }
        }

        public Window()
        {
            Name = "window";

            Location = new Point(20, 20);
            Size = new Size(300, 250);
        }
        public Window(string title)
            : this()
        {
            Text = title;
            Name = title;
        }

        protected override void OnParentChanged()
        {
            if (Parent != null && Parent is GuiRoot == false)
            {
                Parent = null;
                throw new InvalidOperationException("Cannot add a window to anything other than a GuiRoot object.");
            }

            base.OnParentChanged();
        }

        public bool AllowDrag { get; set; }

        bool dragging;
        Point mouseDiff;
        protected internal override void OnMouseDown(AgateLib.InputLib.InputEventArgs e)
        {
            if (AllowDrag)
            {
                dragging = true;
                mouseDiff = new Point(
                    ScreenLocation.X - e.MousePosition.X,
                    ScreenLocation.Y - e.MousePosition.Y);
            }
        }

        protected internal override void OnMouseUp(AgateLib.InputLib.InputEventArgs e)
        {
            dragging = false;
        }

        protected internal override void OnMouseMove(AgateLib.InputLib.InputEventArgs e)
        {
            if (dragging == false)
                return;

            Point newPos = new Point(e.MousePosition.X + mouseDiff.X, e.MousePosition.Y + mouseDiff.Y);

            if (Parent.Width - this.Width < newPos.X) 
                newPos.X = Parent.Width - this.Width;
            if (Parent.Height - this.Height < newPos.Y)
                newPos.Y = Parent.Height - this.Height;
            if (newPos.X < 0) newPos.X = 0;
            if (newPos.Y < 0) newPos.Y = 0;

            Location = Parent.PointToClient(newPos);
        }
    }
}
