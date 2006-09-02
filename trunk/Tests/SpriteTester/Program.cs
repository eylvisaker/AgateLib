using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ERY.SpriteTester
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

            frmSpriteTester form = new frmSpriteTester();

            form.Show();
        

            while (form.Visible)
            {
                form.UpdateDisplay();

                System.Threading.Thread.Sleep(10);
                ERY.AgateLib.Core.KeepAlive();
            }
        }
    }
}