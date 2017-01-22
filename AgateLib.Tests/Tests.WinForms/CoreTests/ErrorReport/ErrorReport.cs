using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;
using AgateLib.Configuration;

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

		public void Run(string[] args)
		{
			new frmErrorReportTester().ShowDialog();
		}

	}
}