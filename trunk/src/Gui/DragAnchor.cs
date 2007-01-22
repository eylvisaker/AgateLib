using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib.Geometry;
using ERY.AgateLib.GuiBase;

namespace ERY.AgateLib.Gui
{
    public class DragAnchor : Component
    {
        private bool mMouseIn = false;
        private Point mAnchor;
        private Point mParentOriginalLocation;

        public DragAnchor(Container parent)
            : base(parent, false)
        {
            AttachEvents();
        }

        private void AttachEvents()
        {
            this.MouseDown += new GuiInputEventHandler(DragAnchor_MouseDown);
            this.MouseUp += new GuiInputEventHandler(DragAnchor_MouseUp);
            this.MouseMove += new GuiInputEventHandler(DragAnchor_MouseMove);
        }

        void DragAnchor_MouseMove(object sender, InputEventArgs e)
        {
            if (mMouseIn == false)
                return;

            Point diff = new Point(
                e.MousePosition.X - mAnchor.X,
                e.MousePosition.Y - mAnchor.Y);

            Parent.Location = new Point(mParentOriginalLocation.X + diff.X,
                                        mParentOriginalLocation.Y + diff.Y);
        }
        void DragAnchor_MouseUp(object sender, InputEventArgs e)
        {
            mMouseIn = false;
        }
        void DragAnchor_MouseDown(object sender, InputEventArgs e)
        {
            mAnchor = e.MousePosition;
            mParentOriginalLocation = Parent.Location;

            mMouseIn = true;
        }

        protected internal override bool ConnectToStyle
        {
            get { return false; }
        }
    }
}
