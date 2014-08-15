using System;
using System.Collections.Generic;

using AgateLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Platform.WindowsForms.ApplicationModels;
using AgateLib.ApplicationModels;

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
			new PassiveModel(new PassiveModelParameters()
				{
					ApplicationName = "Bitmap Font Tester",
					DisplayWindowSize = new Size(800, 600),
				}).Run(args, () => 
			{
				Display.BeginFrame();
				Display.Clear(Color.Navy);
				Display.EndFrame();
				Core.KeepAlive();

				BitmapFontOptions fontOptions = new BitmapFontOptions("Times", 18, FontStyles.Bold);
				fontOptions.UseTextRenderer = true;

				FontSurface font = new FontSurface(fontOptions);

				// TODO: Fix this
				//font.Save("testfont.xml");

				//FontSurface second = FontSurface.LoadBitmapFont("testfont.png", "testfont.xml");

				while (PassiveModel.IsAlive)
				{
					Display.BeginFrame();
					Display.Clear(Color.Navy);

					font.DrawText("The quick brown fox jumps over the lazy dog.");

					//second.DrawText(0, font.StringDisplayHeight("M"), "The quick brown fox jumps over the lazy dog.");

					Display.EndFrame();
					Core.KeepAlive();
				}
			});
		}
	}
}