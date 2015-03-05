using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgateLib.Drivers;

namespace AgateLib.UnitTests.Fakes
{
    public class FakePlatformFactory : IPlatformFactory
    {
        public AgateLib.Platform.PlatformInfo Info
        {
            get { throw new NotImplementedException(); }
        }

        public AgateLib.Platform.IStopwatch CreateStopwatch()
        {
            throw new NotImplementedException();
        }

        public AgateLib.IO.IFile CreateFile()
        {
            throw new NotImplementedException();
        }

        public AgateLib.Diagnostics.AgateConsole CreateConsole()
        {
            throw new NotImplementedException();
        }

        public AgateLib.IO.IPath CreatePath()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<System.Reflection.Assembly> GetSerializationSearchAssemblies(Type objectType)
        {
            throw new NotImplementedException();
        }

        public IReadFileProvider ApplicationFolderFileProvider
        {
            get { throw new NotImplementedException(); }
        }

        public IPlatformSerialization CreateDefaultSerializationConstructor()
        {
            throw new NotImplementedException();
        }
    }
}
