using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.ApplicationModels;

namespace AgateLib.Testing.CoreTests
{
	class PersistantSettingsTest : IDiscreteAgateTest 
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
			new PassiveModel(new PassiveModelParameters()
				{
					ApplicationName = "Testing",
				}).Run(args, () =>
			{
				if (Core.Settings["Testy"].IsEmpty)
				{
					InitializeSettings();
				}

				int runcount = int.Parse(Core.Settings["Testy"]["RunCount"]);
				runcount++;
				Core.Settings["Testy"]["RunCount"] = runcount.ToString();

				Core.Settings.SaveSettings();
			});
		}

		private void InitializeSettings()
		{
			Core.Settings["Testy"]["MyTest"] = "true";
			Core.Settings["Testy"]["RunCount"] = "0";
			
		}

	}
}
