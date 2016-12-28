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
			Core.KeepAlive();

			BitmapFontOptions fontOptions = new BitmapFontOptions("Times", 18, FontStyles.Bold);
			fontOptions.TextRenderer = TextRenderEngine.TextRenderer;

			FontSurface font = new FontSurface(BitmapFontUtil.ConstructFromOSFont(fontOptions));

			// TODO: Fix this
			//font.Save("testfont.xml");

			//FontSurface second = FontSurface.LoadBitmapFont("testfont.png", "testfont.xml");

			while (Display.CurrentWindow.IsClosed == false)
			{
				Display.BeginFrame();
				Display.Clear(Color.Navy);

				font.DrawText("The quick brown fox jumped over the lazy dogs.");

				//second.DrawText(0, font.StringDisplayHeight("M"), "The quick brown fox jumps over the lazy dog.");

				Display.EndFrame();
				Core.KeepAlive();
			}
		}

		public void ModifySetup(IAgateSetup setup)
		{
		}
	}
}