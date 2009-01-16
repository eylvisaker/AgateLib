using System;
using System.Collections.Generic;

using AgateLib;
using AgateLib.BitmapFont;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;

namespace BitmapFontTester
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            new Program().Run();
        }

        private void Run()
        {
			// These two lines are used by AgateLib tests to locate
			// driver plugins and images.
			AgateLib.Utility.AgateFileProvider.AssemblyProvider.AddPath("../Drivers");
			AgateLib.Utility.AgateFileProvider.ImageProvider.AddPath("../../../Tests/TestImages");
            
			using (AgateSetup setup = new AgateSetup())
            {
                setup.AskUser = true;
                setup.Initialize(true, false, false);
                if (setup.Cancel)
                    return;

                DisplayWindow wind = new DisplayWindow("Bitmap Font Tester", 800, 600, false, false);

                Display.BeginFrame();
                Display.Clear(Color.Navy);
                Display.EndFrame();
                Core.KeepAlive();

                BitmapFontOptions fontOptions = new BitmapFontOptions("Times", 18, FontStyle.Bold);
                fontOptions.UseTextRenderer = true;

                FontSurface font = new FontSurface(fontOptions);

                if (font.CanSave == false)
                {
                    return;
                }

                font.Save("testfont.xml");


				FontSurface second = FontSurface.LoadBitmapFont("testfont.png", "testfont.xml");

                while (wind.IsClosed == false)
                {
                    Display.BeginFrame();
                    Display.Clear(Color.Navy);

                    font.DrawText("The quick brown fox jumps over the lazy dog.");

                    second.DrawText(0, font.StringDisplayHeight("M"), "The quick brown fox jumps over the lazy dog.");

                    Display.EndFrame();
                    Core.KeepAlive();
                }
            }
        }
    }
}