using System;
using System.Collections.Generic;

using AgateLib;
using AgateLib.BitmapFont;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;

namespace Tests.BitmapFontTester
{
	class BitmapFonts : IAgateTest
	{
		#region IAgateTest Members

		public string Name { get { return "Bitmap Fonts"; } }
		public string Category { get { return "Fonts"; } }

		#endregion

		public void Main(string[] args)
		{
			using (AgateSetup setup = new AgateSetup(args))
			{
				setup.Initialize(true, false, false);
				if (setup.WasCanceled)
					return;

				DisplayWindow wind = DisplayWindow.CreateWindowed(
					"Bitmap Font Tester", 800, 600, false);

				Display.BeginFrame();
				Display.Clear(Color.Navy);
				Display.EndFrame();
				Core.KeepAlive();

				BitmapFontOptions fontOptions = new BitmapFontOptions("Times", 18, FontStyle.Bold);
				fontOptions.UseTextRenderer = true;

				FontSurface font = new FontSurface(fontOptions);

				// TODO: Fix this
				//font.Save("testfont.xml");


				//FontSurface second = FontSurface.LoadBitmapFont("testfont.png", "testfont.xml");

				while (wind.IsClosed == false)
				{
					Display.BeginFrame();
					Display.Clear(Color.Navy);

					font.DrawText("The quick brown fox jumps over the lazy dog.");

					//second.DrawText(0, font.StringDisplayHeight("M"), "The quick brown fox jumps over the lazy dog.");

					Display.EndFrame();
					Core.KeepAlive();
				}
			}
		}
	}
}