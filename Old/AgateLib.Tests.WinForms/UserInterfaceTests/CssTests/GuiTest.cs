using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgateLib;
using AgateLib.DisplayLib;

namespace AgateLib.Tests.UserInterfaceTests.CssTests
{
	class GuiTest : IAgateTest
	{
		public string Name => "FacetScene Test";

		public string Category => "User Interface";

		public void Run(string[] args)
		{
			var f1 = new frmCssEdit();
			f1.Show();

			Display.RenderState.WaitForVerticalBlank = false;

			while (f1.IsDisposed == false)
			{
				f1.RenderAgateStuff();
				AgateApp.KeepAlive();
				Thread.Sleep(5);
			}
		}
	}
}
