using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgateLib.Platform.WinForms;

namespace WindowsFormsInitialization
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			using (var setup = new AgateSetupWinForms(args))
			{
				setup.CreateDisplayWindow = false;
				setup.InitializeAgateLib();

				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new Form1());
			}
		}
	}
}
