using VermilionTower.ContentPipeline.FileProcessors;
using System.Collections.Generic;

namespace VermilionTower.ContentPipeline
{
    public class ContentIndex
    {
        public ContentIndex(IFileSystem fileSystem)
        {
            Credits = new CreditsCollection(fileSystem);
        }

        public Dictionary<string, FileContent> OutputFiles { get; } = new Dictionary<string, FileContent>();

        public CreditsCollection Credits { get; }
    }

    public class FileContent
    {
        public IFileSource Source { get; set; }

        public MonoGameParameters MonoGame { get; set; }

        public CreditsRequirement Credits { get; set; }
    }

    public class MonoGameParameters
    {
        public AdditionType AdditionType { get; set; }

        public string Importer { get; set; }

        public string Processor { get; set; }

        public List<string> ProcessorParams { get; set; } = new List<string>();
    }

    public enum AdditionType
    {
        Copy,
        Import,
    }
}