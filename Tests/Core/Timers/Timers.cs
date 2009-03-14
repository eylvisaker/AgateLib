// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;

namespace TimerTester
{
    static class TimerTester
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            frmTimerTester frm = new frmTimerTester();
            frm.Show();

            Core.Initialize();

            Application.DoEvents();
            System.Threading.Thread.Sleep(0);

            double startTime = Timing.TotalMilliseconds;

            while (frm.Visible)
            {
                frm.UpdateControls(Timing.TotalMilliseconds - startTime);

                Application.DoEvents();
                System.Threading.Thread.Sleep(0);

            }
        }
    }
}