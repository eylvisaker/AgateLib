using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERY.AgateLib;
using ERY.AgateLib.Geometry;
using ERY.AgateLib.Resources;

namespace ResourceTester
{
    class ResourceTester
    {
        static void Main(string[] args)
        {
            using (AgateSetup setup = new AgateSetup("Resource Tester", args))
            {
                setup.InitializeAll();
                if (setup.Cancel)
                    return;

                AgateResourceManager resources = new AgateResourceManager();
                resources.Load("test.xml");

                DisplayWindow wind = new DisplayWindow(resources, "main_window");
                Surface surf = new Surface(resources, "sample_surf");

                while (wind.IsClosed == false)
                {
                    Display.BeginFrame();
                    Display.Clear(Color.Red);

                    surf.Draw(20, 20);
                    Display.EndFrame();
                    Core.KeepAlive();
                }
            }
        }
    }
}
