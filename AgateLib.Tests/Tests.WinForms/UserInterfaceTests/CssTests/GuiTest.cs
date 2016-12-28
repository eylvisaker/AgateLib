using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Platform.WinForms.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgateLib.Configuration;

namespace AgateLib.Tests.UserInterfaceTests.CssTests
{
	class GuiTest : IAgateTest
	{
		public string Name => "Gui Test";

		public string Category => "User Interface";

		public AgateConfig Configuration { get; set; }

		public void Run()
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
		}

		public void ModifySetup(IAgateSetup setup)
		{
			setup.CreateDisplayWindow = false;
		}
	}
}
