using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib.Geometry;

namespace ERY.AgateLib.Gui
{
    public class SizeAnchor : Component
    {
        private bool mMouseIn = false;
        private Point mAnchor;
        private Size mParentOriginalSize;

        public SizeAnchor(Container parent)
            : base(parent, false)
        {
            AttachEvents();

            Size = new Size(16, 16);
        }

        private void AttachEvents()
        {
            this.MouseDown += new GuiInputEventHandler(SizeAnchor_MouseDown);
            this.MouseUp += new GuiInputEventHandler(SizeAnchor_MouseUp);
            this.MouseMove += new GuiInputEventHandler(SizeAnchor_MouseMove);
        }

        void SizeAnchor_MouseMove(object sender, InputEventArgs e)
        {
            if (mMouseIn == false)
                return;

            Point diff = new Point(
                e.MousePosition.X - mAnchor.X,
                e.MousePosition.Y - mAnchor.Y);

            Parent.Size = new Size(mParentOriginalSize.Width + diff.X,
                                   mParentOriginalSize.Height + diff.Y);

            if (Parent is Window)
                (Parent as Window).SkipSizeTransition();
        }
        void SizeAnchor_MouseUp(object sender, InputEventArgs e)
        {
            mMouseIn = false;
        }
        void SizeAnchor_MouseDown(object sender, InputEventArgs e)
        {
            mAnchor = e.MousePosition;
            mParentOriginalSize = Parent.Size;

            mMouseIn = true;
        }

        protected internal override bool ConnectToStyle
        {
            get { return false; }
        }
    }
}
