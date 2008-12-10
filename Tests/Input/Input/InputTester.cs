// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace InputTester
{
    static class InputTester
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
		{			
			// These two lines are used by AgateLib tests to locate
			// driver plugins and images.
			AgateLib.Utility.FileManager.AssemblyPath.Add("../Drivers");
			AgateLib.Utility.FileManager.ImagePath.Add("../../../Tests/TestImages");


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}