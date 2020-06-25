using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AgateLib.Storage
{
    public class FakeUserStorage : IUserStorage
    {

        private Dictionary<string, MemoryStream> files = new Dictionary<string, MemoryStream>();

        public bool FileExists(string path)
        {
            var p = NormalizePath(path);

            return files.ContainsKey(p);
        }

        public IEnumerable<string> Files => files.Keys;

        public Stream OpenFile(string path, FileMode fileMode)
        {
            var p = NormalizePath(path);

            switch (fileMode)
            {
                case FileMode.Create:
                    files[p] = new MemoryStream();
                    break;

                case FileMode.Open:
                    if (!files.TryGetValue(p, out MemoryStream existing))
                    {
                        throw new FileNotFoundException("Could not find the file.", p);
                    }

                    files[p] = new MemoryStream(existing.ToArray());
                    break;
            }

            return files[p];
        }

        public string GetFileContents(string path)
        {
            var p = NormalizePath(path);

            return Encoding.UTF8.GetString(files[p].ToArray());
        }

        public Stream OpenFile(string path, FileMode fileMode, FileAccess fileAccess) => throw new NotImplementedException();
        public Stream OpenFile(string path, FileMode fileMode, FileAccess fileAccess, FileShare fileShare) => throw new NotImplementedException();

        private string NormalizePath(string path)
        {
            return path;
        }

        public void SetApplicationDataFolder(string folderName)
        { }

        public void DeleteFile(string path)
        {
            var p = NormalizePath(path);
            files.Remove(p);
        }
    }
}
