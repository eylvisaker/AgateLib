using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VermilionTower.ContentPipeline.Shims
{
    public interface IDirectory
    {
        IEnumerable<string> EnumerateFiles(string path);
        IEnumerable<string> EnumerateDirectories(string path);
        void CreateDirectory(string path);
    }

    public class Directory : IDirectory
    {
        public string PathRoot { get; set; }

        public void CreateDirectory(string path)
        {
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(PathRoot, path));
        }

        public IEnumerable<string> EnumerateDirectories(string path)
        {
            return System.IO.Directory.EnumerateDirectories(
                System.IO.Path.Combine(PathRoot, path))
                .Select(Localize);
        }

        private string Localize(string arg)
        {
            if (!arg.StartsWith(PathRoot))
                return arg;

            var result = arg.Substring(PathRoot.Length);

            if (result.StartsWith("/") || result.StartsWith("\\"))
                result = result.Substring(1);

            return result;
        }

        public IEnumerable<string> EnumerateFiles(string path)
        {
            return System.IO.Directory.EnumerateFiles(
                System.IO.Path.Combine(PathRoot, path))
                .Select(Localize);
        }
    }
}
