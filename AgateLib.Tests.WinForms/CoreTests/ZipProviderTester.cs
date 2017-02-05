using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Utility;

namespace Tests.CoreTests
{
	class ZipProviderTester : IAgateTest
	{
		public void Main(string[] args)
		{
			var zip = new ZipFileProvider("Data/dogs.zip");

			using (AgateSetup setup = new AgateSetup())
			{
				setup.Initialize(true, false, false);
				if (setup.WasCanceled)
					return;

				DisplayWindow wind = DisplayWindow.CreateWindowed(
					"ZipFileProvider Tester", 800, 600, false);

				Surface surf = new Surface(zip, "dogs.png");
				Surface surf2 = new Surface(zip, "bigpaddle.png");
				Surface surf3 = new Surface(zip, "other/bg-bricks.png");

				PixelBuffer pix = surf.ReadPixels();

				while (wind.IsClosed == false)
				{
					Display.BeginFrame();
					Display.Clear(Color.Blue);

					surf.Draw();
					surf2.Draw(10, 490);
					surf3.Draw(100, 100);

					Display.EndFrame();
					Core.KeepAlive();
				}
			}
		}

		#region IAgateTest Members

		public string Name { get { return "Zip Provider"; } }
		public string Category { get { return "Core"; } }

		#endregion
	}
}
