// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;

namespace ERY.SpriteTester
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