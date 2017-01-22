using AgateLib.Configuration;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.Geometry;
using AgateLib.Platform.WinForms.Fonts;

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
			AgateApp.KeepAlive();

			BitmapFontOptions fontOptions = new BitmapFontOptions("Times", 18, FontStyles.None);
			fontOptions.TextRenderer = TextRenderEngine.TextRenderer;

			FontSurface surface = new FontSurface(BitmapFontUtil.ConstructFromOSFont(fontOptions));
			Font font = new FontBuilder("Times")
				.AddFontSurface(new FontSettings(18, FontStyles.None), surface)
				.Build();

			// TODO: Fix this
			//font.Save("testfont.xml");

			//FontSurface second = FontSurface.LoadBitmapFont("testfont.png", "testfont.xml");

			while (AgateApp.IsAlive)
			{
				Display.BeginFrame();
				Display.Clear(Color.Navy);

				font.DrawText("The quick brown fox jumped over the lazy dogs.");

				//second.DrawText(0, font.StringDisplayHeight("M"), "The quick brown fox jumps over the lazy dog.");

				Display.EndFrame();
				AgateApp.KeepAlive();
			}
		}

		public void ModifySetup(IAgateSetup setup)
		{
		}
	}
}