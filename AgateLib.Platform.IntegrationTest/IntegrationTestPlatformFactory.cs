using System;
using System.Collections.Generic;
using System.IO;
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
		private string appDirPath;

		public IntegrationTestPlatformFactory(string appDirPath)
		{
			this.appDirPath = appDirPath;
			ApplicationFolderFiles = new FileSystemProvider(appDirPath);
		}

		public IReadFileProvider ApplicationFolderFiles { get; private set; }

		public IPlatformInfo Info => info;
		
		public IStopwatch CreateStopwatch()
		{
			return new DiagnosticsStopwatch();
		}

		public IReadFileProvider OpenAppFolder(string subpath)
		{
			return new FileSystemProvider(Path.Combine(appDirPath, subpath));
		}

		public IReadWriteFileProvider OpenUserAppStorage(string subpath)
		{
			return new FileSystemProvider(Path.Combine(appDirPath, subpath));
		}
	}
}
