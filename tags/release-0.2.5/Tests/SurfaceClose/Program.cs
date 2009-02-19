using System;
using System.Collections.Generic;
using System.IO;
using ERY.AgateLib;

namespace SurfaceClose
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (AgateSetup setup = new AgateSetup())
            {
                setup.AskUser = true;
                setup.InitializeDisplay();
                if (setup.Cancel)
                    return;

                string sourceFile = "test.png";
                string destFile = "test-deleteme.png";

                DisplayWindow wind = new DisplayWindow("Test", 300, 200);

                File.Copy(sourceFile, destFile, true);

                Surface surf = new Surface(destFile);

                File.Delete(destFile);

                while (wind.Closed == false)
                {
                    Display.BeginFrame();
                    Display.Clear();

                    surf.Draw(10, 10);

                    Display.EndFrame();
                    Core.KeepAlive();
                }
            }
        }
    }
}
