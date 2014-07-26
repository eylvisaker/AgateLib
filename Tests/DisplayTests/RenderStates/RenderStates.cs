using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.Platform.WindowsForms.ApplicationModels;

namespace Tests.DisplayTests.RenderStates
{
	class RenderStates : IAgateTest 
	{

		#region IAgateTest Members

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
			PassiveModel.Run(args, () =>
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

		#endregion
	}
}
