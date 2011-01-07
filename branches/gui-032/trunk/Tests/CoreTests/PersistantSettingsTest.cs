using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;

namespace Tests.CoreTests
{
	class PersistantSettingsTest : IAgateTest 
	{

		public string Name
		{
			get { return "Persistant Settings"; }
		}

		public string Category
		{
			get { return "Core"; }
		}

		public void Main(string[] args)
		{
			using (AgateSetup setup = new AgateSetup())
			{
				setup.CompanyName = "Tester Inc.";
				setup.ApplicationName = "Testing";
				setup.InitializeAll();
				if (setup.WasCanceled) 
					return;

				if (Core.Settings["Testy"].IsEmpty)
				{
					InitializeSettings();
				}

				int runcount = int.Parse(Core.Settings["Testy"]["RunCount"]);
				runcount++;
				Core.Settings["Testy"]["RunCount"] = runcount.ToString();

				Core.Settings.SaveSettings();
			}
		}

		private void InitializeSettings()
		{
			Core.Settings["Testy"]["MyTest"] = "true";
			Core.Settings["Testy"]["RunCount"] = "0";
			
		}

	}
}
