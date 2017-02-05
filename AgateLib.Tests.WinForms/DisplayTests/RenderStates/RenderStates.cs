using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;

namespace AgateLib.Tests.DisplayTests.RenderStates
{
	class RenderStates : IAgateTest
	{
		public string Name => "Render States";

		public string Category => "Display";

		public void Run(string[] args)
		{
			frmRenderStateTest frm = new frmRenderStateTest();
			frm.Show();

			int count = 0;

			while (frm.Visible)
			{
				frm.UpdateFrame();

				count++;
				if (count > 60)
					frm.Text = string.Format("Render States - {0:0.00} FPS", AgateLib.DisplayLib.Display.FramesPerSecond);

				AgateApp.KeepAlive();
			}
		}
	}
}