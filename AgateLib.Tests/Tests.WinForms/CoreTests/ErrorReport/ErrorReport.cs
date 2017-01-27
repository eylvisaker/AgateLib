using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;

namespace AgateLib.Tests.CoreTests.ErrorReport
{
	class ErrorReportTester : IAgateTest
	{
		public string Name => "Error Reporting";
		public string Category => "App";

		public void Run(string[] args)
		{
			new frmErrorReportTester().ShowDialog();
		}

	}
}