using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ERY.AgateLib.Utility
{
    /// <summary>
    /// A generic render target control for AgateLib to render to.
    /// </summary>
    public partial class AgateRenderTarget : UserControl
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

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Display.Impl == null)
            {
                e.Graphics.Clear(BackColor);
            }
           
            base.OnPaint(e);

        }
    }
}