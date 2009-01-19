// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;

namespace InputTester
{
    static class InputTester
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
		{			
			// These two lines are used by AgateLib tests to locate
			// driver plugins and images.
			AgateLib.Utility.AgateFileProvider.AssemblyProvider.AddPath("../Drivers");
			AgateLib.Utility.AgateFileProvider.ImageProvider.AddPath("../../../Tests/TestImages");

            using (AgateSetup setup = new AgateSetup())
            {
                setup.AskUser = true;
                setup.Initialize(true, false, true);
                if (setup.WasCanceled)
                    return;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }
    }
}