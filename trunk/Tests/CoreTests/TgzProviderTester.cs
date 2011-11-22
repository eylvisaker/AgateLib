using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Utility;

namespace Tests.TgzProviderTester
{
	class TgzProviderTester : IAgateTest
	{
		public void Main(string[] args)
		{
			var tgz = new TgzFileProvider("Data/dogs.tar.gz");

			using (AgateSetup setup = new AgateSetup())
			{
				setup.Initialize(true, false, false);
				if (setup.WasCanceled)
					return;

				DisplayWindow wind = DisplayWindow.CreateWindowed(
					"TgzFileProvider Tester", 800, 600, false);

				Surface surf = new Surface(tgz, "dogs.png");
				Surface surf2 = new Surface(tgz, "bigpaddle.png");

				PixelBuffer pix = surf.ReadPixels();

				while (wind.IsClosed == false)
				{
					Display.BeginFrame();
					Display.Clear(Color.Blue);

					surf.Draw();
					surf2.Draw(10, 490);

					Display.EndFrame();
					Core.KeepAlive();
				}
			}
		}

		#region IAgateTest Members

		public string Name { get { return "Tar.gzip File Provider"; } }
		public string Category { get { return "Core"; } }

		#endregion
	}
}
