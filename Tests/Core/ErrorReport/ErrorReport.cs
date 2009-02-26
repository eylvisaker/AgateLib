using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;

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
			AgateFileProvider.Assemblies.AddPath("../Drivers");
			AgateFileProvider.Images.AddPath("Images");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmErrorReportTester());
        }
    }
}