using System.Collections.Generic;
using VermilionTower.ContentPipeline.FolderContexts;

namespace VermilionTower.ContentPipeline
{
    public class ContentIndexFile
    {
        public CreditsRequirement? Credits { get; set; }
        public List<FileInstruction> Files { get; set; }
        public bool InheritFiles { get; set; } = true;
        public List<string> ExcludeFolders { get; set; }
    }

    public enum CreditsRequirement
    {
        None,
        Warn,
        Require,
    }
}