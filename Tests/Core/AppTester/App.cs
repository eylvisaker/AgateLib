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
            // These two lines are used by AgateLib tests to locate
            // driver plugins and images.
            AgateFileProvider.Assemblies.AddPath("../Drivers");
            AgateFileProvider.Images.AddPath("Images");
			
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
