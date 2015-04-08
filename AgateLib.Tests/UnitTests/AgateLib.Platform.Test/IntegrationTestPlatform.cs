using AgateLib.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Platform.Test;
using AgateLib.Quality;

namespace AgateLib.Platform.Test
{
    /// <summary>
    /// Initializes AgateLib for doing integration testing, using the physical file system.
    /// </summary>
    public static class IntegrationTestPlatform
    {
        public static void Initialize(ModelParameters parameters, string appDirPath)
        {
            Condition.Requires<ArgumentException>(string.IsNullOrWhiteSpace(appDirPath) == false, "appDirPath");

            Core.Initialize(new FakeAgateFactory(new IntegrationTestPlatformFactory(appDirPath)));
            Core.InitAssetLocations(parameters.AssetLocations);
        }
    }
}
