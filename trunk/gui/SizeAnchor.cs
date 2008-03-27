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
