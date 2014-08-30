using System;
using System.Collections.Generic;
using System.Diagnostics;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;

namespace AgateLib.Testing.DisplayTests
{
	class LargeImageTest : IAgateTest
	{
		public string Name { get { return "Large Image"; } }
		public string Category { get { return "Display"; } }

		public void Main(string[] args)
		{
			DisplayWindow wind = DisplayWindow.CreateWindowed("Hello", 800, 600);

			System.Diagnostics.Stopwatch watch = new Stopwatch();
			watch.Start();
			Surface someSurface = new Surface("largeimage.png");
			watch.Stop();
			double loadTime = watch.ElapsedMilliseconds / 1000.0;
			FontSurface font = new FontSurface("Arial", 24);

			while (wind.IsClosed == false)
			{
				Display.BeginFrame();
				Display.Clear(Color.White);

				someSurface.Draw();
				font.DrawText(0, 0, "Load took {0} seconds.", loadTime);

				Display.EndFrame();

				Core.KeepAlive();
				System.Threading.Thread.Sleep(10);
			}
		}
	}
}
