using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AgateLib;
using AgateLib.Platform.WindowsForms.ApplicationModels;
using AgateLib.Platform.WindowsForms;
using System.IO;

namespace Tests
{
	class TestLauncher
	{
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Directory.SetCurrentDirectory("Data");
			//PassiveParameters = new PassiveModelParameters(args);
			//PassiveParameters.AssetPath = "Data";

			//new PassiveModel(PassiveParameters).Run(() =>
			//	{
			//		Application.Run(new frmLauncher());
			//	});

			Application.Run(new frmLauncher());

			Core.Settings.SaveSettings();
		}

		public static PassiveModelParameters PassiveParameters { get; private set; }
	}
}
