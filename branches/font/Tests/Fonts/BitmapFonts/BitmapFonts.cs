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
			AgateFileProvider.Assemblies.AddPath("../Drivers");
			AgateFileProvider.Images.AddPath("Images");
            
			using (AgateSetup setup = new AgateSetup())
            {
                setup.AskUser = true;
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

                if (font.CanSave == false)
                {
                    return;
                }

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