//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib.Geometry;

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
