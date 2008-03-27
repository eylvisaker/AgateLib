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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ERY.AgateLib.WinForms
{
    /// <summary>
    /// A generic render target control for AgateLib to render to.
    /// </summary>
    public partial class AgateRenderTarget : Panel
    {
        /// <summary>
        /// Constructs an AgateRenderTarget.
        /// </summary>
        public AgateRenderTarget()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);            // No Need To Erase Form Background
            SetStyle(ControlStyles.Opaque, true);                          // No Need To Draw Form Background
            SetStyle(ControlStyles.ResizeRedraw, true);                    // Redraw On Resize
            SetStyle(ControlStyles.UserPaint, true);                       // We'll Handle Painting Ourselves
            
            InitializeComponent();
        }

        enum ClassStyles : uint
        {
            CS_VREDRAW = 0x0001,
            CS_HREDRAW = 0x0002,
            CS_DBLCLKS = 0x0008,
            CS_OWNDC = 0x0020,
            CS_CLASSDC = 0x0040,
            CS_PARENTDC = 0x0080,
            CS_NOCLOSE = 0x0200,
            CS_SAVEBITS = 0x0800,
            CS_BYTEALIGNCLIENT = 0x1000,
            CS_BYTEALIGNWINDOW = 0x2000,
            CS_GLOBALCLASS = 0x4000,
            CS_IME = 0x00010000,
            CS_DROPSHADOW = 0x00020000
        }

        protected override CreateParams CreateParams
        {
            get
            {
                // supposedly this should improve compatibility on Windows Vista
                // when the compositing manager is used and Agate is running through
                // OpenGL.
                CreateParams retval = base.CreateParams;
                retval.ClassStyle |= (int)ClassStyles.CS_OWNDC;

                return retval;
            }
        }

        bool AnyDesignMode
        {
            get
            {
                Control p = this;

                do
                {
                    if (p.Site != null && p.Site.DesignMode)
                        return true;

                    p = p.Parent;

                } while (p != null);

                return false;
            }
        }
        /// <summary>
        /// Overrides the base class OnPaint method, to clear the
        /// client area in the Visual Studio designer.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (AnyDesignMode)
                e.Graphics.Clear(BackColor);
           
            base.OnPaint(e);

        }
    }
}