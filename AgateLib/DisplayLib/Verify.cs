using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgateLib.Quality;

namespace AgateLib.DisplayLib
{
    static class Verify
    {
        public static void DisplayIsInitialized()
        {
            Condition.Requires<AgateException>(Display.Impl != null, "AgateLib's display system has not been initialized.");
        }
    }
}
