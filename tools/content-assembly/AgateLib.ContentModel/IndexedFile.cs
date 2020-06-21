using System.Diagnostics;

namespace AgateLib.ContentModel
{
    [DebuggerDisplay("IndexFile {Type}: {Path}")]
    public class IndexedFile
    {
        public string Path { get; set; }
        public ContentType Type { get; set; }
    }

    public enum ContentType
    {
        Raw,
        Texture,
        SoundEffect,
        Music,
    }
}
