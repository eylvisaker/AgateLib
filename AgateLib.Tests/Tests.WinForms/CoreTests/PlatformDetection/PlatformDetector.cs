using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Tests.CoreTests.PlatformDetection
{
	class PlatformDetector : IAgateTest
	{
		public string Name => "Platform Detection";

		public string Category => "Core";

		public void Run(string[] args)
		{
			new PlatformDetection().ShowDialog();
		}
	}
}
