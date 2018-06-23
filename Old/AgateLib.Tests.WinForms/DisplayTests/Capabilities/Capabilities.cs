using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Tests.DisplayTests.Capabilities
{
	class Capabilities : IAgateTest
	{
		public string Name => "Capabilities";

		public string Category => "Display";

		public void Run(string[] args)
		{
			new frmCapabilities().ShowDialog();
		}
	}
}
