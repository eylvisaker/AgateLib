using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;

namespace Tests.ErrorReportTester
{
    static class ErrorReportTester
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [AgateTest("Error Report Test", "Core")]
        static void Main()
        {
            AgateLib.Core.Initialize();

            new frmErrorReportTester().ShowDialog();
        }
    }
}