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

using AgateLib.Display;
using AgateLib.Geometry;

namespace AgateLib.Gui
{
    public class Window : Container
    {
        private string mTitle;
        private Size mTargetSize;
        private SizeF mInternalSize;
        private const double sizeVelocity = 200;

        public Window()
        {
            Location = new Point(10,10);

            mInternalSize = new SizeF(Width, Height);
            mTargetSize = Size;
        }

        public Window(Container parent, Rectangle bounds, string title)
            : base(parent, bounds)
        {
            mTitle = title;

            mStyle.UpdateClientArea();

            SkipSizeTransition();
        }


        public string Title
        {
            get { return mTitle; }
            set { mTitle = value; }
        }

        public void SkipSizeTransition()
        {
            mInternalSize = new SizeF(Width, Height);
            base.Size = mTargetSize;
        }

        protected internal override void Update()
        {
            if (base.Size.Equals(mTargetSize) == false && AgateDisplay.DeltaTime > 0)
            {
                double diffW = mTargetSize.Width - mInternalSize.Width;
                double diffH = mTargetSize.Height - mInternalSize.Height;
                double maxDelta = AgateDisplay.DeltaTime * 0.001 * sizeVelocity;

                Clamp(ref diffW, maxDelta);
                Clamp(ref diffH, maxDelta);

                mInternalSize.Width += (float)diffW;
                mInternalSize.Height += (float)diffH;

                base.Size = (Size)mInternalSize;
            }
        }

        private void Clamp(ref double value, double max)
        {
            if (value > max)
                value = max;
            else if (value < -max)
                value = -max;
        }

        protected override void ResizeChildren(Size oldSize)
        {
            ResizeChildren(Children, (Size)mInternalSize, oldSize);
        }
        
        public override Size Size
        {
            get
            {
                return mTargetSize;
            }
            set
            {
                mTargetSize = value;
            }
        }

        /// <summary>
        /// Sets the region inside the window which is the client area.
        /// This should only be called by the style which is attached to the window.
        /// </summary>
        /// <param name="client"></param>
        public new void SetClientArea(Rectangle client)
        {
            base.SetClientArea(client);
        }

        public void Close()
        {
            Dispose();
        }
    }
}
