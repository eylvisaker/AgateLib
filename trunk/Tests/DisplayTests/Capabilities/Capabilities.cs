using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.DisplayTests.Capabilities
{
	class Capabilities : IAgateTest 
	{
		#region IAgateTest Members

		public string Name
		{
			get { return "Capabilities"; }
		}

		public string Category
		{
			get { return "Display"; }
		}

		public void Main(string[] args)
		{
			using (AgateLib.AgateSetup setup = new AgateLib.AgateSetup())
			{
				setup.Initialize(true, false, false);
				if (setup.WasCanceled)
					return;

				new frmCapabilities().ShowDialog();
			}
		}

		#endregion
	}
}
