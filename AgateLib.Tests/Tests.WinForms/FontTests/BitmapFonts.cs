using System;
using System.Collections.Generic;

using AgateLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.ApplicationModels;
using AgateLib.Platform.WinForms.Fonts;
using AgateLib.Configuration;

namespace AgateLib.Tests.FontTests
{
	class BitmapFonts : IAgateTest
	{
		public string Name => "Bitmap Fonts";
		public string Category => "Fonts";

		public AgateConfig Configuration { get; set; }
		
		public void Run()
		{
			Display.BeginFrame();
			Display.Clear(Color.Navy);
			Display.EndFrame();
			Core.KeepAlive();

			BitmapFontOptions fontOptions = new BitmapFontOptions("Times", 18, FontStyles.Bold);
			fontOptions.TextRenderer = TextRenderEngine.TextRenderer;

			FontSurface font = new FontSurface(BitmapFontUtil.ConstructFromOSFont(fontOptions));

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
		}

		public void ModifySetup(IAgateSetup setup)
		{
			setup.CreateDisplayWindow = false;
		}
	}
}