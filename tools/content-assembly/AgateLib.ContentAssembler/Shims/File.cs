using System;
using System.IO;

namespace AgateLib.ContentAssembler.Shims
{
    public interface IFile
    {
        string ReadAllText(string fileName);

        void Copy(string sourceFileName, string destFileName);
        void Delete(string path);
        Stream Open(string path, FileMode mode, FileAccess access);
        bool Exists(string path);
        DateTime GetLastWriteTimeUtc(string path);

        void WriteAllText(string path, string contents);
    }

    public class File : IFile
    {
        public string PathRoot { get; set; }

        public void Copy(string sourceFileName, string destFileName)
        {
            System.IO.File.Copy(Normalize(sourceFileName), Normalize(destFileName));
        }

        public void Delete(string path)
        {
            System.IO.File.Delete(Normalize(path));
        }

        public DateTime GetLastWriteTimeUtc(string path)
            => System.IO.File.GetLastWriteTimeUtc(Normalize(path));

        public Stream Open(string path, FileMode mode, System.IO.FileAccess access)
        {
            return System.IO.File.Open(Normalize(path), mode, access);
        }

        public string ReadAllText(string path)
        {
            return System.IO.File.ReadAllText(Normalize(path));
        }

        public bool Exists(string path)
            => System.IO.File.Exists(Normalize(path));

        public void WriteAllText(string path, string contents)
            => System.IO.File.WriteAllText(Normalize(path), contents);

        private string Normalize(string path)
            => System.IO.Path.Combine(PathRoot, path);
    }
}
