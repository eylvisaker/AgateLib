using AgateLib.Drivers;
using AgateLib.IO;
using AgateLib.Platform.WindowsForms.PlatformImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsForms.Factories
{
	class PlatformFactory : IPlatformFactory
	{
		public PlatformInfo CreatePlatformInfo()
		{
			return new FormsPlatformInfo();
		}

		public IStopWatch CreateStopwatch()
		{
			return new DiagnosticsStopwatch();
		}


		public IFile CreateFile()
		{
			return new SysIoFile();
		}
	}
}
