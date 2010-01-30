using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DatabaseEditor
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			TestData r = new TestData();
			r.Run();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new frmEditor());
		}
	}
}
