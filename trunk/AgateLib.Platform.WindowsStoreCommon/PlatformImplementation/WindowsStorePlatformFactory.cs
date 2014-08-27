using AgateLib.Drivers;
using AgateLib.Platform.WindowsStoreCommon.PlatformImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Platform.WindowsStoreCommon.Factories
{
	public abstract class WindowsStorePlatformFactory : IPlatformFactory
	{
		public WindowsStorePlatformFactory(bool handheld)
		{
			Info = new WPPlatformInfo(handheld);
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
