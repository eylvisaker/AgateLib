using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Drivers;
using AgateLib.IO;
using AgateLib.Platform.Common.PlatformImplementation;
using AgateLib.Platform.Test;

namespace AgateLib.Platform.IntegrationTest
{
	class IntegrationTestPlatformFactory : IPlatformFactory
	{
		FakePlatformInfo info = new FakePlatformInfo();

		public IntegrationTestPlatformFactory(string appDirPath)
		{
			ApplicationFolderFileProvider = new FileSystemProvider(appDirPath);
		}

		public IReadFileProvider ApplicationFolderFileProvider { get; private set; }

		public IPlatformInfo Info => info;
		
		public IStopwatch CreateStopwatch()
		{
			return new DiagnosticsStopwatch();
		}

		public void Initialize(FileSystemObjects fileSystemObjects)
		{
		}
	}
}
