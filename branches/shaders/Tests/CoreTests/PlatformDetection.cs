using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tests.CoreTests
{
	public partial class PlatformDetection : Form, IAgateTest 
	{
		public PlatformDetection()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();

		}

		#region IAgateTest Members

		string IAgateTest.Name
		{
			get { return "Platform Detection"; }
		}

		string IAgateTest.Category
		{
			get { return "Core"; }
		}

		void IAgateTest.Main(string[] args)
		{
			Application.Run(this);
		}

		#endregion
	}
}
