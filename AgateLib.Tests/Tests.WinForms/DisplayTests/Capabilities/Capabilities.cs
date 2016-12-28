using AgateLib.ApplicationModels;
using AgateLib.Platform.WinForms.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Configuration;

namespace AgateLib.Tests.DisplayTests.Capabilities
{
	class Capabilities : IAgateTest
	{
		public string Name => "Capabilities";

		public string Category => "Display";

		public AgateConfig Configuration { get; set; }

		public void Run()
		{
			new frmCapabilities().ShowDialog();
		}

		public void ModifySetup(IAgateSetup setup)
		{
			setup.CreateDisplayWindow = false;
		}
	}
}
