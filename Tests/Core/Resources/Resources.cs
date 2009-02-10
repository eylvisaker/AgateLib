using System;
using System.Collections.Generic;
using System.Text;
using AgateLib;
using AgateLib.Geometry;
using AgateLib.DisplayLib;
using AgateLib.Resources;

namespace ResourceTester
{
    class ResourceTester
    {
        static void Main(string[] args)
        {
			// These two lines are used by AgateLib tests to locate
			// driver plugins and images.
			AgateFileProvider.Assemblies.AddPath("../Drivers");
			AgateFileProvider.Images.AddPath("../../../Tests/TestImages");
			
			using (AgateSetup setup = new AgateSetup("Resource Tester", args))
            {
                setup.InitializeAll();
                if (setup.Cancel)
                    return;

                AgateResourceCollection resources = AgateResourceLoader.LoadResources("TestResourceFile.xml");

                DisplayWindow wind = new DisplayWindow(resources, "main_window");
                Surface surf = new Surface(resources, "sample_surf");
                ISprite sprite = new AgateLib.Sprites.NewSprite(resources, "sample_sprite");
                sprite.StartAnimation();

                while (wind.IsClosed == false)
                {
                    Display.BeginFrame();
                    Display.Clear(Color.Red);

                    surf.Draw(20, 20);

                    sprite.Update();
                    sprite.Draw(100, 100);

                    Display.EndFrame();
                    Core.KeepAlive();
                }
            }
        }
    }
}
