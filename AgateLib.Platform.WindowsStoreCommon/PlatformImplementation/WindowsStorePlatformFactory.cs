using AgateLib.ApplicationModels;
using AgateLib.Drivers;
using AgateLib.IO;
using AgateLib.Platform.WindowsStore.PlatformImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Platform.WindowsStore.Factories
{
    public abstract class WindowsStorePlatformFactory : IPlatformFactory
    {
        public WindowsStorePlatformFactory(bool handheld, AssetLocations assetLocations)
        {
            Info = new WindowsStorePlatformInfo(handheld);
            ApplicationFolderFileProvider = new WindowsStoreAssetFileProvider(".");
            FileProvider.UserFiles = new IsolatedStorageFileProvider();
        }
        public PlatformInfo Info { get; private set; }
        public IReadFileProvider ApplicationFolderFileProvider { get; private set; }

        public IStopwatch CreateStopwatch()
        {
            return new DiagnosticsStopwatch();
        }

        public void Initialize(FileSystemObjects fileSystemObjects)
        {
            fileSystemObjects.File = new FakeFile();
            fileSystemObjects.Path = new FakePath();
        }

        public Diagnostics.AgateConsole CreateConsole()
        {
            return new AgateConsoleImpl();
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
