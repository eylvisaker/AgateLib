using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Utility;

namespace Tests.TgzProviderTester
{
    class TgzProviderTester
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [AgateTest("TgzProvider Test", "Core")]
        static void Main()
        {
            new TgzProviderTester().Run();
        }

        private void Run()
        {
            var tgz = new TgzFileProvider("Data/dogs.tar.gz");

            using (AgateSetup setup = new AgateSetup())
            {
                setup.AskUser = true;
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
    }
}
