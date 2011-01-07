// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Tests.PerformanceTester
{
	public partial class frmPerformanceTester : Form
	{
		public frmPerformanceTester()
		{
			InitializeComponent();
			this.Location = new Point(Screen.GetBounds(this).Width - this.Width - 10, 10);
		}

		internal void AddTestResult(PerformanceTester.TestResult r)
		{
			var item = new ListViewItem(
				new string[] { r.Driver, r.Name, r.Time.ToString("0.000"), r.FPS.ToString("0.0") });

			listView1.Items.Add(item);
		}
	}
}