using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AgateLib;

namespace AgateLib.Gui.Tester
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Utility.AgateFileProvider.AssemblyProvider.AddPath("../Drivers");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (AgateSetup setup = new AgateSetup())
            {
                setup.Initialize(true, false, false);
                if (setup.WasCanceled)
                    return;

                Form1 frm = new Form1();
                
                frm.Show();
                frm.Run();
            }
        }
    }
}
