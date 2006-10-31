//     ``The contents of this file are subject to the Mozilla Public License
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
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ERY.AgateLib.Utility
{
    public partial class DisplayWindowForm : Form
    {
        
        /// <summary>
        /// Constructs a DisplayWindowForm object.
        /// </summary>
        public DisplayWindowForm()
        {

           // CreateParams.ClassStyle = this.CreateParams.ClassStyle |       // Redraw On Size, And Own DC For Window.
             //   Tao.Platform.Windows.User.CS_HREDRAW | Tao.Platform.Windows.User.CS_VREDRAW | Tao.Platform.Windows.User.CS_OWNDC;

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);            // No Need To Erase Form Background
            //SetStyle(ControlStyles.DoubleBuffer, true);                    // Buffer Control
            SetStyle(ControlStyles.Opaque, true);                          // No Need To Draw Form Background
            SetStyle(ControlStyles.ResizeRedraw, true);                    // Redraw On Resize
            SetStyle(ControlStyles.UserPaint, true);                       // We'll Handle Painting Ourselves

            InitializeComponent();

        }
        /// <summary>
        /// The control which is rendered into.
        /// </summary>
        public Control RenderTarget
        {
            get { return pictureBox1; }
        }

        private void DisplayWindowForm_Deactivate(object sender, EventArgs e)
        {
            Core.IsActive = false;
        }

        private void DisplayWindowForm_Activated(object sender, EventArgs e)
        {
            Core.IsActive = true;
        }
    }
}