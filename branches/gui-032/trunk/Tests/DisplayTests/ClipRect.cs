using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace Tests.DisplayTests
{
	class ClipRect:IAgateTest
	{
		#region IAgateTest Members

		public string Name
		{
			get { return "Clip Rects"; }
		}

		public string Category
		{
			get { return "Display"; }
		}

		public void Main(string[] args)
		{
			using (AgateSetup setup = new AgateSetup())
			{
				setup.AskUser = true;
				setup.Initialize(true, false, false);
				if (setup.WasCanceled)
					return;

				DisplayWindow wind = DisplayWindow.CreateWindowed(
					"Test clip rects", 640, 480);

				Color[] colors = new Color[] { 
					Color.Red, Color.Orange, Color.Yellow, Color.YellowGreen, Color.Green, Color.Turquoise, Color.Blue, Color.Violet, Color.Wheat, Color.White};

				while (wind.IsClosed == false)
				{
					Display.BeginFrame();
					Display.Clear();

					int index = 0;
					for (int i = 10; i < 100; i += 10)
					{
						Display.SetClipRect(new Rectangle(i, i, 310 - i * 2, 310 - i * 2));
						Display.FillRect(0,0,640,480, colors[index]);
						index++;
					}

					Display.EndFrame();
					Core.KeepAlive();
				}
			}
		}

		#endregion
	}
}
