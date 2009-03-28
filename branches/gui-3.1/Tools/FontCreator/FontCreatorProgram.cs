using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using AgateLib;

namespace FontCreator
{
    class FontCreatorProgram
    {
        static List<string> tempFiles = new List<string>();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Directory.CreateDirectory("./images");

            AgateFileProvider.Assemblies.AddPath("../../Drivers");
            AgateFileProvider.Images.AddPath("./images");

            using (AgateLib.AgateSetup setup = new AgateLib.AgateSetup(args))
            {
                setup.Initialize(true, false, false);
                if (setup.WasCanceled)
                    return;

                frmFontCreator frm = new frmFontCreator();
                frm.Show();

                Properties.Settings.Default.Reload();
                if (Properties.Settings.Default.SkipWarning == false)
                {
                    new frmWarningSplash().ShowDialog(frm);
                }
                Properties.Settings.Default.Save();

                Application.Run(frm);
            }

            foreach (string file in tempFiles)
            {
                File.Delete(file);
            }
        }
        public static void RegisterTempFile(string file)
        {
            tempFiles.Add(file);
        }
    }
}
