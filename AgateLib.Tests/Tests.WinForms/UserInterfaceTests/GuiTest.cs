using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Platform.WinForms.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgateLib.Testing.UserInterfaceTests
{
	class GuiTest : IAgateTest
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public void Main(string[] args)
		{
			new PassiveModel(args).Run(() =>
			{
				var f1 = new frmCssEdit();
				f1.Show();

				Display.RenderState.WaitForVerticalBlank = false;

				while (f1.IsDisposed == false)
				{
					f1.RenderAgateStuff();
					Core.KeepAlive();
					Thread.Sleep(5);
				}
			});
		}

		public string Name
		{
			get { return "Gui Test"; }
		}

		public string Category
		{
			get { return "User Interface"; }
		}

	}
}
