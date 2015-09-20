using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Testing.Platform.Test
{
    public class FakeReadOnlyFileProvider : IReadFileProvider
    {
        public Task<Stream> OpenReadAsync(string filename)
        {
            throw new NotImplementedException();
        }

        public bool FileExists(string filename)
        {
            return false;
        }

        public IEnumerable<string> GetAllFiles()
        {
            yield break;
        }

        public IEnumerable<string> GetAllFiles(string searchPattern)
        {
            yield break;
        }

        public string ReadAllText(string filename)
        {
            return "";
        }

        public bool IsRealFile(string filename)
        {
            return true;
        }

        public string ResolveFile(string filename)
        {
            return filename;
        }

        public bool IsLogicalFilesystem
        {
            get { return true; }
        }
    }
}
