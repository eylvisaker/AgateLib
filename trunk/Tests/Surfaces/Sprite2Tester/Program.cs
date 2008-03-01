using System;
using System.Collections.Generic;
using System.Windows.Forms;

using ERY.AgateLib;

namespace ERY.Sprite2Tester
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

            using (AgateSetup displaySetup = new AgateSetup())
            {
                displaySetup.AskUser = true;
                displaySetup.Initialize(true, false, false);
                if (displaySetup.Cancel)
                    return;


                frmSprite2Tester form = new frmSprite2Tester();
                form.Show();
                
                while (form.Visible)
                {
                    form.UpdateDisplay();

                    //System.Threading.Thread.Sleep(10);
                    ERY.AgateLib.Core.KeepAlive();
                }
            }
        }
    }
}