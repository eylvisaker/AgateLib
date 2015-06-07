using AgateLib.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AgateLib.Platform.WinForms.Factories
{
    class SysIoDirectory : IDirectory
    {
        public string[] GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }

        public IEnumerable<string> GetFiles(string path, string searchPattern)
        {
            return Directory.GetFiles(path, searchPattern);
        }
    }
}
