using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AgateLib.Tests.Shaders.LightingTest
{
	public partial class LightingTestForm : Form
	{
		public LightingTestForm()
		{
			InitializeComponent();

			Icon = AgateLib.Platform.WinForms.Controls.FormUtil.AgateLibIcon;
		}

		private void btnDiffuse_Click(object sender, EventArgs e)
		{
			colorDialog1.Color = btnDiffuse.BackColor;

			if (colorDialog1.ShowDialog() == DialogResult.OK)
				btnDiffuse.BackColor = colorDialog1.Color;
		}

		private void btnAmbient_Click(object sender, EventArgs e)
		{
			colorDialog1.Color = btnAmbient.BackColor;

			if (colorDialog1.ShowDialog() == DialogResult.OK)
				btnAmbient.BackColor = colorDialog1.Color;
		}
	}
}