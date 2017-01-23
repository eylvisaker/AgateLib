using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;

namespace AgateLib.Tests.CoreTests.ErrorReport
{
	class ErrorReportTester : IAgateTest
	{
		public string Name { get { return "Error Reporting"; } }
		public string Category { get { return "Core"; } }

		public void Run(string[] args)
		{
			new frmErrorReportTester().ShowDialog();
		}

	}
}