using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib;

namespace AppTester
{
    class App : AgateApplication  
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            new App().Run();
        }

        protected override AppInitParameters GetAppInitParameters()
        {
            var retval = base.GetAppInitParameters();

            return retval;
        }
    }
}
