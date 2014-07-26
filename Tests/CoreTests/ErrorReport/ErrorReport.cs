using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;
using AgateLib.Platform.WindowsForms.ApplicationModels;

namespace Tests.ErrorReportTester
{
	class ErrorReportTester : IAgateTest
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public void Main(string[] args)
		{
			PassiveModel.Run(args, () => new frmErrorReportTester().ShowDialog());
		}


		#region IAgateTest Members

		public string Name { get { return "Error Report Tester"; } }
		public string Category { get { return "Core"; } }

		#endregion
	}
}