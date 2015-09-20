using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.ApplicationModels;

namespace AgateLib.Platform.Test
{
    public static class UnitTestPlatform
    {
        public static void Initialize(ModelParameters parameters, bool useRealFilesystem, string appDirPath)
        {
            Core.Initialize(new FakeAgateFactory());
            Core.InitAssetLocations(parameters.AssetLocations);
        }
    }
}
