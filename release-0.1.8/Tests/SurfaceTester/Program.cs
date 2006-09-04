// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace ERY.SurfaceTester
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

            frmSurfaceTester form = new frmSurfaceTester();

            try
            {
                form.Show();
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to start:\n\n" + e.Message);
            }

            int frame = 0;

            while (form.Visible)
            {
                form.UpdateDisplay();
               
                frame++;

            }

            
        }

    }
}