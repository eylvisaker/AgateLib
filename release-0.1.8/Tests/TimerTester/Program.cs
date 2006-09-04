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

            using (AgateSetup setup = new AgateSetup())
            {
                setup.InitializeDisplay();
             
                // fake out the display with an off-screen render target
                Surface surf = new Surface(10, 10);
                Display.RenderTarget = surf;

                while (frm.Visible)
                {
                    Display.BeginFrame();
                    Display.EndFrame();

                    frm.UpdateControls(Display.DeltaTime);

                    Application.DoEvents();
                }
            }
        }
    }
}