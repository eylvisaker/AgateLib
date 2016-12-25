using AgateLib.ApplicationModels;
using AgateLib.Platform.WinForms.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Tests.DisplayTests.Capabilities
{
	class Capabilities : IDiscreteAgateTest
	{
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
			using (var model = new PassiveModel(args))
			{
				model.Run(() =>
				{
					new frmCapabilities().ShowDialog();
				});
			}
		}
	}
}
