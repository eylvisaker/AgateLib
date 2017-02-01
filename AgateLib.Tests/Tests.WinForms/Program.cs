using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AgateLib;
using AgateLib.Platform.WinForms;
using System.IO;

namespace AgateLib.Tests
{
	class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var frm = new frmLauncher();
			var presenter = new TestLauncherPresenter(frm);

			Application.Run(frm);

			AgateApp.Settings?.Save();
		}
	}
}
