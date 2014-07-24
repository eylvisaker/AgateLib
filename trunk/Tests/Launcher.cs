using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AgateLib;

namespace Tests
{
	class Launcher
	{
		[STAThread]
		public static void Main(string[] args)
		{
			AgateLib.Platform.WindowsForms.Setup.Initialize();

			AgateFileProvider.Images.AddPath("Data");
			AgateFileProvider.Sounds.AddPath("Data");
			
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new frmLauncher());

			Core.Settings.SaveSettings();
		}
	}
}
