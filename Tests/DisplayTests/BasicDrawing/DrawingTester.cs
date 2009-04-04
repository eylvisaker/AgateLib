// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Tests.DisplayTests.BasicDrawing
{
    public partial class DrawingTester : Form
    {
        public DrawingTester()
        {
            InitializeComponent();
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = btnColor.BackColor;

            if (colorDialog1.ShowDialog() == DialogResult.OK)
                btnColor.BackColor = colorDialog1.Color;
        }

        public Color SelectedColor
        {
            get
            {
                return Color.FromArgb((int)(nudAlpha.Value * 255.0m), btnColor.BackColor);
            }
        }

    }
}