using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AgateLib.Tests.DisplayTests.PixelBufferTest
{
	public partial class PixelBufferForm : Form
	{
		public PixelBufferForm()
		{
			InitializeComponent();
		}

		private void btnColor_Click(object sender, EventArgs e)
		{
			colorDialog1.Color = btnColor.BackColor;

			if (colorDialog1.ShowDialog() == DialogResult.OK)
			{
				btnColor.BackColor = colorDialog1.Color;
			}
		}
	}
}