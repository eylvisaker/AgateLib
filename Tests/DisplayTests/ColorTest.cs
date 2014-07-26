using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.Geometry;
using AgateLib.DisplayLib;
using AgateLib.Platform.WindowsForms.ApplicationModels;

namespace Tests.DisplayTests
{
	class ColorTest : IAgateTest
	{
		#region IAgateTest Members

		public string Name
		{
			get { return "Color Test"; }
		}

		public string Category
		{
			get { return "Display"; }
		}

		public void Main(string[] args)
		{
			PassiveModel.Run(args, () =>
			{
				DisplayWindow wind = DisplayWindow.CreateWindowed(
					"Color Test", 640, 480);

				while (wind.IsClosed == false)
				{
					Display.BeginFrame();
					Display.Clear();

					for (int i = 0; i < 360; i++)
					{
						Display.FillRect(new Rectangle(i, 0, 1, 75), Color.FromHsv(i, 1, 1));
					}

					Display.EndFrame();
					Core.KeepAlive();
				}
			});
		}

		#endregion
	}
}
