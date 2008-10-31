using System;
using System.Collections.Generic;

using AgateLib;
using AgateLib.BitmapFont;
using AgateLib.Display;
using AgateLib.Geometry;
using AgateLib.Input;

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

                BitmapFontOptions fontOptions = new BitmapFontOptions("Times", 14, FontStyle.Bold);
                fontOptions.UseTextRenderer = true;

                FontSurface font = new FontSurface(fontOptions);

                if (font.CanSave == false)
                {
                    return;
                }

                font.Save("testfont.xml");


                FontSurface second = FontSurface.LoadBitmapFont("test.png", "testfont.xml");

                while (wind.IsClosed == false)
                {
                    Display.BeginFrame();
                    Display.Clear(Color.Navy);

                    font.DrawText("Chonk");

                    second.DrawText(0, 70, "sdlkfj");

                    Display.EndFrame();
                    Core.KeepAlive();
                }
            }
        }
    }
}