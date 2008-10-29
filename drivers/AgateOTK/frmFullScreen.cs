using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AgateLib.Display.OpenGL
{
    public partial class frmFullScreen : Form
    {
        public frmFullScreen()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

            InitializeComponent();
        }
    }
}
