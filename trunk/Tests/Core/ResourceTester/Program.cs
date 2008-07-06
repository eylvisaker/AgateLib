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

            }
        }
    }
}
