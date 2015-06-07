using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.IO
{
    public class FileSystemObjects
    {
        public IFile File { get; set; }
        public IPath Path { get; set; }
        public IDirectory Directory { get; set; }
    }
}
