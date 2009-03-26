using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.Geometry;
using AgateLib.DisplayLib;

namespace TestXNA
{
    class TestXNA : AgateApplication
    {
        static void Main(string[] args)
        {
            new TestXNA().Run(args);
        }

        protected override void RenderSplashScreen(double time_ms)
        {
        }

        protected override void Render(double time_ms)
        {
            Display.Clear(Color.White);

            Surface surf = AgateLib.InternalResources.Data.PoweredBy;

            surf.Draw(0, 0);
        }

    }
}
