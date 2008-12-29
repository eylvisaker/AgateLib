// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;

namespace SpriteTester
{
    static class SpriteTester
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
			
			// These two lines are used by AgateLib tests to locate
			// driver plugins and images.
			AgateLib.Utility.AgateFileProvider.AssemblyProvider.AddPath("../Drivers");
			AgateLib.Utility.AgateFileProvider.ImageProvider.AddPath("../../../Tests/TestImages");

            frmSpriteTester form = new frmSpriteTester();

            AgateSetup displaySetup = new AgateSetup();

            using (displaySetup)
            {
                displaySetup.AskUser = true;
                displaySetup.Initialize(true, false, false);
                if (displaySetup.WasCanceled)
                    return;

                form.Show();

                while (form.Visible)
                {
                    form.UpdateDisplay();

                    //System.Threading.Thread.Sleep(10);
                    Core.KeepAlive();
                }
            }
        }
    }
}