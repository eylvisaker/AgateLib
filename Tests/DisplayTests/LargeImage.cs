using System;
using System.Collections.Generic;
using System.Diagnostics;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;

namespace Tests.ScreenCaptureExample
{
	class LargeImageTest : IAgateTest
	{

		#region IAgateTest Members

		public string Name { get { return "Large Image"; } }
		public string Category { get { return "Display"; } }

		#endregion

		public void Main(string[] args)
		{
			using (AgateSetup setup = new AgateSetup())
			{
				setup.Initialize(true, false, false);
				if (setup.WasCanceled) return;

				DisplayWindow wind = DisplayWindow.CreateWindowed("Hello", 800, 600);

				System.Diagnostics.Stopwatch watch = new Stopwatch();
				watch.Start();
				Surface someSurface = new Surface("largeimage.png");
				watch.Stop();
				double loadTime = watch.ElapsedMilliseconds / 1000.0;

				while (wind.IsClosed == false)
				{
					Display.BeginFrame();
					Display.Clear(Color.White);

					someSurface.Draw();
					FontSurface.AgateSans24.DrawText(0,0, "Load took {0} seconds.", loadTime);

					Display.EndFrame();

					Core.KeepAlive();
					System.Threading.Thread.Sleep(10);
				}
			}
		}
	}
}
