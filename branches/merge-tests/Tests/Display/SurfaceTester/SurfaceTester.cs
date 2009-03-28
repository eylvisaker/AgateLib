// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

using AgateLib;

namespace Tests.SurfaceTester
{
    static class SurfaceTester
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [AgateTest("Surface Tester", "Display")]
        static void Main()
        {
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