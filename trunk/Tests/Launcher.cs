using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AgateLib;
using AgateLib.Platform.WindowsForms.ApplicationModels;
using AgateLib.Platform.WindowsForms;

namespace Tests
{
	class Launcher
	{
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Configuration.Initialize();
			Configuration.Images.AddPath("Data");
			Configuration.Sounds.AddPath("Data");

			Application.Run(new frmLauncher());

			Core.Settings.SaveSettings();
		}
	}
}
