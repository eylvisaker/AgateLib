using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib;

namespace Tests.AppTester
{
    class App : AgateApplication  
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [AgateTest("AgateApplication Test", "Core")]
        static void Main()
        {
            new App().Run();
        }

        protected override AppInitParameters GetAppInitParameters()
        {
            var retval = base.GetAppInitParameters();
            retval.AllowResize = true;
            return retval;
        }
    }
}
