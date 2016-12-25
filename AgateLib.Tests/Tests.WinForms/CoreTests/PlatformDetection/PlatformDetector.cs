using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Tests.CoreTests.PlatformDetection
{
	class PlatformDetector: IDiscreteAgateTest
	{
		public string Name
		{
			get { return "Platform Detection"; }
		}

		public string Category
		{
			get { return "Core"; }
		}

		public void Main(string[] args)
		{
			new PlatformDetection().ShowDialog();
		}
	}
}
