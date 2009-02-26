using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Utility;

namespace TgzProviderTester
{
    class TgzProviderTester
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            new TgzProviderTester().Run();
        }

        private void Run()
        {
            var tgz = new TgzFileProvider("archives/dogs.tar.gz");

            // These two lines are used by AgateLib tests to locate
            // driver plugins and images.
            AgateFileProvider.Assemblies.AddPath("../Drivers");
            AgateFileProvider.Images.Add(tgz);

            using (AgateSetup setup = new AgateSetup())
            {
                setup.AskUser = true;
                setup.Initialize(true, false, false);
                if (setup.WasCanceled)
                    return;

                DisplayWindow wind = DisplayWindow.CreateWindowed(
                    "TgzFileProvider Tester", 800, 600, false);

                Surface surf = new Surface("dogs.png");
                Surface surf2 = new Surface("bigpaddle.png");

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
