// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;

namespace Tests.SpriteTester
{
    static class SpriteTester
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [AgateTest("Sprite Tester", "Display")]
        static void Main()
        {
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