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
    public class Label : Component
    {
        private string mText;
        private bool mMouseIn;

        public Label()
        {
            BackColor = Color.FromArgb(0, 0, 0, 0);
            AttachEvents();
        }
        public Label(string text)
        {
            BackColor = Color.FromArgb(0, 0, 0, 0);
            Text = text;

            AttachEvents();
        }
        public Label(Container parent, Point location, string text)
            : base(parent, true)
        {
            BackColor = Color.FromArgb(0, 0, 0, 0);
            Text = text;
            Location = location;

            AttachEvents();
        }

        public string Text
        {
            get { return mText; }
            set
            {
                bool evt = !string.Equals(mText, value, StringComparison.Ordinal);
                
                mText = value;

                if (evt)
                    OnTextChanged();

                CheckAutoSize();
            }
        }


        private void AttachEvents()
        {
            this.MouseDown += new GuiInputEventHandler(Label_MouseDown);
            this.MouseUp += new GuiInputEventHandler(Label_MouseUp);
        }

        void Label_MouseUp(object sender, InputEventArgs e)
        {
            if (mMouseIn && this.Bounds.Contains(e.MousePosition))
                OnClick();

            mMouseIn = false;
        }
        void Label_MouseDown(object sender, InputEventArgs e)
        {
            mMouseIn = true;
        }


        private void OnTextChanged()
        {
            if (TextChanged != null)
                TextChanged(this, EventArgs.Empty);
        }
        private void OnClick()
        {
            if (Click != null)
                Click(this, EventArgs.Empty);
        }

        public event EventHandler Click;
        public event EventHandler TextChanged;
    }
}
