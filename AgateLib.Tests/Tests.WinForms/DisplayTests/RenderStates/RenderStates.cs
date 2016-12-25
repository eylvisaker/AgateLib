using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.Platform.WinForms.ApplicationModels;

namespace AgateLib.Tests.DisplayTests.RenderStates
{
	class RenderStates : IDiscreteAgateTest
	{
		public string Name
		{
			get { return "Render States"; }
		}

		public string Category
		{
			get { return "Display"; }
		}

		public void Main(string[] args)
		{
			using (var model = new PassiveModel(args))
			{
				model.Run(() =>
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
				});
			}
		}
	}
}
