using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Configuration;

namespace AgateLib.Tests.CoreTests.PlatformDetection
{
	class PlatformDetector : IAgateTest
	{
		public string Name => "Platform Detection";

		public string Category => "Core";

		public AgateConfig Configuration { get; set; }

		public void Run()
		{
			new PlatformDetection().ShowDialog();
		}

		public void ModifySetup(IAgateSetup setup)
		{
			setup.CreateDisplayWindow = false;
		}
	}
}
