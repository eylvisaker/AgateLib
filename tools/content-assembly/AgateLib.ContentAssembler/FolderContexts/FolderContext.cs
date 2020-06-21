using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VermilionTower.ContentPipeline.FolderContexts
{
    public interface IFolderContext
    {
        string MappedPath { get; }

        IReadOnlyList<FileInstruction> FileInstructions { get; }

        string GetOutputFileName(string fileName);

        FileInstruction FindMatch(string outputFileName);

        IReadOnlyList<string> ExcludeFolders { get; }

        CreditsRequirement Credits { get; }
    }

    public class FolderContext : FileAccessor, IFolderContext
    {
        private ContentIndexFile contentIndexFile;
        private IFolderContext parentContext;
        private string pathRoot;
        private string mappedPath;
        private List<FileInstruction> instructions = new List<FileInstruction>();

        public FolderContext(ContentIndexFile contentIndexFile,
                             IFolderContext parentContext,
                             string pathRoot,
                             IFileSystem fileSystem)
            : base(fileSystem)
        {
            this.contentIndexFile = contentIndexFile;
            this.parentContext = parentContext ?? new DefaultFolderContext();
            this.pathRoot = pathRoot;

            if (parentContext == null)
                mappedPath = null;
            else if (parentContext.MappedPath == null)
                mappedPath = Path.GetFileName(pathRoot);
            else
                mappedPath = Path.Combine(parentContext.MappedPath, Path.GetFileName(pathRoot));

            if (contentIndexFile.Files == null)
            {
                instructions.AddRange(this.parentContext.FileInstructions);
            }
            else
            {
                instructions.AddRange(contentIndexFile.Files);

                if (contentIndexFile.InheritFiles)
                {
                    instructions.AddRange(this.parentContext.FileInstructions);
                }
            }

            ExcludeFolders = contentIndexFile.ExcludeFolders 
                ?? this.parentContext?.ExcludeFolders;

            Credits = contentIndexFile.Credits ?? this.parentContext.Credits;

        }

        public CreditsRequirement Credits { get; }

        public string MappedPath => mappedPath;

        public IReadOnlyList<FileInstruction> FileInstructions => instructions;

        public IReadOnlyList<string> ExcludeFolders { get; }

        public FileInstruction FindMatch(string outputFileName)
        {
            foreach (var instruction in instructions)
            {
                if (new Regex(instruction.Pattern).IsMatch(outputFileName))
                {
                    return instruction;
                }
            }

            return null;
        }

        public string GetOutputFileName(string fileName)
        {
            if (MappedPath == null)
                return fileName;

            return Path.Combine(MappedPath, fileName);
        }
    }
}