using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using System.Windows.Forms;

namespace AgateLib.Tests.CoreTests
{
	class PersistantSettingsTest : IAgateTest
	{
		class TestSettings
		{
			public int RunCount { get; set; }
			public bool MyTest { get; set; } = true;
		}

		public string Name => "Persistant Settings";

		public string Category => "App";

		public void Run(string[] args)
		{
			var settings = AgateApp.Settings.GetOrCreate<TestSettings>("Testy", () => new TestSettings());

			settings.RunCount++;

			AgateApp.Settings.Save();

			MessageBox.Show($"RunCount = {settings.RunCount}.");
		}
		
	}
}
