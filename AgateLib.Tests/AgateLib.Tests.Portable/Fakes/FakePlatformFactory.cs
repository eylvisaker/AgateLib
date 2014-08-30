using AgateLib.Drivers;
using AgateLib.Platform.Common.PlatformImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Testing.Fakes
{
	class FakePlatformFactory : IPlatformFactory
	{
		public FakePlatformFactory()
		{
			Info = new FakePlatformInfo();
		}
		public Platform.PlatformInfo Info { get; private set; }
		public IReadFileProvider AssetFileProvider { get; private set;}

		public Platform.IStopwatch CreateStopwatch()
		{
			return new DiagnosticsStopwatch();
		}

		public IO.IFile CreateFile()
		{

			return null;
		}

		public Diagnostics.AgateConsole CreateConsole()
		{
			throw new NotImplementedException();
		}

		public IO.IPath CreatePath()
		{
			return null;
		}

		public IEnumerable<System.Reflection.Assembly> GetSerializationSearchAssemblies(Type objectType)
		{
			yield break;
		}


		public IPlatformSerialization CreateDefaultSerializationConstructor()
		{
			return new PlatformSerialization();
		}
	}
	
}
