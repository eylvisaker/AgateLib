using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TileTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            CreateControl();

            ERY.AgateLib.DisplayWindow wind = new ERY.AgateLib.DisplayWindow(agateRenderTarget1);
        }

        public bool ScrollX
        {
            get { return chkScrollX.Checked; }
            set { chkScrollX.Checked = value; }
        }
        public bool ScrollY
        {
            get { return chkScrollY.Checked; }
            set { chkScrollY.Checked = value; }
        }
    }
}