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

using AgateLib.Geometry;
using AgateLib.InputLib;

namespace AgateLib.Gui
{
    public class Button : Component 
    {
        private string mText;
        private bool mDrawDown;
        private bool mMouseClickIn;
        private bool mToggleButton;

        public Button(Container parent, string text)
            : base(parent, true)
        {
            Text = text;
            AttachEvents();
        }
        public Button(Container parent, Rectangle bounds, string text)
            : base(parent, bounds)
        {
            Text = text;
            AttachEvents();
        }
        public Button(Container parent, Point location, string text)
            : base(parent, true)
        {
            Text = text;
            AttachEvents();

            Location = location;

            CheckAutoSize();
        }

        private void AttachEvents()
        {
            MouseDown += new GuiInputEventHandler(Button_MouseDown);
            MouseEnter += new GuiInputEventHandler(Button_MouseEnter);
            MouseLeave += new GuiInputEventHandler(Button_MouseLeave);
            MouseUp += new GuiInputEventHandler(Button_MouseUp);
        }
        void Button_MouseUp(object sender, InputEventArgs e)
        {
            if (mDrawDown)
            {
                if (Click != null)
                    Click(this, EventArgs.Empty);
            }

            mDrawDown = false;
            mMouseClickIn = false;
        }
        void Button_MouseLeave(object sender, InputEventArgs e)
        {
            if (mMouseClickIn)
                mDrawDown = false;
        }
        void Button_MouseEnter(object sender, InputEventArgs e)
        {
            if (mMouseClickIn)
                mDrawDown = true;
        }
        void Button_MouseDown(object sender, InputEventArgs e)
        {
            if (e.MouseButtons == Mouse.MouseButtons.Primary)
            {
                mMouseClickIn = true;
                mDrawDown = true;
            }
        }

        public bool ToggleButton
        {
            get { return mToggleButton; }
            set { mToggleButton = value; }
        }
        public bool DrawDown
        {
            get { return mDrawDown; }
        }
        public bool DrawHover
        {
            get { return mDrawDown == false && mMouseIn == true; }
        }

        public string Text
        {
            get { return mText; }
            set 
            { 
                mText = value;

                if (TextChanged != null)
                    TextChanged(this, EventArgs.Empty);

                CheckAutoSize();
            }
        }


        public event EventHandler Click;
        public event EventHandler TextChanged;

    }
}
