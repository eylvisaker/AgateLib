﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using System.Windows.Forms;

namespace AgateLib.Tests.CoreTests
{
	class PersistantSettingsTest : IAgateTest
	{
		public string Name => "Persistant Settings";

		public string Category => "App";

		public void Run(string[] args)
		{
			if (AgateApp.Settings["Testy"].IsEmpty)
			{
				InitializeSettings();
			}

			int runcount = int.Parse(AgateApp.Settings["Testy"]["RunCount"]);
			runcount++;
			AgateApp.Settings["Testy"]["RunCount"] = runcount.ToString();

			AgateApp.Settings.SaveSettings();

			MessageBox.Show($"RunCount = {runcount}.");
		}

		private void InitializeSettings()
		{
			AgateApp.Settings["Testy"]["MyTest"] = "true";
			AgateApp.Settings["Testy"]["RunCount"] = "0";
		}
	}
}
