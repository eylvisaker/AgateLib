using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AgateLib;

namespace ERY.Sprite2Tester
{
    static class Sprite2Tester
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
			AgateFileProvider.Assemblies.AddPath("../Drivers");
			AgateFileProvider.Images.AddPath("../../../Tests/TestImages");

            using (AgateSetup displaySetup = new AgateSetup())
            {
                displaySetup.AskUser = true;
                displaySetup.Initialize(true, false, false);
                if (displaySetup.WasCanceled)
                    return;


                frmSprite2Tester form = new frmSprite2Tester();
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