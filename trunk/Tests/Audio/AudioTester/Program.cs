// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AudioTester
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (AgateLib.Core.AgateSetup setup = new AgateLib.Core.AgateSetup("Agate Audio Tester", args))
            {
                setup.AskUser = true;
                setup.Initialize(false, true, false);
                if (setup.Cancel)
                    return;

                Application.Run(new frmAudioTester());
            }
        }
    }
}