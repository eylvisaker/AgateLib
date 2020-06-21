using System.Collections.Generic;
using System.Linq;

namespace AgateLib.ContentAssembler.FolderContexts
{
    public class DefaultFolderContext : IFolderContext
    {
        public string MappedPath => null;

        public IReadOnlyList<string> ExcludeFolders { get; } = new List<string>();

        public IReadOnlyList<FileInstruction> FileInstructions { get; } = new List<FileInstruction>
        {
            new FileInstruction
            {
                Pattern = @".*\.fx",
                As = FileProcessType.Effect,
            },
            new FileInstruction
            {
                Pattern = @".*\.png",
                As = FileProcessType.Texture,
            },
            new FileInstruction
            {
                Pattern = @".*\.ogg",
                As = FileProcessType.Song,
                Importer = "OggImporter",
            },
            new FileInstruction
            {
                Pattern = @".*\.mp3",
                As = FileProcessType.Song,
                Importer = "Mp3Importer",
            },
            new FileInstruction
            {
                Pattern = @".*\.wav",
                As = FileProcessType.SoundEffect,
                Importer = "WavImporter",
            },
            new FileInstruction
            {
                Pattern = @".*CREDITS.txt",
                As = FileProcessType.Credits,
            }
        };

        public CreditsRequirement Credits => CreditsRequirement.Warn;

        public IEnumerable<CreateIndex> Indexes => Enumerable.Empty<CreateIndex>();

        public FileInstruction FindMatch(string outputFileName)
        {
            throw new System.NotImplementedException();
        }

        public string GetOutputFileName(string fileName)
        {
            throw new System.NotImplementedException();
        }
    }
}