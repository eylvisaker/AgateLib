using AgateLib.Platform.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Platform.IntegrationTest
{
    class IntegrationTestPlatformFactory : FakePlatformFactory
    {
        public IntegrationTestPlatformFactory(string appDirPath)
        {
            ApplicationFolderFileProvider = new FileSystemProvider(appDirPath);
        }
    }
}
