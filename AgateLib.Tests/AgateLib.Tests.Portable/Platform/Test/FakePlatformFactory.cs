using AgateLib.Drivers;
using AgateLib.Platform.Common.PlatformImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Testing.Platform.Test;

namespace AgateLib.Platform.Test
{
    public class FakePlatformFactory : IPlatformFactory
    {
        public FakePlatformFactory()
        {
            Info = new FakePlatformInfo();
            ApplicationFolderFileProvider = new FakeReadOnlyFileProvider();
        }

        public Platform.PlatformInfo Info { get; private set; }
        public IReadFileProvider ApplicationFolderFileProvider { get; protected set; }

        public Platform.IStopwatch CreateStopwatch()
        {
            return new DiagnosticsStopwatch();
        }

        public virtual void Initialize(IO.FileSystemObjects fileSystemObjects)
        {
        }

        public Diagnostics.AgateConsole CreateConsole()
        {
            return new FakeAgateConsole();
        }

        public IEnumerable<Assembly> GetSerializationSearchAssemblies(Type objectType)
        {
            yield break;
        }

        public IPlatformSerialization CreateDefaultSerializationConstructor()
        {
            return new PlatformSerialization();
        }
    }

}
