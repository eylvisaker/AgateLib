using System.Collections.Generic;
using System.Diagnostics;

namespace AgateLib.ContentAssembler.FolderContexts
{
    [DebuggerDisplay("{Pattern} AS {As}")]
    public class FileInstruction
    {
        public FileProcessType As { get; set; }
        public string Pattern { get; set; }
        public string Importer { get; set; }
        public string Processor { get; set; }
        public List<string> ProcessorParams { get; set; }
    }

    public enum FileProcessType
    {
        Copy,
        Build,
        Credits,
        Ignore,
        Texture,
        Effect,
        Song,
        SoundEffect,
    }
}