using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ErrorReportTester
{
    static class ErrorReportTester
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AgateLib.Core.Initialize();

			// These two lines are used by AgateLib tests to locate
			// driver plugins and images.
			AgateLib.Utility.AgateFileProvider.AssemblyProvider.AddPath("../Drivers");
			AgateLib.Utility.AgateFileProvider.ImageProvider.AddPath("../../../Tests/TestImages");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmErrorReportTester());
        }
    }
}