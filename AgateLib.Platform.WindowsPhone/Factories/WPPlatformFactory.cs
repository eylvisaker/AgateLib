using AgateLib.Drivers;
using AgateLib.Platform.WindowsPhone.PlatformImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Platform.WindowsPhone.Factories
{
	class WPPlatformFactory : IPlatformFactory
	{
		public WPPlatformFactory()
		{
			Info = new WPPlatformInfo();
			AssetFileProvider = new WPAssetFileProvider();

		}
		public PlatformInfo Info { get; private set;}
		public IReadFileProvider AssetFileProvider { get; private set; }
		
		public IStopwatch CreateStopwatch()
		{
			return new DiagnosticsStopwatch();
		}

		public IO.IFile CreateFile()
		{
			return new FakeFile();
		}

		public Diagnostics.AgateConsole CreateConsole()
		{
			return null;
		}

		public IO.IPath CreatePath()
		{
			return new FakePath();
		}

		public IEnumerable<System.Reflection.Assembly> GetSerializationSearchAssemblies(Type objectType)
		{
			throw new NotImplementedException();
		}


		public IPlatformSerialization CreateDefaultSerializationConstructor()
		{
			throw new NotImplementedException();
		}
	}
}
