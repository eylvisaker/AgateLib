using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.ContentAssembler.FolderContexts
{
    public class InheritedFolderContext : FileAccessor, IFolderContext
    {
        private IFolderContext parentContext;
        private string path;
        private string mappedPath;

        public InheritedFolderContext(IFolderContext parentContext, string path, IFileSystem fileSystem)
            :base(fileSystem)
        {
            this.parentContext = parentContext;
            this.path = path;

            if (parentContext.MappedPath == null)
                mappedPath = Path.GetFileName(path);
            else
                mappedPath = Path.Combine(parentContext.MappedPath, path);
        }

        public CreditsRequirement Credits => parentContext.Credits;

        public string MappedPath => mappedPath;

        public IReadOnlyList<string> ExcludeFolders => parentContext.ExcludeFolders;

        public IReadOnlyList<FileInstruction> FileInstructions => parentContext.FileInstructions;

        public IEnumerable<CreateIndex> Indexes => Enumerable.Empty<CreateIndex>();

        public FileInstruction FindMatch(string outputFileName)
            => parentContext.FindMatch(outputFileName);

        public string GetOutputFileName(string fileName)
        {
            return Path.Combine(MappedPath, fileName);
        }
    }
}
