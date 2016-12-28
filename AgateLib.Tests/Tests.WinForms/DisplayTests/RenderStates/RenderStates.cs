using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.Configuration;
using AgateLib.Platform.WinForms.ApplicationModels;

namespace AgateLib.Tests.DisplayTests.RenderStates
{
	class RenderStates : IAgateTest
	{
		public string Name => "Render States";

		public string Category => "Display";

		public AgateConfig Configuration { get; set; }

		public void ModifySetup(IAgateSetup setup)
		{
			setup.CreateDisplayWindow = false;
		}

		public void Run()
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

				Core.KeepAlive();
			}
		}
	}
}