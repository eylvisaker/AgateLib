// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

using AgateLib;

namespace ERY.SurfaceTester
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