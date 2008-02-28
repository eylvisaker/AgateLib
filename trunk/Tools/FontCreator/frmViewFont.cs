using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FontCreator
{
    public partial class frmViewFont : Form
    {
        public frmViewFont()
        {
            InitializeComponent();
        }

        internal DialogResult ShowDialog(IWin32Window owner, string tempImage, string tempXml)
        {
            pctImage.BackColor = Color.FromArgb(128, 64, 64);
            pctImage.Image = new Bitmap(tempImage);

            txtXml.Url = new Uri(tempXml);

            return ShowDialog(owner);
        }
    }
}