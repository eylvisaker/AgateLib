using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Examples.Launcher;

namespace Examples
{
	static class ExampleLauncherProgram
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

			var view = new LauncherView();
			var presenter = new LauncherPresenter(view);

			System.Windows.Forms.Application.Run(view);
		}
	}
}
