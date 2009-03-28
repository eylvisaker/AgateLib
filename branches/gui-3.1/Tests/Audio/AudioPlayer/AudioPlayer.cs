// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;

namespace AudioTester
{
    static class AudioTester
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

			// These two lines are used by AgateLib tests to locate
			// driver plugins and images.
			AgateFileProvider.Assemblies.AddPath("../Drivers");
			AgateFileProvider.Images.AddPath("Images");

            using (AgateLib.AgateSetup setup = new AgateLib.AgateSetup("Agate Audio Tester", args))
            {
                setup.AskUser = true;
                setup.Initialize(false, true, false);
                if (setup.WasCanceled)
                    return;

                Application.Run(new frmAudioTester());
            }
        }
    }
}