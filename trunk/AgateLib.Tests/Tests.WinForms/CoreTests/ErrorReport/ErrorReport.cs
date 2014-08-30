using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;
using AgateLib.Platform.WinForms.ApplicationModels;

namespace AgateLib.Testing.CoreTests.ErrorReport
{
	class ErrorReportTester : IDiscreteAgateTest
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		public void Main(string[] args)
		{
			new PassiveModel(args).Run( () => new frmErrorReportTester().ShowDialog());
		}

		public string Name { get { return "Error Report Tester"; } }
		public string Category { get { return "Core"; } }
	}
}