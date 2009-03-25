// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

using AgateLib;

namespace SurfaceTester
{
    static class SurfaceTester
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
            AgateFileProvider.Images.AddPath("Images");

            frmSurfaceTester form = new frmSurfaceTester();

            using (AgateSetup displaySetup = new AgateSetup())
            {
                displaySetup.AskUser = true;
                displaySetup.Initialize(true, false, false);
                if (displaySetup.WasCanceled)
                    return;

                form.Show();

                int frame = 0;

                while (form.Visible)
                {
                    form.UpdateDisplay();

                    frame++;

                }
            }
            
        }

    }
}