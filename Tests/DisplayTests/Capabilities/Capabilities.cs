using AgateLib.ApplicationModels;
using AgateLib.Platform.WindowsForms.ApplicationModels;
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
			PassiveModel.Run(args, () =>
			{
				new frmCapabilities().ShowDialog();
			});
		}

		#endregion
	}
}
