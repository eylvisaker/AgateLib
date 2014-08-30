using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AgateLib;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.Platform.WinForms;
using System.IO;

namespace AgateLib.Testing
{
	class TestLauncher
	{
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Application.Run(new frmLauncher());

			Core.Settings.SaveSettings();
		}
	}
}
