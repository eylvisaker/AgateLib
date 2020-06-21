using VermilionTower.ContentPipeline.Shims;
using System;
using System.Collections.Generic;
using System.Text;

namespace VermilionTower.ContentPipeline
{
    public class FileAccessor
    {
        public FileAccessor(IFileSystem fileSystem)
        {
            this.FileSystem = fileSystem;
        }

        protected IFileSystem FileSystem { get; }

        protected IFile File => FileSystem.File;

        protected IDirectory Directory => FileSystem.Directory;

        protected IPath Path => FileSystem.Path;
    }
}
