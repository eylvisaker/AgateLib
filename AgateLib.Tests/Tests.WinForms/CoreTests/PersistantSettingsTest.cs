using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.ApplicationModels;
using AgateLib.Configuration;
using System.Windows.Forms;

namespace AgateLib.Tests.CoreTests
{
	class PersistantSettingsTest : IAgateTest
	{
		public string Name => "Persistant Settings";

		public string Category => "Core";

		public AgateConfig Configuration { get; set; }

		public void Run()
		{
			if (Core.Settings["Testy"].IsEmpty)
			{
				InitializeSettings();
			}

			int runcount = int.Parse(Core.Settings["Testy"]["RunCount"]);
			runcount++;
			Core.Settings["Testy"]["RunCount"] = runcount.ToString();

			Core.Settings.SaveSettings();

			MessageBox.Show($"RunCount = {runcount}.");
		}

		private void InitializeSettings()
		{
			Core.Settings["Testy"]["MyTest"] = "true";
			Core.Settings["Testy"]["RunCount"] = "0";
		}

		public void ModifySetup(IAgateSetup setup)
		{
			setup.CreateDisplayWindow = false;
		}
	}
}
