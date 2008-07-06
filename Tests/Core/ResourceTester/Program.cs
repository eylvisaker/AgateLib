using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERY.AgateLib;
using ERY.AgateLib.Geometry;
using ERY.AgateLib.Resources;

namespace ResourceTester
{
    class Program
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

                while (wind.IsClosed == false)
                {
                    Display.BeginFrame();
                    Display.Clear(Color.Red);

                    Display.EndFrame();
                    Core.KeepAlive();
                }
            }
        }
    }
}
