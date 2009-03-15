using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using AgateLib.DisplayLib;
namespace TileTester
{
    public partial class frmTileTester : Form
    {
        public frmTileTester()
        {
            InitializeComponent();

            CreateControl();

            DisplayWindow wind = DisplayWindow.CreateFromControl(agateRenderTarget1);
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