using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AgateLib.Tests.CoreTests.PlatformDetection
{
	public partial class PlatformDetection : Form 
	{
		public PlatformDetection()
		{
			InitializeComponent();

			lblPlatform.Text = "Platform: " + AgateLib.AgateApp.Platform.PlatformType.ToString();
			lblRuntime.Text = "Runtime: " + AgateLib.AgateApp.Platform.Runtime.ToString();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();

		}

	}
}
