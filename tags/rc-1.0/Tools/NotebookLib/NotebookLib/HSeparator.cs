using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace NotebookLib
{
    public partial class HSeparator : UserControl
    {
        public HSeparator()
        {
            InitializeComponent();
        }

        private void HSeparator_Paint(object sender, PaintEventArgs e)
        {
            Pen dark = SystemPens.ControlDark;
            Pen light = SystemPens.ControlLight;

            e.Graphics.DrawLine(dark, new Point(2, 2), new Point(Width - 2, 2));
            e.Graphics.DrawLine(light, new Point(2, 3), new Point(Width - 2, 3));
        }

        [DefaultValue(false)]
        public new bool TabStop
        {
            get { return base.TabStop; }
            set { base.TabStop = value; }
        }
        
    }
}
