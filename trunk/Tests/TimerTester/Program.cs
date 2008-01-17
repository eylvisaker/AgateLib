// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ERY.AgateLib;

namespace TimerTester
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 frm = new Form1();
            frm.Show();

            Core.Initialize();

            double startTime = Timing.TotalMilliseconds;

            while (frm.Visible)
            {
                frm.UpdateControls(Timing.TotalMilliseconds - startTime);

                Application.DoEvents();
            }

        }
    }
}