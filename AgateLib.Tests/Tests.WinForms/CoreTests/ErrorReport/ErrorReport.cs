using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;
using AgateLib.Configuration;
using AgateLib.Platform.WinForms.ApplicationModels;

namespace AgateLib.Tests.CoreTests.ErrorReport
{
	class ErrorReportTester : IAgateTest
	{
		public string Name { get { return "Error Reporting"; } }
		public string Category { get { return "Core"; } }

		public AgateConfig Configuration { get; set; }
		
		public void ModifySetup(IAgateSetup setup)
		{
			setup.CreateDisplayWindow = false;
		}

		public void Run()
		{
			new frmErrorReportTester().ShowDialog();
		}

	}
}